using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.ControlTestingAssessment.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.ControlTestingAssessment
{
    public interface IControlTestingsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetControlTestingForViewDto>> GetAll(GetAllControlTestingsInput input);

        Task<GetControlTestingForViewDto> GetControlTestingForView(int id);

		Task<GetControlTestingForEditOutput> GetControlTestingForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditControlTestingDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetControlTestingsToExcel(GetAllControlTestingsForExcelInput input);

		
    }
}