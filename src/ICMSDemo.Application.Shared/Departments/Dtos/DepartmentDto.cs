
using System;
using Abp.Application.Services.Dto;
using ICMSDemo.Organizations.Dto;

namespace ICMSDemo.Departments.Dtos
{
    public class DepartmentDto : OrganizationUnitDto
    {
		public string Code { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public bool IsAbstract { get; set; }

		public bool IsControlTeam { get; set; }


		 public long? SupervisorUserId { get; set; }

		 		 public long? ControlOfficerUserId { get; set; }

		 		 public long? ControlTeamId { get; set; }

		 
    }
}