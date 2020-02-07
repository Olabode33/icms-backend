using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.Risks.Dtos
{
    public class GetRiskForEditOutput
    {
		public CreateOrEditRiskDto Risk { get; set; }


    }
}