using ICMSDemo.DepartmentRisks;
using ICMSDemo.Controls;

using ICMSDemo;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.DepartmentRiskControls.Exporting;
using ICMSDemo.DepartmentRiskControls.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ICMSDemo.Departments;

namespace ICMSDemo.DepartmentRiskControls
{
    [AbpAuthorize(AppPermissions.Pages_DepartmentRiskControls)]
    public class DepartmentRiskControlsAppService : ICMSDemoAppServiceBase, IDepartmentRiskControlsAppService
    {
        private readonly IRepository<DepartmentRiskControl> _departmentRiskControlRepository;
        private readonly IDepartmentRiskControlsExcelExporter _departmentRiskControlsExcelExporter;
        private readonly IRepository<DepartmentRisk, int> _lookup_departmentRiskRepository;
        private readonly IRepository<Department, long> _lookup_departmentRepository;
        private readonly IRepository<Control, int> _lookup_controlRepository;


        public DepartmentRiskControlsAppService(IRepository<Department, long> lookup_departmentRepository,
            IRepository<DepartmentRiskControl> departmentRiskControlRepository, IDepartmentRiskControlsExcelExporter departmentRiskControlsExcelExporter, IRepository<DepartmentRisk, int> lookup_departmentRiskRepository, IRepository<Control, int> lookup_controlRepository)
        {
            _departmentRiskControlRepository = departmentRiskControlRepository;
            _departmentRiskControlsExcelExporter = departmentRiskControlsExcelExporter;
            _lookup_departmentRiskRepository = lookup_departmentRiskRepository;
            _lookup_controlRepository = lookup_controlRepository;
            _lookup_departmentRepository = lookup_departmentRepository;

        }

        public async Task<PagedResultDto<GetDepartmentRiskControlForViewDto>> GetAll(GetAllDepartmentRiskControlsInput input)
        {
            var frequencyFilter = (Frequency)input.FrequencyFilter;

            var filteredDepartmentRiskControls = _departmentRiskControlRepository.GetAll()
                        .Include(e => e.DepartmentRiskFk)
                        .Include(e => e.ControlFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Notes.Contains(input.Filter))
                        .WhereIf(input.FrequencyFilter > -1, e => e.Frequency == frequencyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DepartmentRiskCodeFilter), e => e.DepartmentRiskFk != null && e.DepartmentRiskFk.Code == input.DepartmentRiskCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ControlCodeFilter), e => e.ControlFk != null && e.ControlFk.Code == input.ControlCodeFilter);

            var pagedAndFilteredDepartmentRiskControls = filteredDepartmentRiskControls
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var departmentRiskControls = from o in pagedAndFilteredDepartmentRiskControls
                                         join o1 in _lookup_departmentRiskRepository.GetAll() on o.DepartmentRiskId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         join o2 in _lookup_controlRepository.GetAll() on o.ControlId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()

                                         select new GetDepartmentRiskControlForViewDto()
                                         {
                                             DepartmentRiskControl = new DepartmentRiskControlDto
                                             {
                                                 Code = o.Code,
                                                 Notes = o.Notes,
                                                 Frequency = o.Frequency,
                                                 Id = o.Id
                                             },
                                             DepartmentRiskCode = s1 == null ? "" : s1.Code.ToString(),
                                             ControlCode = s2 == null ? "" : s2.Code.ToString()
                                         };

            var totalCount = await filteredDepartmentRiskControls.CountAsync();

