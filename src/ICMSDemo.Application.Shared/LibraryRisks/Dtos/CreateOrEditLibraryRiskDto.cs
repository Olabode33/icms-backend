
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.LibraryRisks.Dtos
{
    public class CreateOrEditLibraryRiskDto : EntityDto<int?>
    {

		[Required]
		public string Name { get; set; }
		
		
		public string Process { get; set; }
		
		
		public string Description { get; set; }
		
		
		public string SubProcess { get; set; }
		
		

    }
}