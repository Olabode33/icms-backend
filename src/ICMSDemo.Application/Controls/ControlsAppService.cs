
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

namespace ICMSDemo.Controls
{
	[AbpAuthorize(AppPermissions.Pages_Controls)]
    public class ControlsAppService : ICMSDemoAppServiceBase, IControlsAppService
    {
		 private readonly IRepository<Control> _controlRepository;
		 private readonly IControlsExcelExporter _controlsExcelExporter;
		 

		  public ControlsAppService(IRepository<Control> controlRepository, IControlsExcelExporter controlsExcelExporter ) 
		  {
			_controlRepository = controlRepository;
			_controlsExcelExporter = controlsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetControlForViewDto>> GetAll(GetAllControlsInput input)
         {
			var controlTypeFilter = (ControlType) input.ControlTypeFilter;
			var frequencyFilter = (Frequency) input.FrequencyFilter;
			
			var filteredControls = _controlRepository.GetAll()
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
                                Id = o.Id
							}
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
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditControlDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Controls_Create)]
		 protected virtual async Task Create(CreateOrEditControlDto input)
         {
            var control = ObjectMapper.Map<Control>(input);
			var prevCount = await _controlRepository.CountAsync();
			prevCount++;

			control.Code = "C-" + prevCount.ToString();


			if (AbpSession.TenantId != null)
			{
				control.TenantId = (int) AbpSession.TenantId;
			}
		

            await _controlRepository.InsertAsync(control);
         }

		 [AbpAuthorize(AppPermissions.Pages_Controls_Edit)]
		 protected virtual async Task Update(CreateOrEditControlDto input)
         {
            var control = await _controlRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, control);
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