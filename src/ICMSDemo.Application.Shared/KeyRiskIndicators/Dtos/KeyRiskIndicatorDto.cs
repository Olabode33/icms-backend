
using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.KeyRiskIndicators.Dtos
{
    public class KeyRiskIndicatorDto : EntityDto
    {
		public string Name { get; set; }

		public string Nature { get; set; }

		public decimal LowLevel { get; set; }


		 public int? ExceptionTypeId { get; set; }

		 		 public long? UserId { get; set; }

		public string DataInputMethod { get; set; }
		public int? RiskId { get; set; }
	}
}