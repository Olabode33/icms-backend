﻿using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.KeyRiskIndicators.Dtos
{
    public class GetAllKeyRiskIndicatorsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NatureFilter { get; set; }


		 public string ExceptionTypeCodeFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}