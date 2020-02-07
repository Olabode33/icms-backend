using ICMSDemo;

using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.Risks.Dtos
{
    public class RiskDto : EntityDto
    {
		public string Code { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public Severity Severity { get; set; }



    }
}