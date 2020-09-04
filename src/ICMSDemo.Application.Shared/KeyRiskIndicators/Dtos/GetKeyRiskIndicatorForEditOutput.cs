using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.KeyRiskIndicators.Dtos
{
    public class GetKeyRiskIndicatorForEditOutput
    {
		public CreateOrEditKeyRiskIndicatorDto KeyRiskIndicator { get; set; }

		public string ExceptionTypeCode { get; set;}

		public string UserName { get; set;}


    }
}