using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.Processes.Dtos
{
    public class GetProcessForEditOutput
    {
		public CreateOrEditProcessDto Process { get; set; }

		public string UserName { get; set;}

		public string OrganizationUnitDisplayName { get; set;}


    }
}