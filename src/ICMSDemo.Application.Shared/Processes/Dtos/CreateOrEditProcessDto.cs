
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.Processes.Dtos
{
    public class CreateOrEditProcessDto : EntityDto<long?>
    {
        public long? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Casade { get; set; }
        public long? OwnerId { get; set; }
        public long? DepartmentId { get; set; }
    }
}