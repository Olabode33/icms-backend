using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.LibraryControls.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.LibraryControls
{
    public interface ILibraryControlsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLibraryControlForViewDto>> GetAll(GetAllLibraryControlsInput input);

        Task<GetLibraryControlForViewDto> GetLibraryControlForView(int id);

		Task<GetLibraryControlForEditOutput> GetLibraryControlForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditLibraryControlDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLibraryControlsToExcel(GetAllLibraryControlsForExcelInput input);

		
    }
}