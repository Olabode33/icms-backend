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
using ICMSDemo.Risks.Dtos;
using ICMSDemo.Controls.Dtos;
using ICMSDemo.ExceptionTypes;
using ICMSDemo.WorkingPaperNews.Dtos;
using ICMSDemo.ProcessRiskControls;

namespace ICMSDemo.TestingTemplates
{
    [AbpAuthorize(AppPermissions.Pages_TestingTemplates)]
    public class TestingTemplatesAppService : ICMSDemoAppServiceBase, ITestingTemplatesAppService
    {
        private readonly IRepository<TestingTemplate> _testingTemplateRepository;
        private readonly IRepository<ExceptionType> _exceptionTypesRepository;
        private readonly IRepository<TestingAttrribute> _testingTemplateAttributesRepository;
        private readonly ITestingTemplatesExcelExporter _testingTemplatesExcelExporter;

        private readonly IRepository<ProcessRiskControl, int> _lookup_processRiskControlRepository;


        public TestingTemplatesAppService(IRepository<TestingTemplate> testingTemplateRepository,
            IRepository<ExceptionType> exceptionTypesRepository,
            IRepository<TestingAttrribute> testingTemplateAttributesRepository,
            ITestingTemplatesExcelExporter testingTemplatesExcelExporter, 

            IRepository<ProcessRiskControl, int> lookup_processRiskControlRepository)
        {
            _testingTemplateRepository = testingTemplateRepository;
            _testingTemplatesExcelExporter = testingTemplatesExcelExporter;
       
            _lookup_processRiskControlRepository = lookup_processRiskControlRepository;
            _testingTemplateAttributesRepository = testingTemplateAttributesRepository;
            _exceptionTypesRepository = exceptionTypesRepository;
        }

        public async Task<PagedResultDto<GetTestingTemplateForViewDto>> GetAll(GetAllTestingTemplatesInput input)
        {
            var frequencyFilter = (Frequency)input.FrequencyFilter;

            var filteredTestingTemplates = _testingTemplateRepository.GetAll()
                        .Include(e => e.DepartmentRiskControlFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.DetailedInstructions.Contains(input.Filter) || e.Title.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(input.FrequencyFilter > -1, e => e.Frequency == frequencyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DepartmentRiskControlCodeFilter), e => e.DepartmentRiskControlFk != null && e.DepartmentRiskControlFk.Code == input.DepartmentRiskControlCodeFilter);

            var pagedAndFilteredTestingTemplates = filteredTestingTemplates
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var testingTemplates = from o in pagedAndFilteredTestingTemplates
                                   join o1 in _lookup_processRiskControlRepository
                                   .GetAll().Include(x => x.ProcessRiskFk).Include(x => x.ProcessRiskFk.ProcessFk) on o.ProcessRiskControlId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   select new GetTestingTemplateForViewDto()
                                   {
                                       TestingTemplate = new TestingTemplateDto
                                       {
                                           Code = o.Code,
                                           DetailedInstructions = o.DetailedInstructions,
                                           Title = o.Title,
                                           Frequency = o.Frequency.ToString(),
                                           Id = o.Id,
                                           IsActive = o.IsActive
                                       },
                                       DepartmentRiskControlCode = s1 == null ? "" : s1.Code,
                                       AffectedDepartments = s1 == null ? "" : s1.DepartmentFk.Name,
                                       Cascade = s1 == null ? "" : s1.Cascade.ToString()

                                   };

            var totalCount = await filteredTestingTemplates.CountAsync();

            return new PagedResultDto<GetTestingTemplateForViewDto>(
                totalCount,
                await testingTemplates.ToListAsync()
            );
        }

