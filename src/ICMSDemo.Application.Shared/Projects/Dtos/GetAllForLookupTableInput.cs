﻿using Abp.Application.Services.Dto;

namespace ICMSDemo.Projects.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}