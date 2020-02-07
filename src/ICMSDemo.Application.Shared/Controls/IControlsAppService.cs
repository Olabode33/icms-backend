using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.Controls.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.Controls
{
    public interface IControlsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetControlForViewDto>> GetAll(GetAllControlsInput input);

        Task<GetControlForViewDto> GetControlForView(int id);

		Task<GetControlForEditOutput> GetControlForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditControlDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetControlsToExcel(GetAllControlsForExcelInput input);

		
    }
}