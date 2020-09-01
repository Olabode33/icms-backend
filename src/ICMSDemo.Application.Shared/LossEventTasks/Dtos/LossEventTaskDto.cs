using ICMSDemo;

using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.LossEventTasks.Dtos
{
    public class LossEventTaskDto : EntityDto
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