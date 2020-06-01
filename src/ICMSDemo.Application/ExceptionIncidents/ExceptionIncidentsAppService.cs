using ICMSDemo.ExceptionTypes;
using ICMSDemo.Authorization.Users;
using ICMSDemo.TestingTemplates;
using Abp.Organizations;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.ExceptionIncidents.Exporting;
using ICMSDemo.ExceptionIncidents.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ICMSDemo.WorkingPapers;
using ICMSDemo.Departments;
using ICMSDemo.ExceptionTypeColumns;
using Abp.UI;
using System.Text.Json;

namespace ICMSDemo.ExceptionIncidents
{
    [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents)]
    public class ExceptionIncidentsAppService : ICMSDemoAppServiceBase, IExceptionIncidentsAppService
    {
        private readonly IRepository<ExceptionIncident> _exceptionIncidentRepository;
        private readonly IRepository<UnitOrganizationRole,long> _unitOrganizationRoleRepository;

        private readonly IRepository<ExceptionIncidentColumn> _exceptionIncidentColumnRepository;
        private readonly IRepository<ExceptionTypeColumn> _exceptionTypeColumnColumnRepository;
        private readonly IExceptionIncidentsExcelExporter _exceptionIncidentsExcelExporter;
        private readonly IRepository<ExceptionType, int> _lookup_exceptionTypeRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<WorkingPaper, Guid> _lookup_workingPaperTemplateRepository;
        private readonly IRepository<Department, long> _lookup_organizationUnitRepository;


        public ExceptionIncidentsAppService(IRepository<ExceptionIncident> exceptionIncidentRepository,
            IRepository<ExceptionIncidentColumn> exceptionIncidentColumnRepository,
            IRepository<UnitOrganizationRole, long> unitOrganizationRoleRepository,
             IRepository<ExceptionTypeColumn> exceptionTypeColumnColumnRepository,
            IExceptionIncidentsExcelExporter exceptionIncidentsExcelExporter, IRepository<ExceptionType, int> lookup_exceptionTypeRepository, IRepository<User, long> lookup_userRepository,
          IRepository<WorkingPaper, Guid> lookup_workingPaperTemplateRepository,
            IRepository<Department, long> lookup_organizationUnitRepository)
        {
            _exceptionIncidentRepository = exceptionIncidentRepository;
            _exceptionTypeColumnColumnRepository = exceptionTypeColumnColumnRepository;
            _exceptionIncidentsExcelExporter = exceptionIncidentsExcelExporter;
            _lookup_exceptionTypeRepository = lookup_exceptionTypeRepository;
            _lookup_userRepository = lookup_userRepository;
            _unitOrganizationRoleRepository = unitOrganizationRoleRepository;
            _lookup_workingPaperTemplateRepository = lookup_workingPaperTemplateRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _exceptionIncidentColumnRepository = exceptionIncidentColumnRepository;

        }

        public async Task<PagedResultDto<GetExceptionIncidentForViewDto>> GetAll(GetAllExceptionIncidentsInput input)
        {
            var departments = await UserManager.GetOrganizationUnitsAsync(await UserManager.GetUserByIdAsync((long)AbpSession.UserId));


            string previousCode = string.Empty;
            List<string> codes = new List<string>();

            List<Department> allDepartments = await _lookup_organizationUnitRepository.GetAllListAsync();

            foreach(var item in departments)
            {
                var departmentCode = await OrganizationUnitManager.GetCodeAsync(item.Id);
                var childrenDept = allDepartments.Where(x => x.Code.StartsWith(item.Code)).Select(x => x.Code).ToList();
                codes.AddRange(childrenDept);
            }         


            var statusFilter = (Status)input.StatusFilter;

            var filteredExceptionIncidents = _exceptionIncidentRepository.GetAll()
                        .Include(e => e.ExceptionTypeFk)
                        .Include(e => e.RaisedByFk)

                        .Include(e => e.OrganizationUnitFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ClosureComments.Contains(input.Filter) || e.RaisedByClosureComments.Contains(input.Filter))
                        .WhereIf(input.MinDateFilter != null, e => e.Date >= input.MinDateFilter)
                        .WhereIf(input.MaxDateFilter != null, e => e.Date <= input.MaxDateFilter)
                        .WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(input.MinClosureDateFilter != null, e => e.ClosureDate >= input.MinClosureDateFilter)
                        .WhereIf(input.MaxClosureDateFilter != null, e => e.ClosureDate <= input.MaxClosureDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExceptionTypeNameFilter), e => e.ExceptionTypeFk != null && e.ExceptionTypeFk.Name == input.ExceptionTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.RaisedByFk != null && e.RaisedByFk.Name == input.UserNameFilter)

                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

            var pagedAndFilteredExceptionIncidents = filteredExceptionIncidents;

            var exceptionIncidents = from o in pagedAndFilteredExceptionIncidents
                                     join o1 in _lookup_exceptionTypeRepository.GetAll() on o.ExceptionTypeId equals o1.Id into j1
                                     from s1 in j1.DefaultIfEmpty()

                                     join o2 in _lookup_userRepository.GetAll() on o.RaisedById equals o2.Id into j2
                                     from s2 in j2.DefaultIfEmpty()


                                     join o4 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o4.Id into j4
                                     from s4 in j4.DefaultIfEmpty()

                                     select new GetExceptionIncidentForViewDto()
                                     {
                                         ExceptionIncident = new ExceptionIncidentDto
                                         {
                                             Code = o.Code,
                                             Date = o.Date,
                                             Description = o.Description,
                                             Status = o.Status,
                                             ClosureDate = o.ClosureDate,
                                             ClosureComments = o.ClosureComments,
                                             RaisedByClosureComments = o.RaisedByClosureComments,
                                             Id = o.Id
                                         },
                                         ExceptionTypeName = s1 == null ? "" : s1.Name.ToString(),
                                         UserName = s2 == null ? "" : s2.FullName,
                                         DeptCode = s4 == null ? "" : s4.Code.ToString(),
                                         OrganizationUnitDisplayName = s4 == null ? "" : s4.DisplayName.ToString()
                                     };
            var exceptions = await exceptionIncidents.ToListAsync();
            var totalCount = 0;

            if (codes.Count > 0)
            {
                exceptions = exceptions.Where(x => codes.Any(e => e == x.DeptCode)).ToList();
                totalCount = exceptions.Count();
            }

            var output = exceptions.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return new PagedResultDto<GetExceptionIncidentForViewDto>(
                totalCount,output
                 
            );
        }

        public async Task<GetExceptionIncidentForViewDto> GetExceptionIncidentForView(int id)
        {
            var exceptionIncident = await _exceptionIncidentRepository.GetAsync(id);

            var attachments = new List<ExceptionIncidentAttachment>();
            if (!string.IsNullOrEmpty(exceptionIncident.ExceptionIncidentAttachments) && exceptionIncident.ExceptionIncidentAttachments != "[]")
            {
                attachments = JsonSerializer.Deserialize<List<ExceptionIncidentAttachment>>(exceptionIncident.ExceptionIncidentAttachments);
            }

            var output = new GetExceptionIncidentForViewDto { ExceptionIncident = ObjectMapper.Map<ExceptionIncidentDto>(exceptionIncident) };

            output.ExceptionIncidentAttachment = attachments;

            if (output.ExceptionIncident.ExceptionTypeId != null)
            {
                var _lookupExceptionType = await _lookup_exceptionTypeRepository.FirstOrDefaultAsync((int)output.ExceptionIncident.ExceptionTypeId);
                output.ExceptionTypeName = _lookupExceptionType.Name.ToString();
            }

            if (output.ExceptionIncident.RaisedById != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ExceptionIncident.RaisedById);
                output.UserName = _lookupUser.Name.ToString();
            }

            if (output.ExceptionIncident.WorkingPaperId != null)
            {
                var _lookupTestingTemplate = await _lookup_workingPaperTemplateRepository.FirstOrDefaultAsync((Guid)output.ExceptionIncident.WorkingPaperId);
               // output.WorkingPaperCode = _lookupTestingTemplate.Code.ToString();
            }

            if (output.ExceptionIncident.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.ExceptionIncident.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents_Edit)]
        public async Task<GetExceptionIncidentForEditOutput> GetExceptionIncidentForEdit(EntityDto input)
        {
            var exceptionIncident = await _exceptionIncidentRepository.FirstOrDefaultAsync(input.Id);

            var attachments = new List<ExceptionIncidentAttachment>();
            if (!string.IsNullOrEmpty(exceptionIncident.ExceptionIncidentAttachments) && exceptionIncident.ExceptionIncidentAttachments!="[]")
            {
                attachments = JsonSerializer.Deserialize<List<ExceptionIncidentAttachment>>(exceptionIncident.ExceptionIncidentAttachments);
            }
            
            // await ExceptionRoleTest(exceptionIncident);

            var output = new GetExceptionIncidentForEditOutput { ExceptionIncident = ObjectMapper.Map<CreateOrEditExceptionIncidentDto>(exceptionIncident) };

            if (output.ExceptionIncident.ExceptionTypeId != null)
            {
                var _lookupExceptionType = await _lookup_exceptionTypeRepository.FirstOrDefaultAsync((int)output.ExceptionIncident.ExceptionTypeId);
                output.ExceptionTypeName = _lookupExceptionType.Name.ToString();
            }

            if (exceptionIncident.RaisedById != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)exceptionIncident.RaisedById);
                output.UserName = _lookupUser.FullName.ToString();
            }

            if (output.ExceptionIncident.WorkingPaperId != null)
            {
                var _lookupTestingTemplate = await _lookup_workingPaperTemplateRepository.FirstOrDefaultAsync((Guid)output.ExceptionIncident.WorkingPaperId);
                //output.WorkingPaperCode = _lookupTestingTemplate.Code.ToString();
            }

            if (output.ExceptionIncident.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.ExceptionIncident.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

            var listIncidentColumns = await _exceptionIncidentColumnRepository.GetAll().Include(x => x.ExceptionTypeColumnFk)
                .Where(x => x.ExceptionIncidentId == input.Id).ToListAsync();

            List<CreateOrEditExceptionIncidentColumnDto> outputIncidents = new List<CreateOrEditExceptionIncidentColumnDto>();

            foreach(var item in listIncidentColumns)
            {
                outputIncidents.Add(new CreateOrEditExceptionIncidentColumnDto()
                {
                    Id = item.Id,
                    Name = item.ExceptionTypeColumnFk.Name,
                    Value = item.Value
                });
            }

            output.ExceptionIncident.IncidentColumns = outputIncidents.ToArray();
            output.ExceptionIncidentAttachment = attachments;

            return output;
        }

        private async Task ExceptionRoleTest(ExceptionIncident exceptionIncident)
        {
            if (exceptionIncident == null)
            {
                var userRole = await _unitOrganizationRoleRepository.FirstOrDefaultAsync(x =>  x.UserId == AbpSession.UserId && (x.DepartmentRole == DepartmentRole.ControlHead || x.DepartmentRole == DepartmentRole.ControlTeamMember));

                if (userRole == null)
                    throw new UserFriendlyException("You do not have the right to raise an exception!");
            }

            if (exceptionIncident.Status == Status.Open)
            {
                if (exceptionIncident.CausedById != AbpSession.UserId)
                {
                    var departments = await UserManager.GetOrganizationUnitsAsync(await UserManager.GetUserByIdAsync((long)AbpSession.UserId));


                    if (departments.Count(x => x.Id == exceptionIncident.OrganizationUnitId) == 0)
                        throw new UserFriendlyException("You are not authorized to treat this exception");
                }
            }

            if (exceptionIncident.Status == Status.Resolved)
            {
                if (exceptionIncident.RaisedById != AbpSession.UserId)
                {
                    var departments = await UserManager.GetOrganizationUnitsAsync(await UserManager.GetUserByIdAsync((long)exceptionIncident.RaisedById));

                    var userRole = await _unitOrganizationRoleRepository.FirstOrDefaultAsync(x => x.OrganizationUnitId == exceptionIncident.OrganizationUnitId && x.UserId == AbpSession.UserId && (x.DepartmentRole == DepartmentRole.ControlHead || x.DepartmentRole == DepartmentRole.ControlTeamMember));

                    if (userRole == null)
                        throw new UserFriendlyException("You are not authorized to close this exception");
                }
            }
        }

        public async Task CreateOrEdit(CreateOrEditExceptionIncidentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents_Create)]
        protected virtual async Task Create(CreateOrEditExceptionIncidentDto input)
        {
            var exceptionIncident = ObjectMapper.Map<ExceptionIncident>(input);

            var previousCount = await _exceptionIncidentRepository.CountAsync();
            previousCount++;

            exceptionIncident.Code = "EXCP-" + previousCount.ToString();

            if (AbpSession.TenantId != null)
            {
                exceptionIncident.TenantId = (int)AbpSession.TenantId;
            }
            exceptionIncident.RaisedById = AbpSession.UserId;
            exceptionIncident.Date = DateTime.Now;
            exceptionIncident.Status = Status.Open;

            exceptionIncident.ExceptionIncidentAttachments = JsonSerializer.Serialize(input.ExceptionIncidentAttachment);

            var id = await _exceptionIncidentRepository.InsertAndGetIdAsync(exceptionIncident);

            if (input.IncidentColumns != null)
            {
                foreach (var item in input.IncidentColumns)
                {
                    await _exceptionIncidentColumnRepository.InsertAsync(new ExceptionIncidentColumn()
                    {
                        TenantId = (int)AbpSession.TenantId,
                        ExceptionIncidentId = id,
                        ExceptionTypeColumnId = item.ExceptionTypeColumnId,
                        Value = item.Value
                    });
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents_Edit)]
        protected virtual async Task Update(CreateOrEditExceptionIncidentDto input)
        {
            var exceptionIncident = await _exceptionIncidentRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, exceptionIncident);
        }

        [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents_Edit)]
        public virtual async Task Resolve(CreateOrEditExceptionIncidentDto input)
        {
 
            var exceptionIncident = await _exceptionIncidentRepository.FirstOrDefaultAsync((int)input.Id);

            if (exceptionIncident.Status != Status.Resolved)
            {
                throw new UserFriendlyException("This exception is not open for resolution.");
            }

            exceptionIncident.ResolutionDate = DateTime.Now;
            exceptionIncident.ResolutionComments = input.ResolutionComments;
            exceptionIncident.Status = Status.Resolved;
 
        }


        [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents_Edit)]
        public virtual async Task Close(CreateOrEditExceptionIncidentDto input)
        {

            var exceptionIncident = await _exceptionIncidentRepository.FirstOrDefaultAsync((int)input.Id);

            if (exceptionIncident.Status != Status.Resolved)
            {
                throw new UserFriendlyException("This exception is not open for resolution.");
            }

            exceptionIncident.ResolutionDate = DateTime.Now;
            exceptionIncident.Status = Status.Closed;
            exceptionIncident.ClosureComments = input.ClosureComments;
            exceptionIncident.ClosureDate = input.ClosureDate;
        }


        [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents_Edit)]
        public virtual async Task Reject(CreateOrEditExceptionIncidentDto input)
        {

            var exceptionIncident = await _exceptionIncidentRepository.FirstOrDefaultAsync((int)input.Id);

            if (exceptionIncident.Status != Status.Open)
            {
                throw new UserFriendlyException("This exception is not open for resolution.");
            }

            exceptionIncident.ResolutionDate = null;
            exceptionIncident.ResolutionComments = input.ResolutionComments;
            exceptionIncident.ClosureComments = input.ClosureComments;
            exceptionIncident.Status = Status.Open;

        }


        public async Task<List<GetExceptionTypeColumnsForEdit>> GetExceptionColumnsForIncident(int id)
        {
            var exceptionTypeColumns = from o in _exceptionTypeColumnColumnRepository.GetAll()
                                  where o.ExceptionTypeId == id
                                  select new GetExceptionTypeColumnsForEdit()
                                  {
                                      Name = o.Name,
                                      Id = o.Id,
                                      DataFieldType = o.DataType.ToString(),                         
                                      Maximum = o.Maximum,
                                      Minimum = o.Minimum,
                                      Required = o.Required
                                  };
            var result = await exceptionTypeColumns.ToListAsync();

            return result;
        }


        [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _exceptionIncidentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetExceptionIncidentsToExcel(GetAllExceptionIncidentsForExcelInput input)
        {
            var statusFilter = (Status)input.StatusFilter;

            var filteredExceptionIncidents = _exceptionIncidentRepository.GetAll()
                        .Include(e => e.ExceptionTypeFk)
                        .Include(e => e.RaisedByFk)

                        .Include(e => e.OrganizationUnitFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ClosureComments.Contains(input.Filter) || e.RaisedByClosureComments.Contains(input.Filter))
                        .WhereIf(input.MinDateFilter != null, e => e.Date >= input.MinDateFilter)
                        .WhereIf(input.MaxDateFilter != null, e => e.Date <= input.MaxDateFilter)
                        .WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(input.MinClosureDateFilter != null, e => e.ClosureDate >= input.MinClosureDateFilter)
                        .WhereIf(input.MaxClosureDateFilter != null, e => e.ClosureDate <= input.MaxClosureDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExceptionTypeNameFilter), e => e.ExceptionTypeFk != null && e.ExceptionTypeFk.Name == input.ExceptionTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.RaisedByFk != null && e.RaisedByFk.Name == input.UserNameFilter)

                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

            var query = (from o in filteredExceptionIncidents
                         join o1 in _lookup_exceptionTypeRepository.GetAll() on o.ExceptionTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_userRepository.GetAll() on o.RaisedById equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()



                         join o4 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetExceptionIncidentForViewDto()
                         {
                             ExceptionIncident = new ExceptionIncidentDto
                             {
                                 Code = o.Code,
                                 Date = o.Date,
                                 Description = o.Description,
                                 Status = o.Status,
                                 ClosureDate = o.ClosureDate,
                                 ClosureComments = o.ClosureComments,
                                 RaisedByClosureComments = o.RaisedByClosureComments,
                                 Id = o.Id
                             },
                             ExceptionTypeName = s1 == null ? "" : s1.Name.ToString(),
                             UserName = s2 == null ? "" : s2.Name.ToString()
                         });


            var exceptionIncidentListDtos = await query.ToListAsync();

            return _exceptionIncidentsExcelExporter.ExportToFile(exceptionIncidentListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents)]
        public async Task<PagedResultDto<ExceptionIncidentExceptionTypeLookupTableDto>> GetAllExceptionTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_exceptionTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var exceptionTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ExceptionIncidentExceptionTypeLookupTableDto>();
            foreach (var exceptionType in exceptionTypeList)
            {
                lookupTableDtoList.Add(new ExceptionIncidentExceptionTypeLookupTableDto
                {
                    Id = exceptionType.Id,
                    DisplayName = exceptionType.Name?.ToString()
                });
            }

            return new PagedResultDto<ExceptionIncidentExceptionTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents)]
        public async Task<PagedResultDto<ExceptionIncidentUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository
                .GetAll()
                .WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ExceptionIncidentUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new ExceptionIncidentUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.FullName
                });
            }

            return new PagedResultDto<ExceptionIncidentUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents)]
        public async Task<PagedResultDto<ExceptionIncidentTestingTemplateLookupTableDto>> GetAllTestingTemplateForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_workingPaperTemplateRepository.GetAll()
              //  .WhereIf( !string.IsNullOrWhiteSpace(input.Filter), e => e.Code.ToString().Contains(input.Filter))
                ;

            var totalCount = await query.CountAsync();

            var testingTemplateList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ExceptionIncidentTestingTemplateLookupTableDto>();
            foreach (var testingTemplate in testingTemplateList)
            {
                lookupTableDtoList.Add(new ExceptionIncidentTestingTemplateLookupTableDto
                {
                    Id = testingTemplate.Id.ToString(),
                    DisplayName = testingTemplate.Code?.ToString()
                });
            }

            return new PagedResultDto<ExceptionIncidentTestingTemplateLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents)]
        public async Task<PagedResultDto<ExceptionIncidentOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_organizationUnitRepository.GetAll()
                .Where(x => !x.IsAbstract && !x.IsControlTeam)
                .WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ExceptionIncidentOrganizationUnitLookupTableDto>();
            foreach (var organizationUnit in organizationUnitList)
            {
                lookupTableDtoList.Add(new ExceptionIncidentOrganizationUnitLookupTableDto
                {
                    Id = organizationUnit.Id,
                    DisplayName = organizationUnit.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<ExceptionIncidentOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}