using ICMSDemo.TestingTemplates;
using Abp.Organizations;
using ICMSDemo.Authorization.Users;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.WorkingPaperNews.Dtos;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ICMSDemo.WorkingPapers;
using ICMSDemo.Departments;
using Abp.UI;
using ICMSDemo.Projects;
using Abp.Authorization.Users;
using Abp.Timing;

namespace ICMSDemo.WorkingPaperNews
{
    [AbpAuthorize(AppPermissions.Pages_WorkingPaperNews)]
    public class WorkingPaperNewsAppService : ICMSDemoAppServiceBase, IWorkingPaperNewsAppService
    {
        private readonly IRepository<WorkingPaper, Guid> _workingPaperNewRepository;
        private readonly IRepository<WorkingPaperDetail> _workingPaperDetailsRepository;
        private readonly IRepository<TestingTemplate, int> _lookup_testingTemplateRepository;
        private readonly IRepository<TestingAttrribute, int> _lookup_testingAttributeRepository;
        private readonly IRepository<Department, long> _lookup_organizationUnitRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<Project> _lookup_projectRepository;
        private readonly IRepository<UnitOrganizationRole,long> _lookup_ouRoleRepository;



        public WorkingPaperNewsAppService(IRepository<WorkingPaper, Guid> workingPaperNewRepository,
            IRepository<TestingAttrribute, int> lookup_testingAttributeRepository, IRepository<UnitOrganizationRole, long> lookup_ouRoleRepository,
            IRepository<WorkingPaperDetail> workingPaperDetailsRepository, IRepository<Project> lookup_projectRepository,
            IRepository<TestingTemplate, int> lookup_testingTemplateRepository, IRepository<Department, long> lookup_organizationUnitRepository, IRepository<User, long> lookup_userRepository)
        {
            _workingPaperNewRepository = workingPaperNewRepository;
            _lookup_testingTemplateRepository = lookup_testingTemplateRepository;
            _lookup_projectRepository = lookup_projectRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_testingAttributeRepository = lookup_testingAttributeRepository;
            _workingPaperDetailsRepository = workingPaperDetailsRepository;
            _lookup_ouRoleRepository = lookup_ouRoleRepository;
        }

        public async Task<PagedResultDto<GetWorkingPaperNewForViewDto>> GetAll(GetAllWorkingPaperNewsInput input)
        {
            var taskStatusFilter = (TaskStatus)input.TaskStatusFilter;

            var filteredWorkingPaperNews = _workingPaperNewRepository.GetAll()
                        .Include(e => e.TestingTemplate)
                        .Include(e => e.CompletedBy)
                        .Include(e => e.ReviewedBy)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Comment.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(input.MinTaskDateFilter != null, e => e.TaskDate >= input.MinTaskDateFilter)
                        .WhereIf(input.MaxTaskDateFilter != null, e => e.TaskDate <= input.MaxTaskDateFilter)
                        .WhereIf(input.MinDueDateFilter != null, e => e.DueDate >= input.MinDueDateFilter)
                        .WhereIf(input.MaxDueDateFilter != null, e => e.DueDate <= input.MaxDueDateFilter)
                        .WhereIf(input.TaskStatusFilter > -1, e => e.TaskStatus == taskStatusFilter)
                        .WhereIf(input.MinCompletionDateFilter != null, e => e.CompletionDate >= input.MinCompletionDateFilter)
                        .WhereIf(input.MaxCompletionDateFilter != null, e => e.CompletionDate <= input.MaxCompletionDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TestingTemplateCodeFilter), e => e.TestingTemplate != null && e.TestingTemplate.Code == input.TestingTemplateCodeFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.CompletedById != null && e.CompletedBy.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.ReviewedById != null && e.ReviewedBy.Name == input.UserName2Filter)
                        .WhereIf(input.ProjectId != null && input.ProjectId > 0, e => e.ProjectId == input.ProjectId);

            var pagedAndFilteredWorkingPaperNews = filteredWorkingPaperNews
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var workingPaperNews = from o in pagedAndFilteredWorkingPaperNews
                                   join o1 in _lookup_testingTemplateRepository.GetAll() on o.TestingTemplateId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   join o2 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o2.Id into j2
                                   from s2 in j2.DefaultIfEmpty()

                                   join o3 in _lookup_userRepository.GetAll() on o.CompletedById equals o3.Id into j3
                                   from s3 in j3.DefaultIfEmpty()

                      
                                   join o4 in _lookup_userRepository.GetAll() on o.AssignedToId equals o4.Id into j4
                                   from s4 in j4.DefaultIfEmpty()

