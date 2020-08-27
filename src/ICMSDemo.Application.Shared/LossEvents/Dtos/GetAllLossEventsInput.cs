using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.LossEvents.Dtos
{
    public class GetAllLossEventsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public double? MaxAmountFilter { get; set; }
		public double? MinAmountFilter { get; set; }

		public int? LossTypeFilter { get; set; }

		public int? StatusFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string OrganizationUnitDisplayNameFilter { get; set; }

		 
    }
}