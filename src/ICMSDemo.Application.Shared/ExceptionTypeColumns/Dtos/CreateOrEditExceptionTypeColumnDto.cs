using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.ExceptionTypeColumns.Dtos
{
    public class CreateOrEditExceptionTypeColumnDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		
		public DataTypes DataType { get; set; }
		
		
		public bool Required { get; set; }
		
		
		public decimal? Minimum { get; set; }
		
		
		public decimal? Maximum { get; set; }
		
		
		 public int ExceptionTypeId { get; set; }
		 
		 
    }
}