using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.ExceptionIncidents.Dtos
{
    public class CreateOrEditExceptionIncidentDto : EntityDto<int?>
    {

		[Required]
		public string Code { get; set; }
		
		
		public string Description { get; set; }
		
		
		public Status Status { get; set; }
		
		
		public DateTime? ClosureDate { get; set; }
		
		
		public string ClosureComments { get; set; }
		
		
		public string RaisedByClosureComments { get; set; }
		
		
		 public int? ExceptionTypeId { get; set; }
		 
		 		 public long? RaisedById { get; set; }
		 
		 		 public int? TestingTemplateId { get; set; }
		 
		 		 public long? OrganizationUnitId { get; set; }
		 
		 
    }
}