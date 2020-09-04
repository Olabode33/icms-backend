using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.ControlTestingAssessment.Dtos
{
    public class GetAllControlTestingsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public int? MaxTestingTemplateIdFilter { get; set; }
		public int? MinTestingTemplateIdFilter { get; set; }

		public DateTime? EndDateFilter { get; set; }



    }
}