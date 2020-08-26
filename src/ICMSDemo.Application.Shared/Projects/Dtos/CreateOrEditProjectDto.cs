
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using static ICMSDemo.IcmsEnums;

namespace ICMSDemo.Projects.Dtos
{
    public class CreateOrEditProjectDto : EntityDto<int?>
    {

		public string Code { get; set; }
		public bool Commenced { get; set; }
		
		
		public string Description { get; set; }
		
		
		public DateTime? StartDate { get; set; }

		public  ReviewType ReviewType { get; set; }

		public bool Cascade { get; set; }

		public DateTime? EndDate { get; set; }



		public  DateTime? ScopeStartDate { get; set; }
		public  DateTime? ScopeEndDate { get; set; }
		public DateTime BudgetedStartDate { get; set; }
		
		
		public DateTime BudgetedEndDate { get; set; }
		
		
		public string Title { get; set; }
		
		
		 public long? ControlUnitId { get; set; }
		 
		 		 public long? ScopeId { get; set; }


		public ProjectOwner? ProjectOwner { get; set; }
	}
}