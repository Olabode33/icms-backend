﻿namespace ICMSDemo.ExceptionIncidents.Dtos
{
    public class GetExceptionIncidentForViewDto
    {
		public ExceptionIncidentDto ExceptionIncident { get; set; }

		public string ExceptionTypeName { get; set;}

		public string UserName { get; set;}

		public string TestingTemplateCode { get; set;}

		public string OrganizationUnitDisplayName { get; set;}


    }
}