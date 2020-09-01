using ICMSDemo.Authorization.Users;
using Abp.Organizations;

using ICMSDemo;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.LossEvents.Exporting;
using ICMSDemo.LossEvents.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using static ICMSDemo.IcmsEnums;

namespace ICMSDemo.LossEvents
{
    [AbpAuthorize(AppPermissions.Pages_LossEvents)]
    public class LossEventsAppService : ICMSDemoAppServiceBase, ILossEventsAppService
    {
        private readonly IRepository<LossEvent> _lossEventRepository;
        private readonly IRepository<LossType> _lossTypeRepository;
        private readonly ILossEventsExcelExporter _lossEventsExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository;


        public LossEventsAppService(IRepository<LossEvent> lossEventRepository,
            ILossEventsExcelExporter lossEventsExcelExporter,
            IRepository<User, long> lookup_userRepository,
            IRepository<OrganizationUnit, long> lookup_organizationUnitRepository,
            IRepository<LossType> lossTypeRepository)
        {
            _lossEventRepository = lossEventRepository;
            _lossEventsExcelExporter = lossEventsExcelExporter;
            _lookup_userRepository = lookup_userRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _lossTypeRepository = lossTypeRepository;
        }

        public async Task<PagedResultDto<GetLossEventForViewDto>> GetAll(GetAllLossEventsInput input)
        {
            var lossTypeFilter = input.LossTypeFilter.HasValue
                        ? (LossEventTypeEnums)input.LossTypeFilter
                        : default;
            var statusFilter = input.StatusFilter.HasValue
                ? (Status)input.StatusFilter
                : default;

            var filteredLossEvents = _lossEventRepository.GetAll()
                        .Include(e => e.EmployeeUserFk)
                        .Include(e => e.DepartmentFk)
                        .Include(e => e.LossTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        //.WhereIf(input.LossTypeFilter.HasValue && input.LossTypeFilter > -1, e => e.LossTypeId == lossTypeFilter)
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.EmployeeUserFk != null && e.EmployeeUserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.DepartmentFk != null && e.DepartmentFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

            var pagedAndFilteredLossEvents = filteredLossEvents
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var lossEvents = from o in pagedAndFilteredLossEvents
                             join o1 in _lookup_userRepository.GetAll() on o.EmployeeUserId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _lookup_organizationUnitRepository.GetAll() on o.DepartmentId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             select new GetLossEventForViewDto()
                             {
                                 LossEvent = new LossEventDto
                                 {
                                     Amount = o.Amount,
                                     DateOccured = o.DateOccured,
                                     DateDiscovered = o.DateDiscovered,
                                     LossTypeId = o.LossTypeId,
                                     Status = o.Status,
                                     LossCategory = o.LossCategory,
                                     Id = o.Id
                                 },
                                 UserName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                                 OrganizationUnitDisplayName = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString(),
                                 LossTypeName = o.LossTypeFk == null ? "" : o.LossTypeFk.Name
                             };

            var totalCount = await filteredLossEvents.CountAsync();

            return new PagedResultDto<GetLossEventForViewDto>(
                totalCount,
                await lossEvents.ToListAsync()
            );
        }

        public async Task<GetLossEventForViewDto> GetLossEventForView(int id)
        {
            var lossEvent = await _lossEventRepository.GetAsync(id);

            var output = new GetLossEventForViewDto { LossEvent = ObjectMapper.Map<LossEventDto>(lossEvent) };

            if (output.LossEvent.EmployeeUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.LossEvent.EmployeeUserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.LossEvent.DepartmentId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.LossEvent.DepartmentId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit?.DisplayName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_LossEvents_Edit)]
        public async Task<GetLossEventForEditOutput> GetLossEventForEdit(EntityDto input)
        {
            var lossEvent = await _lossEventRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLossEventForEditOutput { LossEvent = ObjectMapper.Map<CreateOrEditLossEventDto>(lossEvent) };

            if (output.LossEvent.EmployeeUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.LossEvent.EmployeeUserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.LossEvent.DepartmentId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.LossEvent.DepartmentId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit?.DisplayName?.ToString();
            }

            if (output.LossEvent.LossTypeId != null)
            {
                var lossType = await _lossTypeRepository.FirstOrDefaultAsync(e => e.Id == output.LossEvent.LossTypeId);
                output.LossEventTypeName = lossType.Name;
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLossEventDto input)
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

        [AbpAuthorize(AppPermissions.Pages_LossEvents_Create)]
        protected virtual async Task Create(CreateOrEditLossEventDto input)
        {
            var lossEvent = ObjectMapper.Map<LossEvent>(input);


            if (AbpSession.TenantId != null)
            {
                lossEvent.TenantId = (int?)AbpSession.TenantId;
            }


            await _lossEventRepository.InsertAsync(lossEvent);
        }

        [AbpAuthorize(AppPermissions.Pages_LossEvents_Edit)]
        protected virtual async Task Update(CreateOrEditLossEventDto input)
        {
            var lossEvent = await _lossEventRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, lossEvent);
        }

        [AbpAuthorize(AppPermissions.Pages_LossEvents_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _lossEventRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLossEventsToExcel(GetAllLossEventsForExcelInput input)
        {
            var lossTypeFilter = input.LossTypeFilter.HasValue
                        ? (LossEventTypeEnums)input.LossTypeFilter
                        : default;
            var statusFilter = input.StatusFilter.HasValue
                ? (Status)input.StatusFilter
                : default;

            var filteredLossEvents = _lossEventRepository.GetAll()
                        .Include(e => e.EmployeeUserFk)
                        .Include(e => e.DepartmentFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        //.WhereIf(input.LossTypeFilter.HasValue && input.LossTypeFilter > -1, e => e.LossTypeId == lossTypeFilter)
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.EmployeeUserFk != null && e.EmployeeUserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.DepartmentFk != null && e.DepartmentFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

            var query = (from o in filteredLossEvents
                         join o1 in _lookup_userRepository.GetAll() on o.EmployeeUserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_organizationUnitRepository.GetAll() on o.DepartmentId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetLossEventForViewDto()
                         {
                             LossEvent = new LossEventDto
                             {
                                 Amount = o.Amount,
                                 DateOccured = o.DateOccured,
                                 DateDiscovered = o.DateDiscovered,
                                 LossTypeId = o.LossTypeId,
                                 Status = o.Status,
                                 LossCategory = o.LossCategory,
                                 Id = o.Id
                             },
                             UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             OrganizationUnitDisplayName = s2 == null || s2.DisplayName == null ? "" : s2.DisplayName.ToString()
                         });


            var lossEventListDtos = await query.ToListAsync();

            return _lossEventsExcelExporter.ExportToFile(lossEventListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_LossEvents)]
        public async Task<PagedResultDto<LossEventUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LossEventUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new LossEventUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<LossEventUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_LossEvents)]
        public async Task<PagedResultDto<LossEventOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_organizationUnitRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName != null && e.DisplayName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LossEventOrganizationUnitLookupTableDto>();
            foreach (var organizationUnit in organizationUnitList)
            {
                lookupTableDtoList.Add(new LossEventOrganizationUnitLookupTableDto
                {
                    Id = organizationUnit.Id,
                    DisplayName = organizationUnit.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<LossEventOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}