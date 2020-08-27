using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.LossEvents.Dtos
{
    public class GetAllLossTypeColumnsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }



    }
}