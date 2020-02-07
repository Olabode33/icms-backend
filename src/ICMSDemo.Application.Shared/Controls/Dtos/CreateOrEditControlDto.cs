using ICMSDemo;
using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.Controls.Dtos
{
    public class CreateOrEditControlDto : EntityDto<int?>
    {

		[Required]
		public string Code { get; set; }
		
		
		[Required]
		public string Name { get; set; }
		
		
		public string Description { get; set; }
		
		
		public ControlType ControlType { get; set; }
		
		
		public Frequency Frequency { get; set; }
		
		

    }
}