﻿using Abp.Application.Services.Dto;

namespace ICMSDemo.TestingTemplates.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}