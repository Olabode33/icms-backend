using ICMSDemo.ProcessRisks;
using Abp.Organizations;
using ICMSDemo.Controls;

using ICMSDemo;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.ProcessRiskControls.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ICMSDemo.Processes;
using ICMSDemo.TestingTemplates;

namespace ICMSDemo.ProcessRiskControls
{
    [AbpAuthorize(AppPermissions.Pages_ProcessRiskControls)]
    public class ProcessRiskControlsAppService : ICMSDemoAppServiceBase, IProcessRiskControlsAppService
    {
        private readonly IRepository<ProcessRiskControl> _processRiskControlRepository;
        private readonly IRepository<ProcessRisk, int> _lookup_processRiskRepository;
        private readonly IRepository<Process, long> _lookup_processRepository;
        private readonly IRepository<Control, int> _lookup_controlRepository;
        private readonly IRepository<TestingTemplate> _testingTemplateRepository;

        public ProcessRiskControlsAppService(
            IRepository<ProcessRiskControl> processRiskControlRepository, 
            IRepository<ProcessRisk, int> lookup_processRiskRepository, 
            IRepository<Process, long> lookup_processRepository, 
            IRepository<Control, int> lookup_controlRepository,
            IRepository<TestingTemplate> testingTemplateRepository)
        {
            _processRiskControlRepository = processRiskControlRepository;
            _lookup_processRiskRepository = lookup_processRiskRepository;
            _lookup_processRepository = lookup_processRepository;
            _lookup_controlRepository = lookup_controlRepository;
            _testingTemplateRepository = testingTemplateRepository;
        }

        public async Task<PagedResultDto<GetProcessRiskControlForViewDto>> GetAll(GetAllProcessRiskControlsInput input)
        {
            var frequencyFilter = (Frequency)input.FrequencyFilter;

            var filteredProcessRiskControls = _processRiskControlRepository.GetAll()
                        .Include(e => e.ProcessRiskFk)
                        .Include(e => e.ProcessFk)
                        .Include(e => e.ControlFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Notes.Contains(input.Filter))
                        .WhereIf(input.FrequencyFilter > -1, e => e.Frequency == frequencyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessRiskCodeFilter), e => e.ProcessRiskFk != null && e.ProcessRiskFk.Code == input.ProcessRiskCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.ProcessFk != null && e.ProcessFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ControlNameFilter), e => e.ControlFk != null && e.ControlFk.Name == input.ControlNameFilter);

            var pagedAndFilteredProcessRiskControls = filteredProcessRiskControls
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var processRiskControls = from o in pagedAndFilteredProcessRiskControls
                                      join o1 in _lookup_processRiskRepository.GetAll() on o.ProcessRiskId equals o1.Id into j1
                                      from s1 in j1.DefaultIfEmpty()

                                      join o2 in _lookup_processRepository.GetAll() on o.ProcessId equals o2.Id into j2
                                      from s2 in j2.DefaultIfEmpty()

                                      join o3 in _lookup_controlRepository.GetAll() on o.ControlId equals o3.Id into j3
                                      from s3 in j3.DefaultIfEmpty()

                                      select new GetProcessRiskControlForViewDto()
                                      {
                                          ProcessRiskControl = new ProcessRiskControlDto
                                          {
                                              Code = o.Code,
                                              Notes = o.Notes,
                                              Frequency = o.Frequency,
                                              Cascade = o.Cascade,
                                              Id = o.Id
                                          },
                                          ProcessRiskCode = s1 == null ? "" : s1.Code.ToString(),
                                          OrganizationUnitDisplayName = s2 == null ? "" : s2.DisplayName.ToString(),
                                          ControlName = s3 == null ? "" : s3.Name.ToString()
                                      };

            var totalCount = await filteredProcessRiskControls.CountAsync();

