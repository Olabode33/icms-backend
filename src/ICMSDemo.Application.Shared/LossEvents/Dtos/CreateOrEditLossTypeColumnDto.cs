using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using static ICMSDemo.IcmsEnums;

namespace ICMSDemo.LossEvents.Dtos
{
    public class CreateOrEditLossTypeColumnDto : EntityDto<int?>
    {

		public string ColumnName { get; set; }
		
		
		public DataTypes DataType { get; set; }
		
		
		public bool Required { get; set; }
		
		
		public LossEventTypeEnums LossType { get; set; }
		
		
		public double? Minimum { get; set; }
		
		
		public double? Maximum { get; set; }
		
		

    }
}