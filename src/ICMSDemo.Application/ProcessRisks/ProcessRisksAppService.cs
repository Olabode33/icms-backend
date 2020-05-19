﻿using Abp.Organizations;
using ICMSDemo.Risks;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.ProcessRisks.Exporting;
using ICMSDemo.ProcessRisks.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ICMSDemo.Processes;

namespace ICMSDemo.ProcessRisks
{
    [AbpAuthorize(AppPermissions.Pages_ProcessRisks)]
    public class ProcessRisksAppService : ICMSDemoAppServiceBase, IProcessRisksAppService
    {
        private readonly IRepository<ProcessRisk> _processRiskRepository;
        private readonly IProcessRisksExcelExporter _processRisksExcelExporter;
        private readonly IRepository<Process, long> _lookup_processRepository;
        private readonly IRepository<Risk, int> _lookup_riskRepository;


        public ProcessRisksAppService(
            IRepository<ProcessRisk> processRiskRepository, 
            IProcessRisksExcelExporter processRisksExcelExporter, 
            IRepository<Process, long> lookup_processRepository, 
            IRepository<Risk, int> lookup_riskRepository)
        {
            _processRiskRepository = processRiskRepository;
            _processRisksExcelExporter = processRisksExcelExporter;
            _lookup_processRepository = lookup_processRepository;
            _lookup_riskRepository = lookup_riskRepository;

        }

        public async Task<PagedResultDto<GetProcessRiskForViewDto>> GetAll(GetAllProcessRisksInput input)
        {

            var filteredProcessRisks = _processRiskRepository.GetAll()
                        .Include(e => e.ProcessFk)
                        .Include(e => e.RiskFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Comments.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CommentsFilter), e => e.Comments == input.CommentsFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.ProcessFk != null && e.ProcessFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskNameFilter), e => e.RiskFk != null && e.RiskFk.Name == input.RiskNameFilter);

            var pagedAndFilteredProcessRisks = filteredProcessRisks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var processRisks = from o in pagedAndFilteredProcessRisks
                               join o1 in _lookup_processRepository.GetAll() on o.ProcessId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               join o2 in _lookup_riskRepository.GetAll() on o.RiskId equals o2.Id into j2
                               from s2 in j2.DefaultIfEmpty()

                               select new GetProcessRiskForViewDto()
                               {
                                   ProcessRisk = new ProcessRiskDto
                                   {
                                       Code = o.Code,
                                       Comments = o.Comments,
                                       Cascade = o.Cascade,
                                       Id = o.Id
                                   },
                                   ProcessName = s1 == null ? "" : s1.DisplayName.ToString(),
                                   RiskName = s2 == null ? "" : s2.Name.ToString()
                               };

            var totalCount = await filteredProcessRisks.CountAsync();

