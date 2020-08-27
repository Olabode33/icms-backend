using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.LossEvents.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.LossEvents
{
    public interface ILossEventsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLossEventForViewDto>> GetAll(GetAllLossEventsInput input);

        Task<GetLossEventForViewDto> GetLossEventForView(int id);

		Task<GetLossEventForEditOutput> GetLossEventForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditLossEventDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLossEventsToExcel(GetAllLossEventsForExcelInput input);

		
		Task<PagedResultDto<LossEventUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<LossEventOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input);
		
    }
}