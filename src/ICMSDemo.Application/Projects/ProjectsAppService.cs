using Abp.Organizations;
using Abp.Organizations;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.Projects.Exporting;
using ICMSDemo.Projects.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ICMSDemo.Processes;
using ICMSDemo.Departments;
using Abp.Timing;
using ICMSDemo.Authorization.Users;
using Abp.UI;
using Stripe;
using ICMSDemo.Projects.Events;
using ICMSDemo.WorkingPapers;
using ICMSDemo.ExceptionIncidents;
using ICMSDemo.Ratings;
using ICMSDemo.DepartmentRatingHistory;
using static ICMSDemo.IcmsEnums;

namespace ICMSDemo.Projects
{
    [AbpAuthorize]
    public class ProjectsAppService : ICMSDemoAppServiceBase, IProjectsAppService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IProjectsExcelExporter _projectsExcelExporter;
        private readonly IRepository<Process, long> _lookup_processRepository;
        private readonly IRepository<Department, long> _lookup_departmentRepository;
        private readonly IRepository<OrganizationUnit, long> _lookup_OURepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<WorkingPaper, Guid> _lookup_workingPaperRepository;
        private readonly IRepository<ExceptionIncident> _lookup_exceptionsRepository;
        private readonly IRepository<Rating> _lookup_ratingRepository;
        private readonly IRepository<DepartmentRating> _lookup_deptRatingRepository;
        private readonly IRepository<RcsaProgramAssessment> _rcsaAssessmentRepository;


        public ProjectsAppService(
            IRepository<Project> projectRepository, IRepository<OrganizationUnit, long> lookup_OURepository,
             IRepository<DepartmentRating> lookup_deptRatingRepository,
             IRepository<Rating> lookup_ratingRepository,
             IProjectsExcelExporter projectsExcelExporter,
            IRepository<Department, long> lookup_departmentRepository,
            IRepository<Process, long> lookup_processRepository,
            IRepository<WorkingPaper, Guid> lookup_workingPaperRepository,
            IRepository<ExceptionIncident> lookup_exceptionsRepository,
            IRepository<User, long> lookup_userRepository,
            IRepository<RcsaProgramAssessment> rcsaAssessmentRepository)
        {
            _projectRepository = projectRepository;
            _projectsExcelExporter = projectsExcelExporter;
            _lookup_departmentRepository = lookup_departmentRepository;
            _lookup_processRepository = lookup_processRepository;
            _lookup_OURepository = lookup_OURepository;
            _lookup_workingPaperRepository = lookup_workingPaperRepository;
            _lookup_exceptionsRepository = lookup_exceptionsRepository;
            _lookup_ratingRepository = lookup_ratingRepository;
            _lookup_deptRatingRepository = lookup_deptRatingRepository;
            _lookup_userRepository = lookup_userRepository;
            _rcsaAssessmentRepository = rcsaAssessmentRepository;
        }



