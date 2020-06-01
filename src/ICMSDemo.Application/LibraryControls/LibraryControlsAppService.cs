

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.LibraryControls.Exporting;
using ICMSDemo.LibraryControls.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ICMSDemo.LibraryControls
{
	[AbpAuthorize(AppPermissions.Pages_LibraryControls)]
    public class LibraryControlsAppService : ICMSDemoAppServiceBase, ILibraryControlsAppService
    {
		 private readonly IRepository<LibraryControl> _libraryControlRepository;
		 private readonly ILibraryControlsExcelExporter _libraryControlsExcelExporter;
		 

		  public LibraryControlsAppService(IRepository<LibraryControl> libraryControlRepository, ILibraryControlsExcelExporter libraryControlsExcelExporter ) 
		  {
			_libraryControlRepository = libraryControlRepository;
			_libraryControlsExcelExporter = libraryControlsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetLibraryControlForViewDto>> GetAll(GetAllLibraryControlsInput input)
         {
			
			var filteredLibraryControls = _libraryControlRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Process.Contains(input.Filter) || e.SubProcess.Contains(input.Filter) || e.Risk.Contains(input.Filter) || e.ControlType.Contains(input.Filter) || e.ControlPoint.Contains(input.Filter) || e.Frequency.Contains(input.Filter) || e.InformationProcessingObjectives.Contains(input.Filter));

			var pagedAndFilteredLibraryControls = filteredLibraryControls
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var libraryControls = from o in pagedAndFilteredLibraryControls
                         select new GetLibraryControlForViewDto() {
							LibraryControl = new LibraryControlDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                Process = o.Process,
                                SubProcess = o.SubProcess,
                                Risk = o.Risk,
                                ControlType = o.ControlType,
                                ControlPoint = o.ControlPoint,
                                Frequency = o.Frequency,
                                InformationProcessingObjectives = o.InformationProcessingObjectives,
                                Id = o.Id
							}
						};

            var totalCount = await filteredLibraryControls.CountAsync();

            return new PagedResultDto<GetLibraryControlForViewDto>(
                totalCount,
                await libraryControls.ToListAsync()
            );
         }
		 
		 public async Task<GetLibraryControlForViewDto> GetLibraryControlForView(int id)
         {
            var libraryControl = await _libraryControlRepository.GetAsync(id);

            var output = new GetLibraryControlForViewDto { LibraryControl = ObjectMapper.Map<LibraryControlDto>(libraryControl) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_LibraryControls_Edit)]
		 public async Task<GetLibraryControlForEditOutput> GetLibraryControlForEdit(EntityDto input)
         {
            var libraryControl = await _libraryControlRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetLibraryControlForEditOutput {LibraryControl = ObjectMapper.Map<CreateOrEditLibraryControlDto>(libraryControl)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditLibraryControlDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_LibraryControls_Create)]
		 protected virtual async Task Create(CreateOrEditLibraryControlDto input)
         {
            var libraryControl = ObjectMapper.Map<LibraryControl>(input);

			
			if (AbpSession.TenantId != null)
			{
				libraryControl.TenantId = (int) AbpSession.TenantId;
			}
		

            await _libraryControlRepository.InsertAsync(libraryControl);
         }

		 [AbpAuthorize(AppPermissions.Pages_LibraryControls_Edit)]
		 protected virtual async Task Update(CreateOrEditLibraryControlDto input)
         {
            var libraryControl = await _libraryControlRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, libraryControl);
         }

		 [AbpAuthorize(AppPermissions.Pages_LibraryControls_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _libraryControlRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetLibraryControlsToExcel(GetAllLibraryControlsForExcelInput input)
         {
			
			var filteredLibraryControls = _libraryControlRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Process.Contains(input.Filter) || e.SubProcess.Contains(input.Filter) || e.Risk.Contains(input.Filter) || e.ControlType.Contains(input.Filter) || e.ControlPoint.Contains(input.Filter) || e.Frequency.Contains(input.Filter) || e.InformationProcessingObjectives.Contains(input.Filter));

			var query = (from o in filteredLibraryControls
                         select new GetLibraryControlForViewDto() { 
							LibraryControl = new LibraryControlDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                Process = o.Process,
                                SubProcess = o.SubProcess,
                                Risk = o.Risk,
                                ControlType = o.ControlType,
                                ControlPoint = o.ControlPoint,
                                Frequency = o.Frequency,
                                InformationProcessingObjectives = o.InformationProcessingObjectives,
                                Id = o.Id
							}
						 });


            var libraryControlListDtos = await query.ToListAsync();

            return _libraryControlsExcelExporter.ExportToFile(libraryControlListDtos);
         }


    }
}