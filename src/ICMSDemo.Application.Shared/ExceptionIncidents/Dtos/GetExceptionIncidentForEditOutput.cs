using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ICMSDemo.ExceptionIncidents.Dtos
{
    public class GetExceptionIncidentForEditOutput
    {
		public CreateOrEditExceptionIncidentDto ExceptionIncident { get; set; }
        
        public string ExceptionTypeName { get; set;}

		public string UserName { get; set;}

		public string WorkingPaperCode { get; set;}

		public string OrganizationUnitDisplayName { get; set;}


    }
}