                                   join o5 in _lookup_projectRepository.GetAll() on o.ProjectId equals o5.Id into j5
                                   from s5 in j5.DefaultIfEmpty()

                                   select new GetWorkingPaperNewForViewDto()
                                   {
                                       WorkingPaperNew = new WorkingPaperNewDto
                                       {
                                           Code = o.Code,
                                           Comment = o.Comment,
                                           TaskDate = o.TaskDate,
                                           DueDate = o.DueDate,
                                           ProjectId = o.ProjectId,
                                           TaskStatus = o.TaskStatus,
                                           Score = o.Score,
                                           ReviewedDate = o.ReviewedDate,
                                           CompletionDate = o.CompletionDate,
                                           Id = o.Id
                                       },
                                       TestingTemplateCode = s1 == null ? "" : s1.Code.ToString(),
                                       OrganizationUnitDisplayName = s2 == null ? "" : s2.DisplayName.ToString(),
                                       UserName = s3 == null ? "" : s3.FullName.ToString(),
                                       UserName2 = s4 == null ? "" : s4.Name.ToString(),
                                       Frequency = s1 == null ? (Frequency?)null : s1.Frequency,
                                       SampleSize = s1 == null ? 0 : s1.SampleSize,
                                       TestingTemplateName = s1 == null ? "" : s1.Title,
                                       ProjectName = s5 == null ? "" : s5.Title
                                   };

            var totalCount = await filteredWorkingPaperNews.CountAsync();

            return new PagedResultDto<GetWorkingPaperNewForViewDto>(
                totalCount,
                await workingPaperNews.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPaperNews_Update)]
        public async Task<GetWorkingPaperNewForEditOutput> GetWorkingPaperNewForEdit(EntityDto<Guid> input)
        {
            var workingPaperNew = await _workingPaperNewRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetWorkingPaperNewForEditOutput { WorkingPaperNew = new CreateOrEditWorkingPaperNewDto { 
                Code = workingPaperNew.Code,
                Comment = workingPaperNew.Comment,
                CompletionDate = workingPaperNew.CompletionDate,
                CompletedUserId = workingPaperNew.CompletedById,
                DueDate = workingPaperNew.DueDate,
                Id = workingPaperNew.Id,
                OrganizationUnitId = workingPaperNew.OrganizationUnitId,
                ReviewDate = workingPaperNew.ReviewedDate,
                ReviewedUserId = workingPaperNew.ReviewedById,
                Score = workingPaperNew.Score,
                TaskDate = workingPaperNew.TaskDate,
                TaskStatus = workingPaperNew.TaskStatus,
                TestingTemplateId = workingPaperNew.TestingTemplateId
            } };

            if (output.WorkingPaperNew.TestingTemplateId != null)
            {
                var _lookupTestingTemplate = await _lookup_testingTemplateRepository.FirstOrDefaultAsync((int)output.WorkingPaperNew.TestingTemplateId);
                output.TestingTemplateCode = _lookupTestingTemplate.Code.ToString();
                output.TestingTemplate = new TestingTemplates.Dtos.GetTestingTemplateForViewDto()
                {
                    TestingTemplate = ObjectMapper.Map<TestingTemplates.Dtos.TestingTemplateDto>(_lookupTestingTemplate)
                };
            }

            if (output.WorkingPaperNew.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.WorkingPaperNew.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

            if (output.WorkingPaperNew.CompletedUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.WorkingPaperNew.CompletedUserId);
                output.UserName = _lookupUser.Name.ToString();
            }

            if (output.WorkingPaperNew.ReviewedUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.WorkingPaperNew.ReviewedUserId);
                output.UserName2 = _lookupUser.Name.ToString();
            }

            //Get working paper details 
            var workingPaperDetils = await _workingPaperDetailsRepository.GetAll().Where(x => x.WorkingPaperId == input.Id)
                                         .Select(x => new CreateOrEditTestingAttributeDto
                                         {
                                             Sequence = x.Sequence,
                                             TestingAttrributeId = x.TestingAttrributeId,
                                             Result = x.Result,
                                             Weight = x.Weight
                                         }).ToListAsync();
            output.WorkingPaperDetails = workingPaperDetils;

            if (workingPaperDetils.Count > 0)
            {
                output.LastSequence = workingPaperDetils.Max(x => x.Sequence);
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditWorkingPaperNewDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }


