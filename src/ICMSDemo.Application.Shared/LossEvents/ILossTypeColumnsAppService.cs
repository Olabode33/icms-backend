using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.LossEvents.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.LossEvents
{
    public interface ILossTypeColumnsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLossTypeColumnForViewDto>> GetAll(GetAllLossTypeColumnsInput input);

		Task<GetLossTypeColumnForEditOutput> GetLossTypeColumnForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditLossTypeColumnDto input);

		Task Delete(EntityDto input);

		
    }
}