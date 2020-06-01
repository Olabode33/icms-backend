
using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.LibraryRisks.Dtos
{
    public class LibraryRiskDto : EntityDto
    {
		public string Name { get; set; }

		public string Process { get; set; }

		public string Description { get; set; }

		public string SubProcess { get; set; }



    }
}