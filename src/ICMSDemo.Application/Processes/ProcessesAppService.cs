using ICMSDemo.Authorization.Users;
using Abp.Organizations;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.Processes.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ICMSDemo.ProcessRisks;
using ICMSDemo.ProcessRiskControls;
using ICMSDemo.Organizations.Dto;

namespace ICMSDemo.Processes
{
    [AbpAuthorize(AppPermissions.Pages_Processes)]
    public class ProcessesAppService : ICMSDemoAppServiceBase, IProcessesAppService
    {
        private readonly IRepository<Process, long> _processRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository;
        private readonly IRepository<ProcessRisk> _lookup_processRiskRepository;
        private readonly IRepository<ProcessRiskControl> _lookup_processRiskControlRepository;


        public ProcessesAppService(
            IRepository<Process, long> processRepository, 
            IRepository<User, long> lookup_userRepository, 
            IRepository<OrganizationUnit, long> lookup_organizationUnitRepository,
            IRepository<ProcessRisk> lookup_processRiskRepository,
            IRepository<ProcessRiskControl> lookup_processRiskControlRepository
            )
        {
            _processRepository = processRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _lookup_processRiskRepository = lookup_processRiskRepository;
            _lookup_processRiskControlRepository = lookup_processRiskControlRepository;
        }

        public async Task<ListResultDto<OrganizationUnitDto>> GetProcesses(EntityDto<long?> input)
        {

            var processes = await _processRepository.GetAll()
                                                    .Include(x => x.DepartmentFk)
                                                    .WhereIf(input.Id != null && input.Id > 0, x => x.DepartmentId == (long)input.Id || (x.DepartmentId != input.Id && x.Casade)).ToListAsync();

            var processRiskCounts = await _lookup_processRiskRepository.GetAll()
                .GroupBy(x => x.ProcessId)
                .Select(groupedUsers => new
                {
                    processId = groupedUsers.Key,
                    count = groupedUsers.Count()
                }).ToDictionaryAsync(x => x.processId, y => y.count);

            var processRiskControlCounts = await _lookup_processRiskControlRepository.GetAll()
                .GroupBy(x => x.ProcessId)
                .Select(groupedRoles => new
                {
                    processId = groupedRoles.Key,
                    count = groupedRoles.Count()
                }).ToDictionaryAsync(x => x.processId, y => y.count);

            var processsList = processes.Select(ou =>
                                {
                                    var organizationUnitDto = ObjectMapper.Map<OrganizationUnitDto>(ou);
                                    organizationUnitDto.MemberCount = processRiskCounts.ContainsKey(ou.Id) ? processRiskCounts[ou.Id] : 0;
                                    organizationUnitDto.RoleCount = processRiskControlCounts.ContainsKey(ou.Id) ? processRiskControlCounts[ou.Id] : 0;
                                    organizationUnitDto.DepartmentCode = ou.DepartmentFk == null ? "" : ou.DepartmentFk.Code;
                                    organizationUnitDto.DepartmentId = ou.DepartmentId;
                                    return organizationUnitDto;
                                }).ToList();

            //Can cascade if getting for a deparment
            if (input.Id != null && input.Id > 0)
            {
                var processCode = await OrganizationUnitManager.GetCodeAsync((long)input.Id);

                string[] roots = processCode.Split(".");
                string previousCode = string.Empty;
                List<string> codes = new List<string>();

                foreach (var item in roots)
                {
                    previousCode = previousCode == string.Empty ? item : previousCode + "." + item;
                    codes.Add(previousCode);
                }

                var departments = await _lookup_organizationUnitRepository.GetAllListAsync(x => codes.Any(e => e == x.Code));
                processsList = processsList.Where(x => codes.Any(e => e == x.DepartmentCode)).ToList();
            }

            return new ListResultDto<OrganizationUnitDto>(processsList);
        }

        public async Task<PagedResultDto<GetProcessForViewDto>> GetAll(GetAllProcessesInput input)
        {

            var filteredProcesses = _processRepository.GetAll()
                        .Include(e => e.OwnerFk)
                        .Include(e => e.DepartmentFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.OwnerFk != null && e.OwnerFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.DepartmentFk != null && e.DepartmentFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

            var pagedAndFilteredProcesses = filteredProcesses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var processes = from o in pagedAndFilteredProcesses
                            join o1 in _lookup_userRepository.GetAll() on o.OwnerId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _lookup_organizationUnitRepository.GetAll() on o.DepartmentId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            select new GetProcessForViewDto()
                            {
                                Process = new ProcessDto
                                {
                                    Name = o.Name,
                                    Description = o.Description,
                                    Casade = o.Casade,
                                    Id = o.Id
                                },
                                UserName = s1 == null ? "" : s1.Name.ToString(),
                                OrganizationUnitDisplayName = s2 == null ? "" : s2.DisplayName.ToString()
                            };

            var totalCount = await filteredProcesses.CountAsync();

            return new PagedResultDto<GetProcessForViewDto>(
                totalCount,
                await processes.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Processes_Edit)]
        public async Task<GetProcessForEditOutput> GetProcessForEdit(EntityDto<long> input)
        {
            var process = await _processRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProcessForEditOutput { Process = ObjectMapper.Map<CreateOrEditProcessDto>(process) };

            if (output.Process.OwnerId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Process.OwnerId);
                output.UserName = _lookupUser.Name.ToString();
            }

            if (output.Process.DepartmentId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Process.DepartmentId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProcessDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Processes_Create)]
        protected virtual async Task Create(CreateOrEditProcessDto input)
        {
            var process = ObjectMapper.Map<Process>(input);

            process.DisplayName = input.Name;
            process.ParentId = input.ParentId;

            if (AbpSession.TenantId != null)
            {
                process.TenantId = (int?)AbpSession.TenantId;
            }

            await OrganizationUnitManager.CreateAsync(process);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Processes_Edit)]
        protected virtual async Task Update(CreateOrEditProcessDto input)
        {
            var process = await _processRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, process);
        }

        [AbpAuthorize(AppPermissions.Pages_Processes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _processRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Processes)]
        public async Task<PagedResultDto<ProcessUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProcessUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new ProcessUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<ProcessUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Processes)]
        public async Task<PagedResultDto<ProcessOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_organizationUnitRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName != null && e.DisplayName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProcessOrganizationUnitLookupTableDto>();
            foreach (var organizationUnit in organizationUnitList)
            {
                lookupTableDtoList.Add(new ProcessOrganizationUnitLookupTableDto
                {
                    Id = organizationUnit.Id,
                    DisplayName = organizationUnit.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<ProcessOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input)
        {
            await OrganizationUnitManager.MoveAsync(input.Id, input.NewParentId);

            return CreateOrganizationUnitDto(
                await _processRepository.GetAsync(input.Id)
                );
        }

        private OrganizationUnitDto CreateOrganizationUnitDto(OrganizationUnit organizationUnit)
        {
            var dto = ObjectMapper.Map<OrganizationUnitDto>(organizationUnit);
            dto.MemberCount = 0;
            return dto;
        }
    }
}