using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.TestingTemplates.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.TestingTemplates
{
    public interface ITestingTemplatesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTestingTemplateForViewDto>> GetAll(GetAllTestingTemplatesInput input);

        Task<GetTestingTemplateForViewDto> GetTestingTemplateForView(int id);

		Task<GetTestingTemplateForEditOutput> GetTestingTemplateForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTestingTemplateDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTestingTemplatesToExcel(GetAllTestingTemplatesForExcelInput input);

		
		Task<PagedResultDto<TestingTemplateDepartmentRiskControlLookupTableDto>> GetAllDepartmentRiskControlForLookupTable(GetAllForLookupTableInput input);
		
    }
}