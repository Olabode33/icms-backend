using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.Controls.Dtos
{
    public class GetControlForEditOutput
    {
		public CreateOrEditControlDto Control { get; set; }

        public string ControlOwnerName { get; set; }

    }
}