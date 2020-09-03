using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.BusinessObjectives.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.BusinessObjectives
{
    public interface IBusinessObjectivesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBusinessObjectiveForViewDto>> GetAll(GetAllBusinessObjectivesInput input);

        Task<GetBusinessObjectiveForViewDto> GetBusinessObjectiveForView(int id);

		Task<GetBusinessObjectiveForEditOutput> GetBusinessObjectiveForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditBusinessObjectiveDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBusinessObjectivesToExcel(GetAllBusinessObjectivesForExcelInput input);

		
		Task<PagedResultDto<BusinessObjectiveUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}