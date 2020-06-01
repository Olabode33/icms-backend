
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.LibraryControls.Dtos
{
    public class CreateOrEditLibraryControlDto : EntityDto<int?>
    {

		public string Name { get; set; }
		
		
		public string Description { get; set; }
		
		
		public string Process { get; set; }
		
		
		public string SubProcess { get; set; }
		
		
		public string Risk { get; set; }
		
		
		public string ControlType { get; set; }
		
		
		public string ControlPoint { get; set; }
		
		
		public string Frequency { get; set; }
		
		
		public string InformationProcessingObjectives { get; set; }
		
		

    }
}