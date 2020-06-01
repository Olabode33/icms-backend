using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICMSDemo.ExceptionIncidents.Dtos
{
    public class CreateOrEditExceptionIncidentDto : EntityDto<int?>
    {
        public string Description { get; set; }

        public int? ExceptionTypeId { get; set; }

        public long? CausedById { get; set; }

        public Guid? WorkingPaperId { get; set; }

        public long? OrganizationUnitId { get; set; }

        public CreateOrEditExceptionIncidentColumnDto[] IncidentColumns { set; get; }

        public List<ExceptionIncidentAttachment> ExceptionIncidentAttachment { get; set; }

        public virtual Status Status { get; set; }

        public virtual DateTime? ClosureDate { get; set; }

        public virtual string ClosureComments { get; set; }

        public virtual DateTime? ResolutionDate { get; set; }

        public virtual string ResolutionComments { get; set; }
        

    }



    public class CreateOrEditExceptionIncidentColumnDto : EntityDto
    {
        public virtual int? ExceptionIncidentId { get; set; }

        public virtual int? ExceptionTypeColumnId { get; set; }

        public virtual string Value { get; set; }

        public string Name { set; get; }
    }


    public class GetExceptionTypeColumnsForEdit
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public decimal? Maximum { get; set; }
        public string DataFieldType { get; set; }
        public decimal? Minimum { get; set; }
        public bool Required { get; set; }
    }
}