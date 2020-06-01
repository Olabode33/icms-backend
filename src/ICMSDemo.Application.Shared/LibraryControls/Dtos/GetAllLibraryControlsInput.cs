using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.LibraryControls.Dtos
{
    public class GetAllLibraryControlsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }



    }
}