using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using static ICMSDemo.IcmsEnums;

namespace ICMSDemo.LossEvents.Dtos
{
    public class LossTypeColumnDto : EntityDto
    {
		public string ColumnName { get; set; }

		public DataTypes DataType { get; set; }

		public bool Required { get; set; }

		public LossEventTypeEnums LossType { get; set; }

		public double? Minimum { get; set; }

		public double? Maximum { get; set; }



    }
}