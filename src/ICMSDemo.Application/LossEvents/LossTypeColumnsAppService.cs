
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
    [AbpAuthorize(AppPermissions.Pages_LossTypeColumns)]
    public class LossTypeColumnsAppService : ICMSDemoAppServiceBase, ILossTypeColumnsAppService
    {
        private readonly IRepository<LossTypeColumn> _lossTypeColumnRepository;


        public LossTypeColumnsAppService(IRepository<LossTypeColumn> lossTypeColumnRepository)
        {
            _lossTypeColumnRepository = lossTypeColumnRepository;

        }

        public async Task<PagedResultDto<GetLossTypeColumnForViewDto>> GetAll(GetAllLossTypeColumnsInput input)
        {

            var filteredLossTypeColumns = _lossTypeColumnRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ColumnName.Contains(input.Filter));

            var pagedAndFilteredLossTypeColumns = filteredLossTypeColumns
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var lossTypeColumns = from o in pagedAndFilteredLossTypeColumns
                                  select new GetLossTypeColumnForViewDto()
                                  {
                                      LossTypeColumn = new LossTypeColumnDto
                                      {
                                          ColumnName = o.ColumnName,
                                          DataType = o.DataType,
                                          Required = o.Required,
                                          //LossType = o.LossType,
                                          Minimum = o.Minimum,
                                          Maximum = o.Maximum,
                                          Id = o.Id
                                      }
                                  };

            var totalCount = await filteredLossTypeColumns.CountAsync();

            return new PagedResultDto<GetLossTypeColumnForViewDto>(
                totalCount,
                await lossTypeColumns.ToListAsync()
            );
        }

        public async Task<List<LossTypeColumnDto>> GetColumnsForLossType(LossEventTypeEnums input)
        {

            var filteredLossTypeColumns = _lossTypeColumnRepository.GetAll();//.Where(e => e.LossType == input);

            var lossTypeColumns = from o in filteredLossTypeColumns
                                  select new LossTypeColumnDto
                                  {
                                      ColumnName = o.ColumnName,
                                      DataType = o.DataType,
                                      Required = o.Required,
                                      //LossType = o.LossType,
                                      Minimum = o.Minimum,
                                      Maximum = o.Maximum,
                                      Id = o.Id
                                  };

            return await lossTypeColumns.ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_LossTypeColumns_Edit)]
        public async Task<GetLossTypeColumnForEditOutput> GetLossTypeColumnForEdit(EntityDto input)
        {
            var lossTypeColumn = await _lossTypeColumnRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLossTypeColumnForEditOutput { LossTypeColumn = ObjectMapper.Map<CreateOrEditLossTypeColumnDto>(lossTypeColumn) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLossTypeColumnDto input)
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

        [AbpAuthorize(AppPermissions.Pages_LossTypeColumns_Create)]
        protected virtual async Task Create(CreateOrEditLossTypeColumnDto input)
        {
            var lossTypeColumn = ObjectMapper.Map<LossTypeColumn>(input);


            if (AbpSession.TenantId != null)
            {
                lossTypeColumn.TenantId = (int?)AbpSession.TenantId;
            }


            await _lossTypeColumnRepository.InsertAsync(lossTypeColumn);
        }

        [AbpAuthorize(AppPermissions.Pages_LossTypeColumns_Edit)]
        protected virtual async Task Update(CreateOrEditLossTypeColumnDto input)
        {
            var lossTypeColumn = await _lossTypeColumnRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, lossTypeColumn);
        }

        [AbpAuthorize(AppPermissions.Pages_LossTypeColumns_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _lossTypeColumnRepository.DeleteAsync(input.Id);
        }
    }
}