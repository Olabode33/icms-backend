
using ICMSDemo;
using ICMSDemo;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.Controls.Exporting;
using ICMSDemo.Controls.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ICMSDemo.ExceptionTypes.Dtos;
using ICMSDemo.ExceptionTypes;
using ICMSDemo.WorkingPaperNews;
using ICMSDemo.WorkingPaperNews.Dtos;

namespace ICMSDemo.Controls
{
	[AbpAuthorize(AppPermissions.Pages_Controls)]
    public class ControlsAppService : ICMSDemoAppServiceBase, IControlsAppService
    {
		 private readonly IRepository<Control> _controlRepository;
		 private readonly IControlsExcelExporter _controlsExcelExporter;
		private readonly IExceptionTypesAppService _exceptionTypesAppService;
		
		public ControlsAppService(IRepository<Control> controlRepository,
			IControlsExcelExporter controlsExcelExporter,
			IExceptionTypesAppService exceptionTypesAppService
			) 
		  {
			_controlRepository = controlRepository;
			_controlsExcelExporter = controlsExcelExporter;
			_exceptionTypesAppService = exceptionTypesAppService;
			

		}

		 public async Task<PagedResultDto<GetControlForViewDto>> GetAll(GetAllControlsInput input)
         {
			var controlTypeFilter = (ControlType) input.ControlTypeFilter;
			var frequencyFilter = (Frequency) input.FrequencyFilter;
			
			var filteredControls = _controlRepository.GetAll().Include(e => e.ControlOwnerFK)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.ControlTypeFilter > -1, e => e.ControlType == controlTypeFilter)
						.WhereIf(input.FrequencyFilter > -1, e => e.Frequency == frequencyFilter);

			var pagedAndFilteredControls = filteredControls
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var controls = from o in pagedAndFilteredControls
                         select new GetControlForViewDto() {
							Control = new ControlDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Description = o.Description,
                                ControlType = o.ControlType,
                                Frequency = o.Frequency,
                                Id = o.Id,
								ControlObjective = o.ControlObjective,
								ControlOwnerId = o.ControlOwnerId,
								ControlPoint = o.ControlPoint
							},
							ControlOwnerName = o.ControlOwnerFK != null ? o.ControlOwnerFK.FullName : ""
						};

            var totalCount = await filteredControls.CountAsync();

            return new PagedResultDto<GetControlForViewDto>(
                totalCount,
                await controls.ToListAsync()
            );
         }
		 
		 public async Task<GetControlForViewDto> GetControlForView(int id)
         {
            var control = await _controlRepository.GetAsync(id);

            var output = new GetControlForViewDto { Control = ObjectMapper.Map<ControlDto>(control) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Controls_Edit)]
		 public async Task<GetControlForEditOutput> GetControlForEdit(EntityDto input)
         {
            var control = await _controlRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetControlForEditOutput {Control = ObjectMapper.Map<CreateOrEditControlDto>(control)};

			if (control.ControlOwnerId != null)
            {
				var user = await UserManager.GetUserByIdAsync((long)control.ControlOwnerId);
				output.ControlOwnerName = user.FullName;
            }
			
            return output;
         }

		 public async Task<NameValueDto<int>> CreateOrEdit(CreateOrEditControlDto input)
         {
            if(input.Id == null){
				return await Create(input);
			}
			else{
				return await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Controls_Create)]
		 protected virtual async Task<NameValueDto<int>> Create(CreateOrEditControlDto input)
         {
            var control = ObjectMapper.Map<Control>(input);
			var prevCount = await _controlRepository.CountAsync();
			prevCount++;

			control.Code = "C-" + prevCount.ToString();


			if (AbpSession.TenantId != null)
			{
				control.TenantId = (int) AbpSession.TenantId;
			}
		
            var id  = await _controlRepository.InsertAndGetIdAsync(control);
            try
            {
				CreateOrEditExceptionTypeDto exception = new CreateOrEditExceptionTypeDto
				{
					Name = input.Name,
					Description = input.Description,
					Severity = Severity.Medium,
					TargetRemediation = 10,
					Remediation = ExceptionRemediationTypeEnum.Remediable
				};

				await _exceptionTypesAppService.CreateOrEdit(exception);

			}
            catch (Exception)
            {

               
            }


			return new NameValueDto<int>
			{
				Name = control.Name,
				Value = id
			};
         }

		 [AbpAuthorize(AppPermissions.Pages_Controls_Edit)]
		 protected virtual async Task<NameValueDto<int>> Update(CreateOrEditControlDto input)
         {
            var control = await _controlRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, control);
			return new NameValueDto<int>
			{
				Name = input.Name,
				Value = control.Id
			};
         }

		 [AbpAuthorize(AppPermissions.Pages_Controls_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _controlRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetControlsToExcel(GetAllControlsForExcelInput input)
         {
			var controlTypeFilter = (ControlType) input.ControlTypeFilter;
			var frequencyFilter = (Frequency) input.FrequencyFilter;
			
			var filteredControls = _controlRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.ControlTypeFilter > -1, e => e.ControlType == controlTypeFilter)
						.WhereIf(input.FrequencyFilter > -1, e => e.Frequency == frequencyFilter);

			var query = (from o in filteredControls
                         select new GetControlForViewDto() { 
							Control = new ControlDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Description = o.Description,
                                ControlType = o.ControlType,
                                Frequency = o.Frequency,
                                Id = o.Id
							}
						 });


            var controlListDtos = await query.ToListAsync();

            return _controlsExcelExporter.ExportToFile(controlListDtos);
         }


    }
}