            return new PagedResultDto<GetDepartmentRiskControlForViewDto>(
                totalCount,
                await departmentRiskControls.ToListAsync()
            );
        }


        public async Task<PagedResultDto<GetDepartmentRiskControlForViewDto>> GetAllForDepartment(GetAllDepartmentRiskControlsInput input)
        {
            var departmentCode = await OrganizationUnitManager.GetCodeAsync((long)input.DepartmentId);

            string[] roots = departmentCode.Split(".");
            string previousCode = string.Empty;
            List<string> codes = new List<string>();

            foreach (var item in roots)
            {
                previousCode = previousCode == string.Empty ? item : previousCode + "." + item;
                codes.Add(previousCode);
            }

            var departments = await _lookup_departmentRepository.GetAllListAsync(x => codes.Any(e => e == x.Code));


            var filteredDepartmentRiskControls = _departmentRiskControlRepository.GetAll()
                                                .Include(e => e.ControlFk)
                                                .Include(e => e.DepartmentRiskFk)
                                                .ThenInclude(x => x.DepartmentFk)
                                                .Where(x => x.DepartmentId == input.DepartmentId || (x.DepartmentId != input.DepartmentId && x.Cascade))
            .Select(x => new GetDepartmentRiskControlForViewDto()
            {
                DepartmentRiskControl = new DepartmentRiskControlDto
                {
                    Code = x.Code,
                    Notes = x.Notes,
                    Frequency = x.Frequency,
                    Id = x.Id,
                    DepartmentId = x.DepartmentId,
                    Inherited = x.DepartmentId == null ? true : false,
                    DepartmentRiskId = x.DepartmentRiskId,
                    ControlId = x.ControlId,
                    DepartmentCode = x.DepartmentRiskFk.DepartmentFk.Code
                },
                DepartmentRiskCode = x.DepartmentRiskFk.Code,
                ControlCode = x.ControlFk.Name + " [" + x.ControlFk.Code + "]"
            });

            var pagedAndFilteredDepartmentRiskControls = await filteredDepartmentRiskControls.ToListAsync();

            var outputList = pagedAndFilteredDepartmentRiskControls.Where(x => codes.Any(e => e == x.DepartmentRiskControl.DepartmentCode));

            var totalCount = outputList.Count();

            return new PagedResultDto<GetDepartmentRiskControlForViewDto>(
                totalCount, outputList.Skip(input.SkipCount).Take(input.MaxResultCount).ToList()
            ); 
        }

        public async Task<GetDepartmentRiskControlForViewDto> GetDepartmentRiskControlForView(int id)
        {
            var departmentRiskControl = await _departmentRiskControlRepository.GetAsync(id);

            var output = new GetDepartmentRiskControlForViewDto { DepartmentRiskControl = ObjectMapper.Map<DepartmentRiskControlDto>(departmentRiskControl) };

            if (output.DepartmentRiskControl.DepartmentRiskId != null)
            {
                var _lookupDepartmentRisk = await _lookup_departmentRiskRepository.FirstOrDefaultAsync((int)output.DepartmentRiskControl.DepartmentRiskId);
                output.DepartmentRiskCode = _lookupDepartmentRisk.Code.ToString();
            }

            if (output.DepartmentRiskControl.ControlId != null)
            {
                var _lookupControl = await _lookup_controlRepository.FirstOrDefaultAsync((int)output.DepartmentRiskControl.ControlId);
                output.ControlCode = _lookupControl.Code.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DepartmentRiskControls_Edit)]
        public async Task<GetDepartmentRiskControlForEditOutput> GetDepartmentRiskControlForEdit(EntityDto input)
        {
            var departmentRiskControl = await _departmentRiskControlRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDepartmentRiskControlForEditOutput { DepartmentRiskControl = ObjectMapper.Map<CreateOrEditDepartmentRiskControlDto>(departmentRiskControl) };

            if (output.DepartmentRiskControl.DepartmentRiskId != null)
            {
                var _lookupDepartmentRisk = await _lookup_departmentRiskRepository.FirstOrDefaultAsync((int)output.DepartmentRiskControl.DepartmentRiskId);
                output.DepartmentRiskCode = _lookupDepartmentRisk.Code.ToString();
            }

            if (output.DepartmentRiskControl.ControlId != null)
            {
                var _lookupControl = await _lookup_controlRepository.FirstOrDefaultAsync((int)output.DepartmentRiskControl.ControlId);
                output.ControlCode = _lookupControl.Code.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDepartmentRiskControlDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DepartmentRiskControls_Create)]
        protected virtual async Task Create(CreateOrEditDepartmentRiskControlDto input)
        {
            var departmentRiskControl = ObjectMapper.Map<DepartmentRiskControl>(input);


            if (AbpSession.TenantId != null)
            {
                departmentRiskControl.TenantId = (int)AbpSession.TenantId;
            }


            await _departmentRiskControlRepository.InsertAsync(departmentRiskControl);
        }

        [AbpAuthorize(AppPermissions.Pages_DepartmentRiskControls_Edit)]
        protected virtual async Task Update(CreateOrEditDepartmentRiskControlDto input)
        {
            var departmentRiskControl = await _departmentRiskControlRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, departmentRiskControl);
        }

        [AbpAuthorize(AppPermissions.Pages_DepartmentRiskControls_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _departmentRiskControlRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDepartmentRiskControlsToExcel(GetAllDepartmentRiskControlsForExcelInput input)
        {
            var frequencyFilter = (Frequency)input.FrequencyFilter;

            var filteredDepartmentRiskControls = _departmentRiskControlRepository.GetAll()
                        .Include(e => e.DepartmentRiskFk)
                        .Include(e => e.ControlFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Notes.Contains(input.Filter))
                        .WhereIf(input.FrequencyFilter > -1, e => e.Frequency == frequencyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DepartmentRiskCodeFilter), e => e.DepartmentRiskFk != null && e.DepartmentRiskFk.Code == input.DepartmentRiskCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ControlCodeFilter), e => e.ControlFk != null && e.ControlFk.Code == input.ControlCodeFilter);

            var query = (from o in filteredDepartmentRiskControls
                         join o1 in _lookup_departmentRiskRepository.GetAll() on o.DepartmentRiskId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_controlRepository.GetAll() on o.ControlId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetDepartmentRiskControlForViewDto()
                         {
                             DepartmentRiskControl = new DepartmentRiskControlDto
                             {
                                 Code = o.Code,
                                 Notes = o.Notes,
                                 Frequency = o.Frequency,
                                 Id = o.Id
                             },
                             DepartmentRiskCode = s1 == null ? "" : s1.Code.ToString(),
                             ControlCode = s2 == null ? "" : s2.Code.ToString()
                         });


            var departmentRiskControlListDtos = await query.ToListAsync();

            return _departmentRiskControlsExcelExporter.ExportToFile(departmentRiskControlListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_DepartmentRiskControls)]
        public async Task<PagedResultDto<DepartmentRiskControlDepartmentRiskLookupTableDto>> GetAllDepartmentRiskForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_departmentRiskRepository.GetAll().
               Include(x => x.DepartmentFk)
               .WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Code.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var departmentRiskList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DepartmentRiskControlDepartmentRiskLookupTableDto>();
            foreach (var departmentRisk in departmentRiskList)
            {
                lookupTableDtoList.Add(new DepartmentRiskControlDepartmentRiskLookupTableDto
                {
                    Id = departmentRisk.Id,
                    DisplayName = departmentRisk.RiskFk.Name + " [" + departmentRisk.Cascade.ToString() + "]"
                });
            }

            return new PagedResultDto<DepartmentRiskControlDepartmentRiskLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_DepartmentRiskControls)]
        public async Task<PagedResultDto<DepartmentRiskControlControlLookupTableDto>> GetAllControlForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_controlRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Code.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var controlList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DepartmentRiskControlControlLookupTableDto>();
            foreach (var control in controlList)
            {
                lookupTableDtoList.Add(new DepartmentRiskControlControlLookupTableDto
                {
                    Id = control.Id,
                    DisplayName = control.Name + "[" + control.Code?.ToString() + "]"
                });
            }

            return new PagedResultDto<DepartmentRiskControlControlLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}