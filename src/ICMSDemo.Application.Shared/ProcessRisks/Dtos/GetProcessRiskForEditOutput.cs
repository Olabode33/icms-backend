using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.ProcessRisks.Dtos
{
    public class GetProcessRiskForEditOutput
    {
		public CreateOrEditProcessRiskDto ProcessRisk { get; set; }

		public string OrganizationUnitDisplayName { get; set;}

		public string RiskName { get; set;}


    }
}