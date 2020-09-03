
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.BusinessObjectives.Dtos
{
    public class CreateOrEditBusinessObjectiveDto : EntityDto<int?>
    {

		[Required]
		public string Name { get; set; }
		
		
		public string Description { get; set; }
		
		
		public DateTime StartDate { get; set; }
		
		
		public DateTime EndDate { get; set; }
		
		
		public string ObjectiveType { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 
    }
}