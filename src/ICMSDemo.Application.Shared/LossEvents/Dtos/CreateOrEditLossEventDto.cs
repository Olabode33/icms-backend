using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using static ICMSDemo.IcmsEnums;

namespace ICMSDemo.LossEvents.Dtos
{
    public class CreateOrEditLossEventDto : EntityDto<int?>
    {

		public double Amount { get; set; }
		
		
		public string Description { get; set; }
		
		
		public DateTime DateOccured { get; set; }
		
		
		public DateTime DateDiscovered { get; set; }
		
		
		public LossEventTypeEnums LossType { get; set; }
		
		
		public Status Status { get; set; }
		
		
		public LossCategoryEnums LossCategory { get; set; }

		public string ExtensionData { get; set; }

		public long? EmployeeUserId { get; set; }
		 
		 		 public long? DepartmentId { get; set; }
		 
		 
    }
}