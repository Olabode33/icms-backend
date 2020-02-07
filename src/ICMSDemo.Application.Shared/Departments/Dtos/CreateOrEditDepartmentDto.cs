
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.Departments.Dtos
{
    public class CreateOrEditDepartmentDto : EntityDto<int?>
    {

		public string Code { get; set; }
		
		
		public string Name { get; set; }
		
		
		public string Description { get; set; }
		
		
		public bool IsAbstract { get; set; }
		
		
		public bool IsControlTeam { get; set; }
		
		
		 public long? SupervisorUserId { get; set; }
		 
		 		 public long? ControlOfficerUserId { get; set; }
		 
		 		 public long? ControlTeamId { get; set; }
		 		
		
		public long? SupervisingUnitId { get; set; }



		 
		 
    }
}