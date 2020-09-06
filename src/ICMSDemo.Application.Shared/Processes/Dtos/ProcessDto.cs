
using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.Processes.Dtos
{
    public class ProcessDto : EntityDto<long>
    {
		public long? ParentId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Casade { get; set; }
		 public long? OwnerId { get; set; }

		 public long? DepartmentId { get; set; }

		 
    }
}