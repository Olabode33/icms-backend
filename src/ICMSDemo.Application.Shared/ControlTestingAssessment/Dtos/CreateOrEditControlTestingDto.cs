
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.ControlTestingAssessment.Dtos
{
    public class CreateOrEditControlTestingDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		public int? TestingTemplateId { get; set; }
		
		
		public string EndDate { get; set; }
		public virtual int? ProcessRiskControlId { get; set; }

		public int? OrganizationUnitId { get; set; }
		public virtual long? AssignedUserId { get; set; }
	}

}