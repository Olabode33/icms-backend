using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.DataLists.Dtos
{
    public class GetAllDataListsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }



    }
}