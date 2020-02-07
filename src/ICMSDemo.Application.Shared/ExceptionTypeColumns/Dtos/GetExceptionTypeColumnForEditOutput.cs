using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.ExceptionTypeColumns.Dtos
{
    public class GetExceptionTypeColumnForEditOutput
    {
		public CreateOrEditExceptionTypeColumnDto ExceptionTypeColumn { get; set; }

		public string ExceptionTypeName { get; set;}


    }
}