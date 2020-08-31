using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.LossEventTasks.Dtos
{
    public class GetAllLossEventTasksInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }



    }
}