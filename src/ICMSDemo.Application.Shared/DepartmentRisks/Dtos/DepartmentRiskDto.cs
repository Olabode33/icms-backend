
using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.DepartmentRisks.Dtos
{
    public class DepartmentRiskDto : EntityDto
    {
		public string Code { get; set; }

		public string Comments { get; set; }


		 public int? DepartmentId { get; set; }

		 		 public int? RiskId { get; set; }
        public bool Cascade { get; set; }
        public bool Inherited { get; set; }
        public string DeptCode { get; set; }

        public int? Likelyhood { get; set; }

        public int? Impact { get; set; }
    }
}