
using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.ProcessRisks.Dtos
{
    public class ProcessRiskDto : EntityDto
    {
		public string Code { get; set; }

		public string Comments { get; set; }

		public bool Cascade { get; set; }


		 public long ProcessId { get; set; }

		 		 public int RiskId { get; set; }


		public int? Likelyhood { get; set; }

		public int? Impact { get; set; }
	}
}