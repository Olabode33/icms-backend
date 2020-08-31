using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.LossEventTasks.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.LossEventTasks
{
    public interface ILossEventTasksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLossEventTaskForViewDto>> GetAll(GetAllLossEventTasksInput input);

		Task<GetLossEventTaskForEditOutput> GetLossEventTaskForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditLossEventTaskDto input);

		Task Delete(EntityDto input);

		
    }
}