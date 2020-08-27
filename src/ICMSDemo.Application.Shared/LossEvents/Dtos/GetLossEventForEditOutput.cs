using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.LossEvents.Dtos
{
    public class GetLossEventForEditOutput
    {
		public CreateOrEditLossEventDto LossEvent { get; set; }

		public string UserName { get; set;}

		public string OrganizationUnitDisplayName { get; set;}


    }
}