using ICMSDemo;
using ICMSDemo;

using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.Controls.Dtos
{
    public class ControlDto : EntityDto
    {
		public string Code { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public ControlType ControlType { get; set; }

		public Frequency Frequency { get; set; }

		public string ControlPoint { get; set; }
		public string ControlObjective { get; set; }
		public long? ControlOwnerId { get; set; }

	}
}