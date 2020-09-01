
using ICMSDemo;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.LossEventTasks.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ICMSDemo.LossEvents;

namespace ICMSDemo.LossEventTasks
{
	[AbpAuthorize(AppPermissions.Pages_LossEventTasks)]
    public class LossEventTasksAppService : ICMSDemoAppServiceBase, ILossEventTasksAppService
    {
		 private readonly IRepository<LossEventTask> _lossEventTaskRepository;
		 private readonly IRepository<LossType> _lossTypeRepository;
		 

		  public LossEventTasksAppService(IRepository<LossEventTask> lossEventTaskRepository, IRepository<LossType> lossTypeRepository) 
		  {
			_lossEventTaskRepository = lossEventTaskRepository;
            _lossTypeRepository = lossTypeRepository;
		  }

		 public async Task<PagedResultDto<GetLossEventTaskForViewDto>> GetAll(GetAllLossEventTasksInput input)
         {
			
			var filteredLossEventTasks = _lossEventTaskRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Description.Contains(input.Filter));

			var pagedAndFilteredLossEventTasks = filteredLossEventTasks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var lossEventTasks = from o in pagedAndFilteredLossEventTasks
                                 join t in _lossTypeRepository.GetAll() on o.LossTypeId equals t.Id into t1
                                 from t2 in t1.DefaultIfEmpty()
                         select new GetLossEventTaskForViewDto() {
							LossEventTask = new LossEventTaskDto
							{
                                Title = o.Title,
                                Description = o.Description,
                                LossTypeId = o.LossTypeId,
                                LossTypeTriggerId = o.LossTypeTriggerId,
                                Status = o.Status,
                                AssignedTo = o.AssignedTo,
                                DateAssigned = o.DateAssigned,
                                Id = o.Id
							},
                            LossTypeName = t2 == null ? "" : t2.Name
						};

            var totalCount = await filteredLossEventTasks.CountAsync();

            return new PagedResultDto<GetLossEventTaskForViewDto>(
                totalCount,
                await lossEventTasks.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_LossEventTasks_Edit)]
		 public async Task<GetLossEventTaskForEditOutput> GetLossEventTaskForEdit(EntityDto input)
         {
            var lossEventTask = await _lossEventTaskRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetLossEventTaskForEditOutput {LossEventTask = ObjectMapper.Map<CreateOrEditLossEventTaskDto>(lossEventTask)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditLossEventTaskDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_LossEventTasks_Create)]
		 protected virtual async Task Create(CreateOrEditLossEventTaskDto input)
         {
            var lossEventTask = ObjectMapper.Map<LossEventTask>(input);

			
			if (AbpSession.TenantId != null)
			{
				lossEventTask.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _lossEventTaskRepository.InsertAsync(lossEventTask);
         }

		 [AbpAuthorize(AppPermissions.Pages_LossEventTasks_Edit)]
		 protected virtual async Task Update(CreateOrEditLossEventTaskDto input)
         {
            var lossEventTask = await _lossEventTaskRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, lossEventTask);
         }

		 [AbpAuthorize(AppPermissions.Pages_LossEventTasks_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _lossEventTaskRepository.DeleteAsync(input.Id);
         } 
    }
}