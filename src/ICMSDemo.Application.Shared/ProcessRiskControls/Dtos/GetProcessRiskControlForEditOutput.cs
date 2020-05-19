using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.ProcessRiskControls.Dtos
{
    public class GetProcessRiskControlForEditOutput
    {
		public CreateOrEditProcessRiskControlDto ProcessRiskControl { get; set; }

		public string ProcessRiskCode { get; set;}

		public string OrganizationUnitDisplayName { get; set;}

		public string ControlName { get; set;}


    }
}