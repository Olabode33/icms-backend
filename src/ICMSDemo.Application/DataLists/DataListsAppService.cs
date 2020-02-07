

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.DataLists.Exporting;
using ICMSDemo.DataLists.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ICMSDemo.DataLists
{
	[AbpAuthorize(AppPermissions.Pages_DataLists)]
    public class DataListsAppService : ICMSDemoAppServiceBase, IDataListsAppService
    {
		 private readonly IRepository<DataList> _dataListRepository;
		 private readonly IDataListsExcelExporter _dataListsExcelExporter;
		 

		  public DataListsAppService(IRepository<DataList> dataListRepository, IDataListsExcelExporter dataListsExcelExporter ) 
		  {
			_dataListRepository = dataListRepository;
			_dataListsExcelExporter = dataListsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetDataListForViewDto>> GetAll(GetAllDataListsInput input)
         {
			
			var filteredDataLists = _dataListRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var pagedAndFilteredDataLists = filteredDataLists
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var dataLists = from o in pagedAndFilteredDataLists
                         select new GetDataListForViewDto() {
							DataList = new DataListDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                Id = o.Id
							}
						};

            var totalCount = await filteredDataLists.CountAsync();

            return new PagedResultDto<GetDataListForViewDto>(
                totalCount,
                await dataLists.ToListAsync()
            );
         }
		 
		 public async Task<GetDataListForViewDto> GetDataListForView(int id)
         {
            var dataList = await _dataListRepository.GetAsync(id);

            var output = new GetDataListForViewDto { DataList = ObjectMapper.Map<DataListDto>(dataList) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_DataLists_Edit)]
		 public async Task<GetDataListForEditOutput> GetDataListForEdit(EntityDto input)
         {
            var dataList = await _dataListRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetDataListForEditOutput {DataList = ObjectMapper.Map<CreateOrEditDataListDto>(dataList)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditDataListDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_DataLists_Create)]
		 protected virtual async Task Create(CreateOrEditDataListDto input)
         {
            var dataList = ObjectMapper.Map<DataList>(input);

			
			if (AbpSession.TenantId != null)
			{
				dataList.TenantId = (int) AbpSession.TenantId;
			}
		

            await _dataListRepository.InsertAsync(dataList);
         }

		 [AbpAuthorize(AppPermissions.Pages_DataLists_Edit)]
		 protected virtual async Task Update(CreateOrEditDataListDto input)
         {
            var dataList = await _dataListRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, dataList);
         }

		 [AbpAuthorize(AppPermissions.Pages_DataLists_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _dataListRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetDataListsToExcel(GetAllDataListsForExcelInput input)
         {
			
			var filteredDataLists = _dataListRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var query = (from o in filteredDataLists
                         select new GetDataListForViewDto() { 
							DataList = new DataListDto
							{
                                Name = o.Name,
                                Description = o.Description,
                                Id = o.Id
							}
						 });


            var dataListListDtos = await query.ToListAsync();

            return _dataListsExcelExporter.ExportToFile(dataListListDtos);
         }


    }
}