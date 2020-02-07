using ICMSDemo.DepartmentRiskControls;

using ICMSDemo;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.TestingTemplates.Exporting;
using ICMSDemo.TestingTemplates.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ICMSDemo.TestingTemplates
{
	[AbpAuthorize(AppPermissions.Pages_TestingTemplates)]
    public class TestingTemplatesAppService : ICMSDemoAppServiceBase, ITestingTemplatesAppService
    {
		 private readonly IRepository<TestingTemplate> _testingTemplateRepository;
		 private readonly ITestingTemplatesExcelExporter _testingTemplatesExcelExporter;
		 private readonly IRepository<DepartmentRiskControl,int> _lookup_departmentRiskControlRepository;
		 

		  public TestingTemplatesAppService(IRepository<TestingTemplate> testingTemplateRepository, ITestingTemplatesExcelExporter testingTemplatesExcelExporter , IRepository<DepartmentRiskControl, int> lookup_departmentRiskControlRepository) 
		  {
			_testingTemplateRepository = testingTemplateRepository;
			_testingTemplatesExcelExporter = testingTemplatesExcelExporter;
			_lookup_departmentRiskControlRepository = lookup_departmentRiskControlRepository;
		
		  }

		 public async Task<PagedResultDto<GetTestingTemplateForViewDto>> GetAll(GetAllTestingTemplatesInput input)
         {
			var frequencyFilter = (Frequency) input.FrequencyFilter;
			
			var filteredTestingTemplates = _testingTemplateRepository.GetAll()
						.Include( e => e.DepartmentRiskControlFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.DetailedInstructions.Contains(input.Filter) || e.Title.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title == input.TitleFilter)
						.WhereIf(input.FrequencyFilter > -1, e => e.Frequency == frequencyFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DepartmentRiskControlCodeFilter), e => e.DepartmentRiskControlFk != null && e.DepartmentRiskControlFk.Code == input.DepartmentRiskControlCodeFilter);

			var pagedAndFilteredTestingTemplates = filteredTestingTemplates
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var testingTemplates = from o in pagedAndFilteredTestingTemplates
                         join o1 in _lookup_departmentRiskControlRepository.GetAll() on o.DepartmentRiskControlId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetTestingTemplateForViewDto() {
							TestingTemplate = new TestingTemplateDto
							{
                                Code = o.Code,
                                DetailedInstructions = o.DetailedInstructions,
                                Title = o.Title,
                                Frequency = o.Frequency,
                                Id = o.Id
							},
                         	DepartmentRiskControlCode = s1 == null ? "" : s1.Code.ToString()
						};

            var totalCount = await filteredTestingTemplates.CountAsync();

            return new PagedResultDto<GetTestingTemplateForViewDto>(
                totalCount,
                await testingTemplates.ToListAsync()
            );
         }
		 
		 public async Task<GetTestingTemplateForViewDto> GetTestingTemplateForView(int id)
         {
            var testingTemplate = await _testingTemplateRepository.GetAsync(id);

            var output = new GetTestingTemplateForViewDto { TestingTemplate = ObjectMapper.Map<TestingTemplateDto>(testingTemplate) };

		    if (output.TestingTemplate.DepartmentRiskControlId != null)
            {
                var _lookupDepartmentRiskControl = await _lookup_departmentRiskControlRepository.FirstOrDefaultAsync((int)output.TestingTemplate.DepartmentRiskControlId);
                output.DepartmentRiskControlCode = _lookupDepartmentRiskControl.Code.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_TestingTemplates_Edit)]
		 public async Task<GetTestingTemplateForEditOutput> GetTestingTemplateForEdit(EntityDto input)
         {
            var testingTemplate = await _testingTemplateRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTestingTemplateForEditOutput {TestingTemplate = ObjectMapper.Map<CreateOrEditTestingTemplateDto>(testingTemplate)};

		    if (output.TestingTemplate.DepartmentRiskControlId != null)
            {
                var _lookupDepartmentRiskControl = await _lookup_departmentRiskControlRepository.FirstOrDefaultAsync((int)output.TestingTemplate.DepartmentRiskControlId);
                output.DepartmentRiskControlCode = _lookupDepartmentRiskControl.Code.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTestingTemplateDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_TestingTemplates_Create)]
		 protected virtual async Task Create(CreateOrEditTestingTemplateDto input)
         {
            var testingTemplate = ObjectMapper.Map<TestingTemplate>(input);

			
			if (AbpSession.TenantId != null)
			{
				testingTemplate.TenantId = (int) AbpSession.TenantId;
			}
		

            await _testingTemplateRepository.InsertAsync(testingTemplate);
         }

		 [AbpAuthorize(AppPermissions.Pages_TestingTemplates_Edit)]
		 protected virtual async Task Update(CreateOrEditTestingTemplateDto input)
         {
            var testingTemplate = await _testingTemplateRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, testingTemplate);
         }

		 [AbpAuthorize(AppPermissions.Pages_TestingTemplates_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _testingTemplateRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTestingTemplatesToExcel(GetAllTestingTemplatesForExcelInput input)
         {
			var frequencyFilter = (Frequency) input.FrequencyFilter;
			
			var filteredTestingTemplates = _testingTemplateRepository.GetAll()
						.Include( e => e.DepartmentRiskControlFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.DetailedInstructions.Contains(input.Filter) || e.Title.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title == input.TitleFilter)
						.WhereIf(input.FrequencyFilter > -1, e => e.Frequency == frequencyFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DepartmentRiskControlCodeFilter), e => e.DepartmentRiskControlFk != null && e.DepartmentRiskControlFk.Code == input.DepartmentRiskControlCodeFilter);

			var query = (from o in filteredTestingTemplates
                         join o1 in _lookup_departmentRiskControlRepository.GetAll() on o.DepartmentRiskControlId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetTestingTemplateForViewDto() { 
							TestingTemplate = new TestingTemplateDto
							{
                                Code = o.Code,
                                DetailedInstructions = o.DetailedInstructions,
                                Title = o.Title,
                                Frequency = o.Frequency,
                                Id = o.Id
							},
                         	DepartmentRiskControlCode = s1 == null ? "" : s1.Code.ToString()
						 });


            var testingTemplateListDtos = await query.ToListAsync();

            return _testingTemplatesExcelExporter.ExportToFile(testingTemplateListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_TestingTemplates)]
         public async Task<PagedResultDto<TestingTemplateDepartmentRiskControlLookupTableDto>> GetAllDepartmentRiskControlForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_departmentRiskControlRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Code.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var departmentRiskControlList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TestingTemplateDepartmentRiskControlLookupTableDto>();
			foreach(var departmentRiskControl in departmentRiskControlList){
				lookupTableDtoList.Add(new TestingTemplateDepartmentRiskControlLookupTableDto
				{
					Id = departmentRiskControl.Id,
					DisplayName = departmentRiskControl.Code?.ToString()
				});
			}

            return new PagedResultDto<TestingTemplateDepartmentRiskControlLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}