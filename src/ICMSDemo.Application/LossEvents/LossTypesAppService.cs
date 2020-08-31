
using ICMSDemo;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
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
    //[AbpAuthorize(AppPermissions.Pages_LossTypeColumns)]
    public class LossTypesAppService : ICMSDemoAppServiceBase
    {
        private readonly IRepository<LossType> _lossTypeRepository;
        private readonly IRepository<LossTypeColumn> _lossTypeColumnRepository;
        private readonly IRepository<LossTypeTrigger> _lossTypeTriggerRepository;


        public LossTypesAppService(IRepository<LossType> lossTypeRepository,
                IRepository<LossTypeColumn> lossTypeColumnRepository,
                IRepository<LossTypeTrigger> lossTypeTriggerRepository)
        {
            _lossTypeRepository = lossTypeRepository;
            _lossTypeColumnRepository = lossTypeColumnRepository;
            _lossTypeTriggerRepository = lossTypeTriggerRepository;
        }

        public async Task<PagedResultDto<GetLossTypeForViewDto>> GetAll(GetAllLossTypeColumnsInput input)
        {

            var filteredLossTypeColumns = _lossTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter));

            var pagedAndFilteredLossTypeColumns = filteredLossTypeColumns
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var lossTypeColumns = from o in pagedAndFilteredLossTypeColumns

                                  select new GetLossTypeForViewDto()
                                  {
                                      LossType = ObjectMapper.Map<LossTypeDto>(o)
                                  };

            var totalCount = await filteredLossTypeColumns.CountAsync();

            return new PagedResultDto<GetLossTypeForViewDto>(
                totalCount,
                await lossTypeColumns.ToListAsync()
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_LossTypeColumns_Edit)]
        public async Task<CreateOrEditLossTypeDto> GetLossTypeColumnForEdit(EntityDto input)
        {
            var lossTypeColumn = await _lossTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new CreateOrEditLossTypeDto { LossType = ObjectMapper.Map<LossTypeDto>(lossTypeColumn) };

            var columns = await _lossTypeColumnRepository.GetAll().Where(e => e.LossTypeId == input.Id)
                                                   .Select(e => ObjectMapper.Map<LossTypeColumnDto>(e))
                                                   .ToListAsync();

            var triggers = await _lossTypeTriggerRepository.GetAll().Include(e => e.NotifyUserFk)
                                                   .Where(e => e.LossTypeId == input.Id)
                                                   .Select(e => new GetLossTypeTriggerForView {
                                                       LossTypeTrigger =  ObjectMapper.Map<LossTypeTriggerDto>(e),
                                                       NotifyUserName = e.NotifyUserFk == null ? "" : e.NotifyUserFk.FullName
                                                   })
                                                   .ToListAsync();

            output.LossTypeColumns = columns;
            output.LossTypeTriggers = triggers;

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLossTypeDto input)
        {
            if (input.LossType.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        //[AbpAuthorize(AppPermissions.Pages_LossTypeColumns_Create)]
        protected virtual async Task Create(CreateOrEditLossTypeDto input)
        {
            var lossTypeColumn = ObjectMapper.Map<LossType>(input.LossType);


            if (AbpSession.TenantId != null)
            {
                lossTypeColumn.TenantId = (int?)AbpSession.TenantId;
            }


            var id = await _lossTypeRepository.InsertAndGetIdAsync(lossTypeColumn);
            await CurrentUnitOfWork.SaveChangesAsync();
            await SaveLossTypeColumn(input.LossTypeColumns, id);
            await SaveLossTypeTrigger(input.LossTypeTriggers, id);
        }

        //[AbpAuthorize(AppPermissions.Pages_LossTypeColumns_Edit)]
        protected virtual async Task Update(CreateOrEditLossTypeDto input)
        {
            var lossTypeColumn = await _lossTypeRepository.FirstOrDefaultAsync((int)input.LossType.Id);
            ObjectMapper.Map(input.LossType, lossTypeColumn);

            await SaveLossTypeColumn(input.LossTypeColumns, (int)input.LossType.Id);
            await SaveLossTypeTrigger(input.LossTypeTriggers, (int)input.LossType.Id);
        }

        private async Task SaveLossTypeColumn(List<LossTypeColumnDto> input, int lossTypeId)
        {   
            await _lossTypeColumnRepository.DeleteAsync(e => e.LossTypeId == lossTypeId);

            foreach (var item in input)
            {
                var column = ObjectMapper.Map<LossTypeColumn>(item);
                column.LossTypeId = lossTypeId;
                if (AbpSession.TenantId != null)
                {
                    column.TenantId = (int?)AbpSession.TenantId;
                }
                await _lossTypeColumnRepository.InsertAsync(column);
            }
        }

        private async Task SaveLossTypeTrigger(List<GetLossTypeTriggerForView> input, int lossTypeId)
        {
            await _lossTypeTriggerRepository.DeleteAsync(e => e.LossTypeId == lossTypeId);

            foreach (var item in input)
            {
                var column = ObjectMapper.Map<LossTypeTrigger>(item.LossTypeTrigger);
                column.LossTypeId = lossTypeId;
                if (AbpSession.TenantId != null)
                {
                    column.TenantId = (int?)AbpSession.TenantId;
                }
                await _lossTypeTriggerRepository.InsertAsync(column);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_LossTypeColumns_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _lossTypeRepository.DeleteAsync(input.Id);
        }
    }
}