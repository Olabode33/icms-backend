
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.DataLists.Dtos
{
    public class CreateOrEditDataListDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		
		public string Description { get; set; }
		
		

    }
}