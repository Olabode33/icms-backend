using ICMSDemo;

using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.ExceptionTypeColumns.Dtos
{
    public class ExceptionTypeColumnDto : EntityDto
    {
		public string Name { get; set; }

		public DataTypes DataType { get; set; }

		public bool Required { get; set; }

		public decimal? Minimum { get; set; }

		public decimal? Maximum { get; set; }


		 public int ExceptionTypeId { get; set; }

		 
    }
}