        protected virtual async Task Create(CreateOrEditWorkingPaperNewDto input)
        {
            var testingTemplateAttributes = await _lookup_testingAttributeRepository.GetAllListAsync(x => x.TestingTemplateId == input.TestingTemplateId);
            
            if (testingTemplateAttributes == null)
            {
                throw new UserFriendlyException("There is no testing template for this document.");
            }
            var testingTemplate = await _lookup_testingTemplateRepository.FirstOrDefaultAsync((int)input.TestingTemplateId);


            var workingPaperNew = new WorkingPaper();

            workingPaperNew.Comment = input.Comment;
            workingPaperNew.OrganizationUnitId  = (long)input.OrganizationUnitId;
            workingPaperNew.TestingTemplateId  = input.TestingTemplateId;

            if (AbpSession.TenantId != null)
            {
                workingPaperNew.TenantId = (int)AbpSession.TenantId;
            }
            workingPaperNew.Code = DateTime.Now.ToString("YYMMdd") + "-" + Guid.NewGuid().ToString().ToUpper().Substring(1, 5);
            workingPaperNew.CompletedById = AbpSession.UserId;
            workingPaperNew.CompletionDate = DateTime.Now;
            workingPaperNew.DueDate = DateTime.Now.AddDays(1);
            workingPaperNew.TaskStatus = input.Attributes.Length >= testingTemplate.SampleSize ? TaskStatus.PendingReview : TaskStatus.Open;
            workingPaperNew.TaskDate = DateTime.Now;

            var id = await _workingPaperNewRepository.InsertAndGetIdAsync(workingPaperNew);
            await CurrentUnitOfWork.SaveChangesAsync();
            workingPaperNew.Score = await SaveWorkingPaperDetails(input.Attributes, testingTemplateAttributes, id);

            //Save score
            workingPaperNew.Id = id;
            await _workingPaperNewRepository.UpdateAsync(workingPaperNew);
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPaperNews_Update)]
        protected virtual async Task Update(CreateOrEditWorkingPaperNewDto input)
        {
            var workingPaperNew = await _workingPaperNewRepository.FirstOrDefaultAsync((Guid)input.Id);
            var previousState = workingPaperNew.TaskStatus;

            //var testingTemplate = await _lookup_testingTemplateRepository.FirstOrDefaultAsync((int)input.TestingTemplateId);


            var testingTemplateAttributes = await _lookup_testingAttributeRepository.GetAllListAsync(x => x.TestingTemplateId == input.TestingTemplateId);

            if (testingTemplateAttributes == null)
            {
                throw new UserFriendlyException("There is no testing template for this document.");
            }

            input.Score = await SaveWorkingPaperDetails(input.Attributes, testingTemplateAttributes, workingPaperNew.Id);
            
            if (previousState == TaskStatus.Open && input.TaskStatus == TaskStatus.PendingReview)
            {
                input.CompletionDate = DateTime.Now;
                input.CompletedUserId = AbpSession.UserId;

               var newAssignee =  await _lookup_ouRoleRepository.FirstOrDefaultAsync(x => x.DepartmentRole == DepartmentRole.ControlHead && x.OrganizationUnitId == workingPaperNew.OrganizationUnitId);
                workingPaperNew.AssignedToId = newAssignee.UserId;
            }
        
           
            ObjectMapper.Map(input, workingPaperNew);
        }


        public async Task ApproveWorkPaper(EntityDto<Guid> input)
        {
            var workingPaperNew = await _workingPaperNewRepository.FirstOrDefaultAsync((Guid)input.Id);
            workingPaperNew.ReviewedById = AbpSession.UserId;
            workingPaperNew.ReviewedDate = Clock.Now;
            workingPaperNew.TaskStatus = TaskStatus.Approved;
            await _workingPaperNewRepository.UpdateAsync(workingPaperNew);
        }

        private async Task<decimal> SaveWorkingPaperDetails(CreateOrEditTestingAttributeDto[] input, List<TestingAttrribute> testingAttrributes, Guid workingPaperId)
        {
            decimal totalNumber = 0.00M;
            decimal workPaperTotal = 0.00M;
            List<WorkingPaperDetail> workingPaperDetails = new List<WorkingPaperDetail>();
            

            foreach (var item in input)
            {
                var testingTemplateAttribute = testingAttrributes.FirstOrDefault(x => x.Id == item.TestingAttrributeId);

                if (testingTemplateAttribute == null)
                {
                    throw new UserFriendlyException(string.Format("This test attribute ({0}) no longer exists.", item.AttributeText));
                }

                var workPaperDetail = new WorkingPaperDetail()
                {
                    WorkingPaperId = workingPaperId,
                    Sequence = item.Sequence,
                    TestingAttrributeId = item.TestingAttrributeId,
                    Result = item.Result,
                     Comments = item.Comments,
                      
                    Identifier = item.SampleIdentifier,
                    TenantId = (int)AbpSession.TenantId,
                    Weight = testingTemplateAttribute.Weight,
                    Score = item.Result ? testingTemplateAttribute.Weight : 0
                };

                workingPaperDetails.Add(workPaperDetail);
                totalNumber += workPaperDetail.Weight;
                workPaperTotal += workPaperDetail.Score;
            }
            foreach (var item in workingPaperDetails)
            {
                await _workingPaperDetailsRepository.InsertAsync(item);
            }

            return totalNumber <= 0 ? 0 : Math.Round((workPaperTotal / totalNumber) * 100.0M, 2);
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPaperNews_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _workingPaperNewRepository.DeleteAsync(input.Id);
        }

        public async Task AssignToUser(AssignWorkingPaperNewDto input)
        {
            var workingPaperNew = await _workingPaperNewRepository.FirstOrDefaultAsync((Guid)input.Id);
            workingPaperNew.AssignedToId = input.UserId;
            await _workingPaperNewRepository.UpdateAsync(workingPaperNew);
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPaperNews)]
        public async Task<PagedResultDto<WorkingPaperNewTestingTemplateLookupTableDto>> GetAllTestingTemplateForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_testingTemplateRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Code.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var testingTemplateList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<WorkingPaperNewTestingTemplateLookupTableDto>();
            foreach (var testingTemplate in testingTemplateList)
            {
                lookupTableDtoList.Add(new WorkingPaperNewTestingTemplateLookupTableDto
                {
                    Id = testingTemplate.Id,
                    DisplayName = testingTemplate.Title + " (" + testingTemplate.Code?.ToString() + ")"
                });
            }

            return new PagedResultDto<WorkingPaperNewTestingTemplateLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPaperNews)]
        public async Task<PagedResultDto<WorkingPaperNewOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_organizationUnitRepository.GetAll()
               .Where(x => !x.IsAbstract)
               .WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<WorkingPaperNewOrganizationUnitLookupTableDto>();
            foreach (var organizationUnit in organizationUnitList)
            {
                lookupTableDtoList.Add(new WorkingPaperNewOrganizationUnitLookupTableDto
                {
                    Id = organizationUnit.Id,
                    DisplayName = organizationUnit.Name?.ToString()
                });
            }

            return new PagedResultDto<WorkingPaperNewOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPaperNews)]
        public async Task<PagedResultDto<WorkingPaperNewUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
           var user = await  UserManager.Users.FirstOrDefaultAsync(x => x.Id == AbpSession.UserId.Value);

            var organizationUnits = await UserManager.GetOrganizationUnitsAsync(user);

            var organizationUnitsIdLists = organizationUnits.Select(x => x.Id).ToList();

            var userUnitsQuery = from o in _lookup_ouRoleRepository.GetAll()
                   
                                 .Where(x => x.DepartmentRole == DepartmentRole.ControlTeamMember && organizationUnitsIdLists.Any(y => y == x.OrganizationUnitId))
                            join u in _lookup_userRepository.GetAll() on o.UserId equals u.Id
                            select new WorkingPaperNewUserLookupTableDto
                            {
                                Id = u.Id,
                                 DisplayName = u.FullName
                            };


            var userInMyUnits = await userUnitsQuery.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.DisplayName.Contains(input.Filter)).ToListAsync();
         


            List <WorkingPaperNewUserLookupTableDto> output = new List<WorkingPaperNewUserLookupTableDto>();

            foreach (var u in userInMyUnits)
            {
                if (output.Count(x => x.Id == u.Id) == 0)
                     output.Add(u);
            }

            var totalCount = output.Count();
          
            return new PagedResultDto<WorkingPaperNewUserLookupTableDto>(
                totalCount,
                output.Skip(input.SkipCount).Take(input.MaxResultCount).ToList()
            );
        }


        public async Task<List<CreateOrEditTestingAttributeDto>> GetTestAttributesForTemplate(int testTemplateId)
        {
            var attributesToTest = await _lookup_testingAttributeRepository.GetAllListAsync(x => x.TestingTemplateId == testTemplateId);

            List<CreateOrEditTestingAttributeDto> output = new List<CreateOrEditTestingAttributeDto>();

            foreach (var item in attributesToTest)
            {
                output.Add(new CreateOrEditTestingAttributeDto()
                {
                    AttributeText = item.TestAttribute,
                    TestingAttrributeId = item.Id,
                    Result = false
                });
            }

            return output;
        }



    }
}