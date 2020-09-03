
using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.BusinessObjectives.Dtos
{
    public class BusinessObjectiveDto : EntityDto
    {
		public string Name { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public string ObjectiveType { get; set; }


		 public long? UserId { get; set; }

		 
    }
}