            return new PagedResultDto<GetProcessRiskControlForViewDto>(
                totalCount,
                await processRiskControls.ToListAsync()
            );
        }

        public async Task<ListResultDto<GetProcessRiskControlForViewDto>> GetAllForProcess(GetAllProcessRiskControlsInput input)
        {
            var processCode = await OrganizationUnitManager.GetCodeAsync((long)input.ProcessId);

            string[] roots = processCode.Split(".");
            string previousCode = string.Empty;
            List<string> codes = new List<string>();

            foreach (var item in roots)
            {
                previousCode = previousCode == string.Empty ? item : previousCode + "." + item;
                codes.Add(previousCode);
            }

            var departments = await _lookup_processRepository.GetAllListAsync(x => codes.Any(e => e == x.Code));


            var filteredDepartmentRiskControls = _processRiskControlRepository.GetAll()
                                                .Include(e => e.ControlFk)
                                                .Include(e => e.ProcessRiskFk)
                                                .ThenInclude(x => x.ProcessFk)
                                                .Where(x => x.ProcessId == input.ProcessId || (x.ProcessId != input.ProcessId && x.Cascade))
            .Select(x => new GetProcessRiskControlForViewDto()
            {
                ProcessRiskControl = ObjectMapper.Map<ProcessRiskControlDto>(x),
                ProcessRiskCode = x.ProcessFk == null ? "" : x.ProcessFk.Code,
                ControlName = x.ControlFk.Name + " [" + x.ControlFk.Code + "]",
                Inherited = x.ProcessId == null ? true : false,
            });

            var pagedAndFilteredDepartmentRiskControls = await filteredDepartmentRiskControls.ToListAsync();

            var outputList = pagedAndFilteredDepartmentRiskControls.Where(x => codes.Any(e => e == x.ProcessRiskCode));

            var totalCount = outputList.Count();

            foreach (var item in outputList)
            {
                item.TestingTemplates = await GetForProcessControl(item.ProcessRiskControl.Id);
            }

            return new ListResultDto<GetProcessRiskControlForViewDto>(
                outputList.Skip(input.SkipCount).Take(input.MaxResultCount).ToList()
            );
        }

        private async Task<ListResultDto<TestingTemplates.Dtos.GetTestingTemplateForViewDto>> GetForProcessControl(int input)
        {
            var filteredTestingTemplates = _testingTemplateRepository.GetAll()
                        .Include(e => e.DepartmentRiskControlFk)
                        .Where(x => x.ProcessRiskControlId == input);

            var testingTemplates = from o in filteredTestingTemplates
                                   select new TestingTemplates.Dtos.GetTestingTemplateForViewDto()
                                   {
                                       TestingTemplate = new TestingTemplates.Dtos.TestingTemplateDto
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

            return new ListResultDto<TestingTemplates.Dtos.GetTestingTemplateForViewDto>(await testingTemplates.ToListAsync());
        }


        [AbpAuthorize(AppPermissions.Pages_ProcessRiskControls_Edit)]
        public async Task<GetProcessRiskControlForEditOutput> GetProcessRiskControlForEdit(EntityDto input)
        {
            var processRiskControl = await _processRiskControlRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProcessRiskControlForEditOutput { ProcessRiskControl = ObjectMapper.Map<CreateOrEditProcessRiskControlDto>(processRiskControl) };

            if (output.ProcessRiskControl.ProcessRiskId != null)
            {
                var _lookupProcessRisk = await _lookup_processRiskRepository.FirstOrDefaultAsync((int)output.ProcessRiskControl.ProcessRiskId);
                output.ProcessRiskCode = _lookupProcessRisk.Code.ToString();
            }

            if (output.ProcessRiskControl.ProcessId != null)
            {
                var _lookupOrganizationUnit = await _lookup_processRepository.FirstOrDefaultAsync((long)output.ProcessRiskControl.ProcessId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

            if (output.ProcessRiskControl.ControlId != null)
            {
                var _lookupControl = await _lookup_controlRepository.FirstOrDefaultAsync((int)output.ProcessRiskControl.ControlId);
                output.ControlName = _lookupControl.Name.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProcessRiskControlDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProcessRiskControls_Create)]
        protected virtual async Task Create(CreateOrEditProcessRiskControlDto input)
        {
            var processRiskControl = ObjectMapper.Map<ProcessRiskControl>(input);


            if (AbpSession.TenantId != null)
            {
                processRiskControl.TenantId = (int)AbpSession.TenantId;
            }


            await _processRiskControlRepository.InsertAsync(processRiskControl);
        }

        [AbpAuthorize(AppPermissions.Pages_ProcessRiskControls_Edit)]
        protected virtual async Task Update(CreateOrEditProcessRiskControlDto input)
        {
            var processRiskControl = await _processRiskControlRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, processRiskControl);
        }

        [AbpAuthorize(AppPermissions.Pages_ProcessRiskControls_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _processRiskControlRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_ProcessRiskControls)]
        public async Task<PagedResultDto<ProcessRiskControlProcessRiskLookupTableDto>> GetAllProcessRiskForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_processRiskRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Code != null && e.Code.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var processRiskList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProcessRiskControlProcessRiskLookupTableDto>();
            foreach (var processRisk in processRiskList)
            {
                lookupTableDtoList.Add(new ProcessRiskControlProcessRiskLookupTableDto
                {
                    Id = processRisk.Id,
                    DisplayName = processRisk.Code?.ToString()
                });
            }

            return new PagedResultDto<ProcessRiskControlProcessRiskLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProcessRiskControls)]
        public async Task<PagedResultDto<ProcessRiskControlOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_processRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName != null && e.DisplayName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProcessRiskControlOrganizationUnitLookupTableDto>();
            foreach (var organizationUnit in organizationUnitList)
            {
                lookupTableDtoList.Add(new ProcessRiskControlOrganizationUnitLookupTableDto
                {
                    Id = organizationUnit.Id,
                    DisplayName = organizationUnit.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<ProcessRiskControlOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProcessRiskControls)]
        public async Task<PagedResultDto<ProcessRiskControlControlLookupTableDto>> GetAllControlForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_controlRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var controlList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProcessRiskControlControlLookupTableDto>();
            foreach (var control in controlList)
            {
                lookupTableDtoList.Add(new ProcessRiskControlControlLookupTableDto
                {
                    Id = control.Id,
                    DisplayName = control.Name?.ToString()
                });
            }

            return new PagedResultDto<ProcessRiskControlControlLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}