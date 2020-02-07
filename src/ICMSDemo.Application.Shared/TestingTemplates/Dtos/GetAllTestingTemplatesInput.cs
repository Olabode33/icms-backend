﻿using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.TestingTemplates.Dtos
{
    public class GetAllTestingTemplatesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string TitleFilter { get; set; }

		public int FrequencyFilter { get; set; }


		 public string DepartmentRiskControlCodeFilter { get; set; }

		 
    }
}