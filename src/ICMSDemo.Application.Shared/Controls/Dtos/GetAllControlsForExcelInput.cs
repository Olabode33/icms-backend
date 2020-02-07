using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.Controls.Dtos
{
    public class GetAllControlsForExcelInput
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }

		public int ControlTypeFilter { get; set; }

		public int FrequencyFilter { get; set; }



    }
}