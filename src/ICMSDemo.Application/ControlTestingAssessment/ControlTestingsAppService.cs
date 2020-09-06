

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.ControlTestingAssessment.Exporting;
using ICMSDemo.ControlTestingAssessment.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ICMSDemo.Projects;
using static ICMSDemo.IcmsEnums;
using ICMSDemo.WorkingPaperNews.Dtos;
using ICMSDemo.WorkingPaperNews;
using ICMSDemo.ProcessRiskControls;
using ICMSDemo.Processes;
using ICMSDemo.ProcessRisks;
using ICMSDemo.Processes.Dtos;
using Abp.Organizations;

namespace ICMSDemo.ControlTestingAssessment
{
	[AbpAuthorize(AppPermissions.Pages_ControlTestings)]
    public class ControlTestingsAppService : ICMSDemoAppServiceBase, IControlTestingsAppService
    {
		 private readonly IRepository<ControlTesting> _controlTestingRepository;
		 private readonly IControlTestingsExcelExporter _controlTestingsExcelExporter;
		private readonly IRepository<Project> _projectRepository;
		private readonly IRepository<Process, long> _processRepository;
		private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository;
		private readonly IRepository<ProcessRisk> _lookup_processRiskRepository;
		private readonly IRepository<ProcessRiskControl> _lookup_processRiskControlRepository;
		private readonly IWorkingPaperNewsAppService _workingPaperNewsAppService;

		public ControlTestingsAppService(IRepository<ControlTesting> controlTestingRepository,
			IControlTestingsExcelExporter controlTestingsExcelExporter, 
			IRepository<Project> projectRepository,
			IRepository<OrganizationUnit, long> lookup_organizationUnitRepository,
			IWorkingPaperNewsAppService workingPaperNewsAppService,
			 IRepository<Process, long> processRepository,
			  IRepository<ProcessRisk> lookup_processRiskRepository,
			IRepository<ProcessRiskControl> lookup_processRiskControlRepository) 
		  {
			_controlTestingRepository = controlTestingRepository;
			_projectRepository = projectRepository;
			_processRepository = processRepository;
			_lookup_organizationUnitRepository = lookup_organizationUnitRepository;
			_controlTestingsExcelExporter = controlTestingsExcelExporter;
			_workingPaperNewsAppService = workingPaperNewsAppService;
			_lookup_processRiskRepository = lookup_processRiskRepository;
			_lookup_processRiskControlRepository = lookup_processRiskControlRepository;
		}

		 public async Task<PagedResultDto<GetControlTestingForViewDto>> GetAll(GetAllControlTestingsInput input)
         {
			var filteredControlTestings = _controlTestingRepository.GetAll().Include(e => e.AssignedUserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
						.WhereIf(input.MinTestingTemplateIdFilter != null, e => e.TestingTemplateId >= input.MinTestingTemplateIdFilter)
						.WhereIf(input.MaxTestingTemplateIdFilter != null, e => e.TestingTemplateId <= input.MaxTestingTemplateIdFilter);
						//.WhereIf(!string.IsNullOrWhiteSpace(input.EndDateFilter),  e => e.EndDate == input.EndDateFilter);

			var pagedAndFilteredControlTestings = filteredControlTestings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var controlTestings = from o in pagedAndFilteredControlTestings
                         select new GetControlTestingForViewDto() {
							ControlTesting = new ControlTestingDto
							{
                                Name = o.Name,
                                TestingTemplateId = o.TestingTemplateId,
                                EndDate = o.EndDate,
                                Id = o.Id,
								AssignedUserId = o.AssignedUserId
							},
							Name = o.AssignedUserFk == null ? o.Name : o.AssignedUserFk.FullName,
							EndDate = o.EndDate,
							Id = o.Id,
						 };

            var totalCount = await filteredControlTestings.CountAsync();

			var output =await controlTestings.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();

			return new PagedResultDto<GetControlTestingForViewDto>(
                totalCount,
                 output
			);
         }
		 
		 public async Task<GetControlTestingForViewDto> GetControlTestingForView(int id)
         {
            var controlTesting = await _controlTestingRepository.GetAsync(id);

            var output = new GetControlTestingForViewDto { ControlTesting = ObjectMapper.Map<ControlTestingDto>(controlTesting) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ControlTestings_Edit)]
		 public async Task<GetControlTestingForEditOutput> GetControlTestingForEdit(EntityDto input)
         {
            var controlTesting = await _controlTestingRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetControlTestingForEditOutput {ControlTesting = ObjectMapper.Map<CreateOrEditControlTestingDto>(controlTesting)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditControlTestingDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ControlTestings_Create)]
		 protected virtual async Task Create(CreateOrEditControlTestingDto input)
         {

            var controlTesting = ObjectMapper.Map<ControlTesting>(input);
		    var res  = await  _projectRepository.FirstOrDefaultAsync(e => e.ProjectOwner == ProjectOwner.OperationRisk && e.Closed != true);
			controlTesting.ProjectId = res == null ? (int?)null : res.Id;
			if (AbpSession.TenantId != null)
			{
				controlTesting.TenantId = (int?) AbpSession.TenantId;
			}
		

			var id = await _controlTestingRepository.InsertAndGetIdAsync(controlTesting);

            try
            {
				CreateOrEditWorkingPaperNewDto workingPaper = new CreateOrEditWorkingPaperNewDto
				{
					AssignedToId = controlTesting.AssignedUserId,
					Comment = controlTesting.Name,
					TestingTemplateId = controlTesting.TestingTemplateId,
					OrganizationUnitId = controlTesting.OrganizationUnitId,
					ProjectId = controlTesting.ProjectId,
					Id = null
				};
				await _workingPaperNewsAppService.CreateOrEdit(workingPaper);

			}
            catch (Exception)
            {

               
            }
         


		}

		[AbpAuthorize(AppPermissions.Pages_ControlTestings_Edit)]
		 protected virtual async Task Update(CreateOrEditControlTestingDto input)
         {
            var controlTesting = await _controlTestingRepository.FirstOrDefaultAsync((int)input.Id);
			var res = await _projectRepository.FirstOrDefaultAsync(e => e.ProjectOwner == ProjectOwner.OperationRisk && e.Closed != true);
			controlTesting.ProjectId = res == null ? (int?)null : res.Id;

			ObjectMapper.Map(input, controlTesting);
         }

		 [AbpAuthorize(AppPermissions.Pages_ControlTestings_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _controlTestingRepository.DeleteAsync(input.Id);
         } 


		public async Task<FileDto> GetControlTestingsToExcel()
         {
			//var list = new ExportToExcelDto();
			var filteredControlTestings = _lookup_organizationUnitRepository.GetAll();

            var query = (from o in filteredControlTestings
						 select new GetProcessForViewDto()
						 {
							 Process = new ProcessDto
							 {
								 Name = o.DisplayName,
								 Description = o.DisplayName,
								 
							 },
						 });


			var controlTestingListDtos = await query.ToListAsync();

            return _controlTestingsExcelExporter.ExportToFile(controlTestingListDtos);
         }


    }
}