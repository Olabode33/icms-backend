using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.LossEventTasks.Dtos
{
    public class CreateOrEditLossEventTaskDto : EntityDto<int?>
    {

		public string Title { get; set; }
		
		
		public string Description { get; set; }
		
		
		public int? LossTypeId { get; set; }
		
		
		public int? LossTypeTriggerId { get; set; }
		
		
		public Status Status { get; set; }
		
		
		public long? AssignedTo { get; set; }
		
		
		public DateTime DateAssigned { get; set; }
		
		

    }
}