        public async Task<ListResultDto<GetTestingTemplateForViewDto>> GetForProcessControl(EntityDto input)
        {
            var filteredTestingTemplates = _testingTemplateRepository.GetAll()
                        .Include(e => e.DepartmentRiskControlFk)
                        .Where(x => x.ProcessRiskControlId == input.Id);

            var testingTemplates = from o in filteredTestingTemplates
                                   select new GetTestingTemplateForViewDto()
                                   {
                                       TestingTemplate = new TestingTemplateDto
                                       {
                                           Code = o.Code,
                                           DetailedInstructions = o.DetailedInstructions,
                                           Title = o.Title,
                                           Frequency = o.Frequency.ToString(),
                                           Id = o.Id,
                                           IsActive = o.IsActive
                                       }
                                   };

            var totalCount = await filteredTestingTemplates.CountAsync();

            return new ListResultDto<GetTestingTemplateForViewDto>(await testingTemplates.ToListAsync());
        }


        public async Task<GetTestingTemplateForViewDto> GetTestingTemplateForView(int id)
        {
            var testingTemplate = await _testingTemplateRepository.GetAsync(id);

            var output = new GetTestingTemplateForViewDto { TestingTemplate = ObjectMapper.Map<TestingTemplateDto>(testingTemplate) };

            if (output.TestingTemplate.DepartmentRiskControlId != null)
            {
                var _lookupDepartmentRiskControl = await _lookup_processRiskControlRepository
                    .GetAll()
                    .Include(x => x.ControlFk)
                    .Include(x => x.DepartmentRiskFk).ThenInclude(x => x.RiskFk)
                    .Include(x => x.DepartmentRiskFk).ThenInclude(x => x.DepartmentFk)
                    .FirstOrDefaultAsync(x => x.Id == (int)output.TestingTemplate.DepartmentRiskControlId);

                if (typeof(ProcessRiskControl) == _lookupDepartmentRiskControl.GetType())
                {
                    var _lookupProcessRiskControl = await _lookup_processRiskControlRepository
                                                           .GetAll()
                                                           .Include(x => x.ControlFk)
                                                           .Include(x => x.ProcessRiskFk).ThenInclude(x => x.RiskFk)
                                                           .Include(x => x.ProcessRiskFk).ThenInclude(x => x.ProcessFk)
                                                           .FirstOrDefaultAsync(x => x.Id == (int)output.TestingTemplate.DepartmentRiskControlId);

                    output.Risk = ObjectMapper.Map<RiskDto>(_lookupProcessRiskControl.ProcessRiskFk.RiskFk);
                    output.Control = ObjectMapper.Map<ControlDto>(_lookupProcessRiskControl.ControlFk);
                    output.EntityType = "Process";
                    output.OuDisplayName = _lookupProcessRiskControl.ProcessRiskFk.ProcessFk.Name;
                } else
                {
                    output.Risk = ObjectMapper.Map<RiskDto>(_lookupDepartmentRiskControl.DepartmentRiskFk.RiskFk);
                    output.Control = ObjectMapper.Map<ControlDto>(_lookupDepartmentRiskControl.ControlFk);
                    output.EntityType = "Business Unit";
                    output.OuDisplayName = _lookupDepartmentRiskControl.DepartmentRiskFk.DepartmentFk.DisplayName;
                }

            }


            var attributesList = await _testingTemplateAttributesRepository.GetAllListAsync(x => x.TestingTemplateId == id);

            List<CreateOrEditTestingAttributeDto> outputAttributes = new List<CreateOrEditTestingAttributeDto>();

            foreach (var item in attributesList)
            {
                outputAttributes.Add(new CreateOrEditTestingAttributeDto()
                {
                    AttributeText = item.TestAttribute,
                    TestingAttrributeId = item.Id,
                    Weight = item.Weight,
                    Result = false
                });
            }

            output.Attributes = outputAttributes.ToArray();

            if (testingTemplate.ExceptionTypeId != null)
            {
                var exceptionType = await _exceptionTypesRepository.FirstOrDefaultAsync((int)testingTemplate.ExceptionTypeId);
                output.ExceptionTypeName = exceptionType.Name;
            }

            return output;
        }

