
using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.ControlTestingAssessment.Dtos
{
    public class ControlTestingDto : EntityDto
    {
		public string Name { get; set; }

		public int? TestingTemplateId { get; set; }

		public DateTime? EndDate { get; set; }



    }
}