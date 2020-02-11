using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.ExceptionIncidents.Dtos;
using ICMSDemo.Dto;
using System.Collections.Generic;

namespace ICMSDemo.ExceptionIncidents
{
    public interface IExceptionIncidentsAppService : IApplicationService 
    {
		Task Reject(CreateOrEditExceptionIncidentDto input);

		Task Close(CreateOrEditExceptionIncidentDto input);

		Task Resolve(CreateOrEditExceptionIncidentDto input);

		Task<PagedResultDto<GetExceptionIncidentForViewDto>> GetAll(GetAllExceptionIncidentsInput input);

        Task<GetExceptionIncidentForViewDto> GetExceptionIncidentForView(int id);

		Task<GetExceptionIncidentForEditOutput> GetExceptionIncidentForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditExceptionIncidentDto input);
		Task<List<GetExceptionTypeColumnsForEdit>> GetExceptionColumnsForIncident(int id);


		Task Delete(EntityDto input);

		Task<FileDto> GetExceptionIncidentsToExcel(GetAllExceptionIncidentsForExcelInput input);

		
		Task<PagedResultDto<ExceptionIncidentExceptionTypeLookupTableDto>> GetAllExceptionTypeForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ExceptionIncidentUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ExceptionIncidentTestingTemplateLookupTableDto>> GetAllTestingTemplateForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ExceptionIncidentOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input);
		
    }
}