        //public async Task<List<CreateOrEditTestingAttributeDto>> GetTestAttributesForTemplate(int testTemplateId)
        //{
        //    var attributesToTest = await _lookup_testingAttributeRepository.GetAllListAsync(x => x.TestingTemplateId == testTemplateId);

        //    List<CreateOrEditTestingAttributeDto> output = new List<CreateOrEditTestingAttributeDto>();

        //    foreach (var item in attributesToTest)
        //    {
        //        output.Add(new CreateOrEditTestingAttributeDto()
        //        {
        //            AttributeText = item.TestAttribute,
        //            TestingAttrributeId = item.Id,
        //            Result = false
        //        });
        //    }

        //    return output;
        //}

        [AbpAuthorize(AppPermissions.Pages_TestingTemplates_Edit)]
        public async Task<GetTestingTemplateForEditOutput> GetTestingTemplateForEdit(EntityDto input)
        {
            var testingTemplate = await _testingTemplateRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTestingTemplateForEditOutput { TestingTemplate = ObjectMapper.Map<CreateOrEditTestingTemplateDto>(testingTemplate) };

            if (output.TestingTemplate.DepartmentRiskControlId != null)
            {
                var _lookupDepartmentRiskControl = await _lookup_processRiskControlRepository.FirstOrDefaultAsync((int)output.TestingTemplate.DepartmentRiskControlId);
                output.DepartmentRiskControlCode = _lookupDepartmentRiskControl.Code.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTestingTemplateDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TestingTemplates_Create)]
        protected virtual async Task Create(CreateOrEditTestingTemplateDto input)
        {
            var previousCount = await _testingTemplateRepository.CountAsync();

            previousCount++;

            var testingTemplate = ObjectMapper.Map<TestingTemplate>(input);
            testingTemplate.Code = "TT-" + previousCount.ToString();
            testingTemplate.ProcessRiskControlId = input.DepartmentRiskControlId;

            if (AbpSession.TenantId != null)
            {
                testingTemplate.TenantId = (int)AbpSession.TenantId;
            }

            var id = await _testingTemplateRepository.InsertAndGetIdAsync(testingTemplate);

            foreach (var item in input.Attributes)
            {
                await _testingTemplateAttributesRepository.InsertAsync(new TestingAttrribute()
                {
                    TestAttribute = item.TestAttribute,
                    TestingTemplateId = id,
                    Weight = item.Weight  
                });
            }
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
            var frequencyFilter = (Frequency)input.FrequencyFilter;

            var filteredTestingTemplates = _testingTemplateRepository.GetAll()
                        .Include(e => e.DepartmentRiskControlFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.DetailedInstructions.Contains(input.Filter) || e.Title.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title == input.TitleFilter)
                        .WhereIf(input.FrequencyFilter > -1, e => e.Frequency == frequencyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DepartmentRiskControlCodeFilter), e => e.DepartmentRiskControlFk != null && e.DepartmentRiskControlFk.Code == input.DepartmentRiskControlCodeFilter);

            var query = (from o in filteredTestingTemplates
                         join o1 in _lookup_processRiskControlRepository.GetAll() on o.ProcessRiskControlId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetTestingTemplateForViewDto()
                         {
                             TestingTemplate = new TestingTemplateDto
                             {
                                 Code = o.Code,
                                 DetailedInstructions = o.DetailedInstructions,
                                 Title = o.Title,
                                 Frequency = o.Frequency.ToString(),
                                 Id = o.Id
                             },
                             DepartmentRiskControlCode = s1 == null ? "" : s1.Code.ToString()
                         });


            var testingTemplateListDtos = await query.ToListAsync();

            return _testingTemplatesExcelExporter.ExportToFile(testingTemplateListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_TestingTemplates)]
        public async Task<PagedResultDto<TestingTemplateDepartmentRiskControlLookupTableDto>> GetAllDepartmentRiskControlForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            var query = _lookup_processRiskControlRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Code.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var departmentRiskControlList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TestingTemplateDepartmentRiskControlLookupTableDto>();
            foreach (var departmentRiskControl in departmentRiskControlList)
            {
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