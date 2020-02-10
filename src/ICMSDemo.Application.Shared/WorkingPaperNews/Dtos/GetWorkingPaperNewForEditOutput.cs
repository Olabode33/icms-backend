using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.WorkingPaperNews.Dtos
{
    public class GetWorkingPaperNewForEditOutput
    {
		public CreateOrEditWorkingPaperNewDto WorkingPaperNew { get; set; }

		public string TestingTemplateCode { get; set;}

		public string OrganizationUnitDisplayName { get; set;}

		public string UserName { get; set;}

		public string UserName2 { get; set;}


    }
}