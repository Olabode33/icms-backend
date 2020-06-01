using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.LibraryRisks.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.LibraryRisks
{
    public interface ILibraryRisksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLibraryRiskForViewDto>> GetAll(GetAllLibraryRisksInput input);

        Task<GetLibraryRiskForViewDto> GetLibraryRiskForView(int id);

		Task<GetLibraryRiskForEditOutput> GetLibraryRiskForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditLibraryRiskDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLibraryRisksToExcel(GetAllLibraryRisksForExcelInput input);

		
    }
}