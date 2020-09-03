using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.KeyRiskIndicators.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.KeyRiskIndicators
{
    public interface IKeyRiskIndicatorsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetKeyRiskIndicatorForViewDto>> GetAll(GetAllKeyRiskIndicatorsInput input);

        Task<GetKeyRiskIndicatorForViewDto> GetKeyRiskIndicatorForView(int id);

		Task<GetKeyRiskIndicatorForEditOutput> GetKeyRiskIndicatorForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditKeyRiskIndicatorDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetKeyRiskIndicatorsToExcel(GetAllKeyRiskIndicatorsForExcelInput input);

		
		Task<PagedResultDto<KeyRiskIndicatorExceptionTypeLookupTableDto>> GetAllExceptionTypeForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<KeyRiskIndicatorUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}