            return new PagedResultDto<GetProcessRiskForViewDto>(
                totalCount,
                await processRisks.ToListAsync()
            );
        }

        public async Task<ListResultDto<GetProcessRiskForViewDto>> GetRiskForProcess(GetAllProcessRisksInput input)
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


            var filteredDepartmentRisks = from o in _processRiskRepository
                                          .GetAll()
                                          .Include(e => e.RiskFk)
                                          .Include(x => x.ProcessFk)
                                          .Where(x => x.ProcessId == input.ProcessId || (x.ProcessId != input.ProcessId && x.Cascade))
                                          select new GetProcessRiskForViewDto()
                                          {
                                              ProcessRisk = ObjectMapper.Map<ProcessRiskDto>(o),

                                              Inherited = o.ProcessId == input.ProcessId ? false : true,
                                              ProcessName = o.ProcessFk == null ? "" : o.ProcessFk.Name.ToString(),
                                              RiskName = o.RiskFk == null ? "" : o.RiskFk.Name.ToString(),
                                              Severity = o.RiskFk == null ? "" : o.RiskFk.Severity.ToString(),
                                              ProcessCode = o.ProcessFk == null ? "" : o.ProcessFk.Code
                                          };


            //var pagedAndFilteredDepartmentRisks = filteredDepartmentRisks
            //    .PageBy(input);

            //  var totalCount = await filteredDepartmentRisks.CountAsync();

            var lists = await filteredDepartmentRisks.ToListAsync();

            //fix later 
            lists = lists.Where(x => codes.Any(e => e == x.ProcessCode)).ToList();

            var totalCount = lists.Count;

            var output = lists.Skip(input.SkipCount).Take(input.MaxResultCount);

            return new ListResultDto<GetProcessRiskForViewDto>(lists);
        }


        public async Task<GetProcessRiskForViewDto> GetProcessRiskForView(int id)
        {
            var processRisk = await _processRiskRepository.GetAsync(id);

            var output = new GetProcessRiskForViewDto { ProcessRisk = ObjectMapper.Map<ProcessRiskDto>(processRisk) };

            if (output.ProcessRisk.ProcessId != null)
            {
                var _lookupOrganizationUnit = await _lookup_processRepository.FirstOrDefaultAsync((long)output.ProcessRisk.ProcessId);
                output.ProcessName = _lookupOrganizationUnit.DisplayName.ToString();
            }

            if (output.ProcessRisk.RiskId != null)
            {
                var _lookupRisk = await _lookup_riskRepository.FirstOrDefaultAsync((int)output.ProcessRisk.RiskId);
                output.RiskName = _lookupRisk.Name.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProcessRisks_Edit)]
        public async Task<GetProcessRiskForEditOutput> GetProcessRiskForEdit(EntityDto input)
        {
            var processRisk = await _processRiskRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProcessRiskForEditOutput { ProcessRisk = ObjectMapper.Map<CreateOrEditProcessRiskDto>(processRisk) };

            if (output.ProcessRisk.ProcessId != null)
            {
                var _lookupOrganizationUnit = await _lookup_processRepository.FirstOrDefaultAsync((long)output.ProcessRisk.ProcessId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

            if (output.ProcessRisk.RiskId != null)
            {
                var _lookupRisk = await _lookup_riskRepository.FirstOrDefaultAsync((int)output.ProcessRisk.RiskId);
                output.RiskName = _lookupRisk.Name.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProcessRiskDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProcessRisks_Create)]
        protected virtual async Task Create(CreateOrEditProcessRiskDto input)
        {
            var processRisk = ObjectMapper.Map<ProcessRisk>(input);
            var previousCount = await _processRiskRepository.CountAsync(x => x.DepartmentId == input.ProcessId);
            previousCount++;
            processRisk.Code = "PR-" + previousCount.ToString();

            if (AbpSession.TenantId != null)
            {
                processRisk.TenantId = (int)AbpSession.TenantId;
            }


            await _processRiskRepository.InsertAsync(processRisk);
        }

        [AbpAuthorize(AppPermissions.Pages_ProcessRisks_Edit)]
        protected virtual async Task Update(CreateOrEditProcessRiskDto input)
        {
            var processRisk = await _processRiskRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, processRisk);
        }

        [AbpAuthorize(AppPermissions.Pages_ProcessRisks_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _processRiskRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProcessRisksToExcel(GetAllProcessRisksForExcelInput input)
        {

            var filteredProcessRisks = _processRiskRepository.GetAll()
                        .Include(e => e.ProcessFk)
                        .Include(e => e.RiskFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Comments.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CommentsFilter), e => e.Comments == input.CommentsFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.ProcessFk != null && e.ProcessFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RiskNameFilter), e => e.RiskFk != null && e.RiskFk.Name == input.RiskNameFilter);

            var query = (from o in filteredProcessRisks
                         join o1 in _lookup_processRepository.GetAll() on o.ProcessId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_riskRepository.GetAll() on o.RiskId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProcessRiskForViewDto()
                         {
                             ProcessRisk = new ProcessRiskDto
                             {
                                 Code = o.Code,
                                 Comments = o.Comments,
                                 Cascade = o.Cascade,
                                 Id = o.Id
                             },
                             ProcessName = s1 == null ? "" : s1.DisplayName.ToString(),
                             RiskName = s2 == null ? "" : s2.Name.ToString()
                         });


            var processRiskListDtos = await query.ToListAsync();

            return _processRisksExcelExporter.ExportToFile(processRiskListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_ProcessRisks)]
        public async Task<PagedResultDto<ProcessRiskOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_processRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName != null && e.DisplayName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProcessRiskOrganizationUnitLookupTableDto>();
            foreach (var organizationUnit in organizationUnitList)
            {
                lookupTableDtoList.Add(new ProcessRiskOrganizationUnitLookupTableDto
                {
                    Id = organizationUnit.Id,
                    DisplayName = organizationUnit.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<ProcessRiskOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProcessRisks)]
        public async Task<PagedResultDto<ProcessRiskRiskLookupTableDto>> GetAllRiskForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_riskRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var riskList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProcessRiskRiskLookupTableDto>();
            foreach (var risk in riskList)
            {
                lookupTableDtoList.Add(new ProcessRiskRiskLookupTableDto
                {
                    Id = risk.Id,
                    DisplayName = risk.Name?.ToString()
                });
            }

            return new PagedResultDto<ProcessRiskRiskLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}