        public async Task<PagedResultDto<GetProjectForViewDto>> GetAll(GetAllProjectsInput input)
        {


            var filteredProjects = _projectRepository.GetAll()
                        .Include(e => e.ControlUnitFk)
                        .Include(e => e.ScopeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Title.Contains(input.Filter))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.MinBudgetedStartDateFilter != null, e => e.BudgetedStartDate >= input.MinBudgetedStartDateFilter)
                        .WhereIf(input.MaxBudgetedStartDateFilter != null, e => e.BudgetedStartDate <= input.MaxBudgetedStartDateFilter)
                        .WhereIf(input.MinBudgetedEndDateFilter != null, e => e.BudgetedEndDate >= input.MinBudgetedEndDateFilter)
                        .WhereIf(input.MaxBudgetedEndDateFilter != null, e => e.BudgetedEndDate <= input.MaxBudgetedEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.ControlUnitFk != null && e.ControlUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayName2Filter), e => e.ScopeFk != null && e.ScopeFk.DisplayName == input.OrganizationUnitDisplayName2Filter)
                        .WhereIf(input.CommencedFilter, e => e.Commenced == input.CommencedFilter)
                        .WhereIf(input.ProjectOwner != null, e => e.ProjectOwner == input.ProjectOwner || e.ProjectOwner == ProjectOwner.General);
            //.WhereIf(input.ProjectOwner == ProjectOwner.General, e => e.ProjectOwner == null);


            var pagedAndFilteredProjects = filteredProjects
                .OrderBy(input.Sorting ?? "budgetedEndDate desc")
                .PageBy(input);

            var projects = from o in pagedAndFilteredProjects
                           join o1 in _lookup_departmentRepository.GetAll() on o.ControlUnitId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _lookup_OURepository.GetAll() on o.ScopeId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           select new GetProjectForViewDto()
                           {
                               Project = new ProjectDto
                               {
                                   Code = o.Code,
                                   Description = o.Description,
                                   StartDate = o.StartDate,
                                   EndDate = o.EndDate,
                                   BudgetedStartDate = o.BudgetedStartDate,
                                   BudgetedEndDate = o.BudgetedEndDate,
                                   Title = o.Title,
                                   ProjectOwner = o.ProjectOwner,
                                   Progress = o.Progress,
                                   ReviewType = o.ReviewType,
                                   Commenced = o.Commenced,
                                   Id = o.Id
                               },
                               OrganizationUnitDisplayName = s1 == null ? "" : s1.DisplayName.ToString(),
                               OrganizationUnitDisplayName2 = o.Cascade ? string.Format("{0} and its sub-sets", s2.DisplayName.ToString()) : s2.DisplayName.ToString()

                           };

            var totalCount = await filteredProjects.CountAsync();

            return new PagedResultDto<GetProjectForViewDto>(
                totalCount,
                await projects.ToListAsync()
            );
        }

        public async Task<GetProjectForViewDto> GetProjectForView(int id)
        {
            var project = await _projectRepository.GetAsync(id);

            var output = new GetProjectForViewDto { Project = ObjectMapper.Map<ProjectDto>(project) };

            if (output.Project.ControlUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_departmentRepository.FirstOrDefaultAsync((long)output.Project.ControlUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

            if (output.Project.ScopeId != null)
            {

                if (output.Project.ReviewType == ReviewType.Department)
                {
                    var _lookupOrganizationUnit = await _lookup_departmentRepository.FirstOrDefaultAsync((long)output.Project.ScopeId);
                    output.OrganizationUnitDisplayName2 = output.Project.Cascade ? string.Format("{0} and its sub-units.", _lookupOrganizationUnit.DisplayName) : _lookupOrganizationUnit.DisplayName;
                }
                else
                {
                    var _lookupOrganizationUnit = await _lookup_processRepository.FirstOrDefaultAsync((long)output.Project.ScopeId);
                    output.OrganizationUnitDisplayName2 = output.Project.Cascade ? string.Format("{0} and its sub-processes.", _lookupOrganizationUnit.DisplayName) : _lookupOrganizationUnit.DisplayName;
                }
            }

            return output;
        }

        // [AbpAuthorize(AppPermissions.Pages_Projects_Edit)]
        public async Task<GetProjectForEditOutput> GetProjectForEdit(EntityDto input)
        {
            var project = await _projectRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProjectForEditOutput { Project = ObjectMapper.Map<CreateOrEditProjectDto>(project) };

            if (output.Project.ControlUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_departmentRepository.FirstOrDefaultAsync((long)output.Project.ControlUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

            if (output.Project.ScopeId != null)
            {

                if (output.Project.ReviewType == ReviewType.Department)
                {
                    var _lookupOrganizationUnit = await _lookup_departmentRepository.FirstOrDefaultAsync((long)output.Project.ScopeId);
                    output.OrganizationUnitDisplayName2 = output.Project.Cascade ? string.Format("{0} and its sub-units.", _lookupOrganizationUnit.DisplayName) : _lookupOrganizationUnit.DisplayName;
                }
                else
                {
                    var _lookupOrganizationUnit = await _lookup_processRepository.FirstOrDefaultAsync((long)output.Project.ScopeId);
                    output.OrganizationUnitDisplayName2 = output.Project.Cascade ? string.Format("{0} and its sub-processes.", _lookupOrganizationUnit.DisplayName) : _lookupOrganizationUnit.DisplayName;
                }
            }

            output.OpenWorkingPapers = await _lookup_workingPaperRepository.GetAll().Where(x => x.ProjectId == input.Id).CountAsync(x => (x.TaskStatus == TaskStatus.Open) || (x.TaskStatus == TaskStatus.Draft));
            output.PendingReviews = await _lookup_workingPaperRepository.GetAll().Where(x => x.ProjectId == input.Id).CountAsync(x => x.TaskStatus == TaskStatus.PendingReview);

            var workingPaperCount = await _lookup_workingPaperRepository.GetAll().Where(x => x.ProjectId == input.Id).CountAsync();

            output.OpenTaskPercent = workingPaperCount == 0 ? 0 : (double)output.OpenWorkingPapers / (double)workingPaperCount;
            output.PendingReviewsPercent = workingPaperCount == 0 ? 0 : (double)output.PendingReviews / (double)workingPaperCount;
            output.CompletedTaskCount = workingPaperCount - (output.OpenWorkingPapers + output.PendingReviews);
            output.CompletionLevel = (double)output.CompletedTaskCount == (double)0 ? 0 : 1 - (output.OpenTaskPercent + output.PendingReviewsPercent);

            output.ExceptionsCount = await _lookup_exceptionsRepository.GetAll()
                                                                       .Include(x => x.WorkingPaperFk)
                                                                       .Where(x => x.WorkingPaperFk != null && x.WorkingPaperFk.ProjectId == input.Id)
                                                                       .CountAsync();

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProjectDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Projects_Create)]
        protected virtual async Task Create(CreateOrEditProjectDto input)
        {
            if (input.BudgetedStartDate <= Clock.Now)
            {
                throw new UserFriendlyException("The budgeted start date can not be in the past!");
            }

            if (input.BudgetedEndDate <= Clock.Now)
            {
                throw new UserFriendlyException("The budgeted end date can not be in the past!");
            }

            if (input.BudgetedEndDate.Date <= input.BudgetedStartDate.Date)
            {
                throw new UserFriendlyException("The budgeted end date has to be after the budgeted start date!");
            }


            if (input.ScopeEndDate.Value <= input.ScopeStartDate.Value)
            {
                throw new UserFriendlyException("The review Period end date has to be after the Period start date!");
            }


            if (input.ScopeId == null)
            {
                throw new UserFriendlyException("You must select a scope of review!");
            }


            var project = ObjectMapper.Map<Project>(input);
            project.StartDate = null;
            project.EndDate = null;

            project.Code = DateTime.Now.ToString("yyMMdd") + "-" + Guid.NewGuid().ToString().ToUpper().Substring(0, 6);


            if (AbpSession.TenantId != null)
            {
                project.TenantId = (int)AbpSession.TenantId;
            }

            await _projectRepository.InsertAsync(project);
        }

        [AbpAuthorize(AppPermissions.Pages_Projects_Edit)]
        public virtual async Task Activate(EntityDto input)
        {
            var project = await _projectRepository.FirstOrDefaultAsync(input.Id);

            if (project == null)
            {
                throw new UserFriendlyException("The project with the Id does not exist.");
            }

            if (project.Commenced)
            {
                throw new UserFriendlyException("The project has already been activated for commencement.");
            }

            project.Commenced = true;
            project.StartDate = Clock.Now.Date;

            await EventBus.TriggerAsync(new ProjectActivatedEventData() { EventSource = project, EventTime = Clock.Now, TenantId = project.TenantId, Project = project, ProjectOwner = project.ProjectOwner });
        }

        [AbpAuthorize(AppPermissions.Pages_Projects_Edit)]
        protected virtual async Task Update(CreateOrEditProjectDto input)
        {
            var project = await _projectRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, project);
        }

        [AbpAuthorize(AppPermissions.Pages_Projects_Edit)]
        public virtual async Task CloseProject(EntityDto input)
        {
            var project = await _projectRepository.FirstOrDefaultAsync((int)input.Id);

            if (project.Closed)
            {
                throw new UserFriendlyException("This project has been closed already.");
            }

            project.Closed = true;
            project.CloseDate = Clock.Now;


            //// TODO:Abstract this into a background and hangfire job

            var workingPapers = await _lookup_workingPaperRepository.GetAllListAsync(x => x.ProjectId == input.Id);

            if (workingPapers.Count(x => x.TaskStatus != TaskStatus.Approved) > 0)
            {
                throw new UserFriendlyException("There are un-approved working papers in this project.");
            }

            var groupedWpByOU = workingPapers.GroupBy(x => x.OrganizationUnitId);

            var departmentList = groupedWpByOU.Select(x => x.Key).ToList();

            var allDepartments = await _lookup_departmentRepository.GetAllListAsync();

            var relevantDepartments = allDepartments.Where(x => departmentList.Any(y => y == x.Id)).ToList();

            foreach (var dept in groupedWpByOU)
            {
                var averageScore = dept.Average(x => x.Score);

                var department = relevantDepartments.FirstOrDefault(x => x.Id == dept.Key);

                var rating = await _lookup_ratingRepository.GetAll().Where(x => x.UpperBoundary > averageScore).OrderBy(x => x.UpperBoundary).FirstOrDefaultAsync();

                department.RatingId = rating.Id;

                await _lookup_deptRatingRepository.InsertAsync(new DepartmentRating { OrganizationUnitId = department.Id, RatingDate = Clock.Now, TenantId = department.TenantId.Value, ChangeType = "Automated", Comment = "This rating was auto-generated after the completion of a project", RatingId = rating.Id });
            }

        }

        [AbpAuthorize(AppPermissions.Pages_Projects_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _projectRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProjectsToExcel(GetAllProjectsForExcelInput input)
        {

            var filteredProjects = _projectRepository.GetAll()
                        .Include(e => e.ControlUnitFk)
                        .Include(e => e.ScopeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Title.Contains(input.Filter))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.MinBudgetedStartDateFilter != null, e => e.BudgetedStartDate >= input.MinBudgetedStartDateFilter)
                        .WhereIf(input.MaxBudgetedStartDateFilter != null, e => e.BudgetedStartDate <= input.MaxBudgetedStartDateFilter)
                        .WhereIf(input.MinBudgetedEndDateFilter != null, e => e.BudgetedEndDate >= input.MinBudgetedEndDateFilter)
                        .WhereIf(input.MaxBudgetedEndDateFilter != null, e => e.BudgetedEndDate <= input.MaxBudgetedEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.ControlUnitFk != null && e.ControlUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayName2Filter), e => e.ScopeFk != null && e.ScopeFk.DisplayName == input.OrganizationUnitDisplayName2Filter);

            var query = (from o in filteredProjects
                         join o1 in _lookup_departmentRepository.GetAll() on o.ControlUnitId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_OURepository.GetAll() on o.ScopeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProjectForViewDto()
                         {
                             Project = new ProjectDto
                             {
                                 Code = o.Code,
                                 Description = o.Description,
                                 StartDate = o.StartDate,
                                 EndDate = o.EndDate,
                                 BudgetedStartDate = o.BudgetedStartDate,
                                 BudgetedEndDate = o.BudgetedEndDate,
                                 Title = o.Title,
                                 Id = o.Id
                             },
                             OrganizationUnitDisplayName = s1 == null ? "" : s1.DisplayName.ToString(),
                             OrganizationUnitDisplayName2 = o.Cascade ? string.Format("{0} and its sub-sets", s2.DisplayName.ToString()) : s2.DisplayName.ToString()
                         });


            var projectListDtos = await query.ToListAsync();

            return _projectsExcelExporter.ExportToFile(projectListDtos);
        }

        //[AbpAuthorize(AppPermissions.Pages_Projects)]
        public async Task<PagedResultDto<ProjectOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_departmentRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName != null && e.DisplayName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProjectOrganizationUnitLookupTableDto>();
            foreach (var organizationUnit in organizationUnitList)
            {
                lookupTableDtoList.Add(new ProjectOrganizationUnitLookupTableDto
                {
                    Id = organizationUnit.Id,
                    DisplayName = organizationUnit.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<ProjectOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        //  [AbpAuthorize(AppPermissions.Pages_Projects)]
        public async Task<PagedResultDto<ProjectOrganizationUnitLookupTableDto>> GetAllProcesses(GetAllForLookupTableInput input)
        {
            var query = _lookup_processRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName != null && e.DisplayName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProjectOrganizationUnitLookupTableDto>();
            foreach (var organizationUnit in organizationUnitList)
            {
                lookupTableDtoList.Add(new ProjectOrganizationUnitLookupTableDto
                {
                    Id = organizationUnit.Id,
                    DisplayName = organizationUnit.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<ProjectOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}