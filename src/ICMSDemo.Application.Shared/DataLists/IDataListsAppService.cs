using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.DataLists.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.DataLists
{
    public interface IDataListsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDataListForViewDto>> GetAll(GetAllDataListsInput input);

        Task<GetDataListForViewDto> GetDataListForView(int id);

		Task<GetDataListForEditOutput> GetDataListForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDataListDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDataListsToExcel(GetAllDataListsForExcelInput input);

		
    }
}