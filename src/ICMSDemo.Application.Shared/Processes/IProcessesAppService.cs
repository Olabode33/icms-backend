using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.Processes.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.Processes
{
    public interface IProcessesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProcessForViewDto>> GetAll(GetAllProcessesInput input);

		Task<GetProcessForEditOutput> GetProcessForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditProcessDto input);

		Task Delete(EntityDto<long> input);

		
		Task<PagedResultDto<ProcessUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ProcessOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input);
		
    }
}