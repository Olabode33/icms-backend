

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.LibraryRisks.Exporting;
using ICMSDemo.LibraryRisks.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ICMSDemo.LibraryRisks
{
	[AbpAuthorize(AppPermissions.Pages_LibraryRisks)]
    public class LibraryRisksAppService : ICMSDemoAppServiceBase, ILibraryRisksAppService
    {
		 private readonly IRepository<LibraryRisk> _libraryRiskRepository;
		 private readonly ILibraryRisksExcelExporter _libraryRisksExcelExporter;
		 

		  public LibraryRisksAppService(IRepository<LibraryRisk> libraryRiskRepository, ILibraryRisksExcelExporter libraryRisksExcelExporter ) 
		  {
			_libraryRiskRepository = libraryRiskRepository;
			_libraryRisksExcelExporter = libraryRisksExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetLibraryRiskForViewDto>> GetAll(GetAllLibraryRisksInput input)
         {
			
			var filteredLibraryRisks = _libraryRiskRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Process.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.SubProcess.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProcessFilter),  e => e.Process == input.ProcessFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SubProcessFilter),  e => e.SubProcess == input.SubProcessFilter);

			var pagedAndFilteredLibraryRisks = filteredLibraryRisks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var libraryRisks = from o in pagedAndFilteredLibraryRisks
                         select new GetLibraryRiskForViewDto() {
							LibraryRisk = new LibraryRiskDto
							{
                                Name = o.Name,
                                Process = o.Process,
                                Description = o.Description,
                                SubProcess = o.SubProcess,
                                Id = o.Id
							}
						};

            var totalCount = await filteredLibraryRisks.CountAsync();

            return new PagedResultDto<GetLibraryRiskForViewDto>(
                totalCount,
                await libraryRisks.ToListAsync()
            );
         }
		 
		 public async Task<GetLibraryRiskForViewDto> GetLibraryRiskForView(int id)
         {
            var libraryRisk = await _libraryRiskRepository.GetAsync(id);

            var output = new GetLibraryRiskForViewDto { LibraryRisk = ObjectMapper.Map<LibraryRiskDto>(libraryRisk) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_LibraryRisks_Edit)]
		 public async Task<GetLibraryRiskForEditOutput> GetLibraryRiskForEdit(EntityDto input)
         {
            var libraryRisk = await _libraryRiskRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetLibraryRiskForEditOutput {LibraryRisk = ObjectMapper.Map<CreateOrEditLibraryRiskDto>(libraryRisk)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditLibraryRiskDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_LibraryRisks_Create)]
		 protected virtual async Task Create(CreateOrEditLibraryRiskDto input)
         {
            var libraryRisk = ObjectMapper.Map<LibraryRisk>(input);

			
			if (AbpSession.TenantId != null)
			{
				libraryRisk.TenantId = (int) AbpSession.TenantId;
			}
		

            await _libraryRiskRepository.InsertAsync(libraryRisk);
         }

		 [AbpAuthorize(AppPermissions.Pages_LibraryRisks_Edit)]
		 protected virtual async Task Update(CreateOrEditLibraryRiskDto input)
         {
            var libraryRisk = await _libraryRiskRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, libraryRisk);
         }

		 [AbpAuthorize(AppPermissions.Pages_LibraryRisks_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _libraryRiskRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetLibraryRisksToExcel(GetAllLibraryRisksForExcelInput input)
         {
			
			var filteredLibraryRisks = _libraryRiskRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Process.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.SubProcess.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProcessFilter),  e => e.Process == input.ProcessFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SubProcessFilter),  e => e.SubProcess == input.SubProcessFilter);

			var query = (from o in filteredLibraryRisks
                         select new GetLibraryRiskForViewDto() { 
							LibraryRisk = new LibraryRiskDto
							{
                                Name = o.Name,
                                Process = o.Process,
                                Description = o.Description,
                                SubProcess = o.SubProcess,
                                Id = o.Id
							}
						 });


            var libraryRiskListDtos = await query.ToListAsync();

            return _libraryRisksExcelExporter.ExportToFile(libraryRiskListDtos);
         }


    }
}