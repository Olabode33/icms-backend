using ICMSDemo;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

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

    }



    public class CreateOrEditExceptionIncidentColumnDto : EntityDto
    {
        public virtual int? ExceptionIncidentId { get; set; }

        public virtual int? ExceptionTypeColumnId { get; set; }

        public virtual string Value { get; set; }
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