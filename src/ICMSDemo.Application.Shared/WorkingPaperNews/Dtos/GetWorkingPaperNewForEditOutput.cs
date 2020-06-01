using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using ICMSDemo.TestingTemplates.Dtos;
using System.Collections.Generic;

namespace ICMSDemo.WorkingPaperNews.Dtos
{
    public class GetWorkingPaperNewForEditOutput
    {
		public CreateOrEditWorkingPaperNewDto WorkingPaperNew { get; set; }
		public GetTestingTemplateForViewDto TestingTemplate { get; set; }
		public List<CreateOrEditTestingAttributeDto> WorkingPaperDetails { get; set; }
		public int? LastSequence { get; set; }
		public string TestingTemplateCode { get; set;}

		public string OrganizationUnitDisplayName { get; set;}

		public string CompletedBy { get; set;}

		public string ReviewedBy { get; set;}
		public string AssignedTo { get; set;}


    }
}