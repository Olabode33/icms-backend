using ICMSDemo;

using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.ProcessRiskControls.Dtos
{
    public class ProcessRiskControlDto : EntityDto
    {
		public string Code { get; set; }

		public string Notes { get; set; }

		public Frequency Frequency { get; set; }

		public bool Cascade { get; set; }


		 public int? ProcessRiskId { get; set; }

		 		 public long? ProcessId { get; set; }

		 		 public int? ControlId { get; set; }


		public double? Likelyhood { get; set; }

		public double? Impact { get; set; }
	}
}