using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.LibraryRisks.Dtos
{
    public class GetLibraryRiskForEditOutput
    {
		public CreateOrEditLibraryRiskDto LibraryRisk { get; set; }


    }
}