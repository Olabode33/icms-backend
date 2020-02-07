using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.ExceptionTypeColumns.Dtos
{
    public class GetAllExceptionTypeColumnsForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public int DataTypeFilter { get; set; }

		public int RequiredFilter { get; set; }

		public decimal? MaxMinimumFilter { get; set; }
		public decimal? MinMinimumFilter { get; set; }

		public decimal? MaxMaximumFilter { get; set; }
		public decimal? MinMaximumFilter { get; set; }


		 public string ExceptionTypeNameFilter { get; set; }

		 
    }
}