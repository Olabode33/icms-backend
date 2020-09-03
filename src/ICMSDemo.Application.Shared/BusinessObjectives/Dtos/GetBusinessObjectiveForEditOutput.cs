using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.BusinessObjectives.Dtos
{
    public class GetBusinessObjectiveForEditOutput
    {
		public CreateOrEditBusinessObjectiveDto BusinessObjective { get; set; }

		public string UserName { get; set;}


    }
}