﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICMSDemo.ExceptionIncidents.Dtos
{
    public class GetExceptionIncidentForViewDto
    {
		public ExceptionIncidentDto ExceptionIncident { get; set; }

        [NotMapped]
        public List<ExceptionIncidentAttachment> ExceptionIncidentAttachment { get; set; }

        public string ExceptionTypeName { get; set;}

		public string UserName { get; set;}

		public string WorkingPaperCode { get; set;}

		public string OrganizationUnitDisplayName { get; set;}
        public string DeptCode { get; set; }
    }
}