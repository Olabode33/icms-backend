using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.Departments.Dtos
{
    public class GetAllDepartmentsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public int IsControlTeamFilter { get; set; }


		 public string UserNameFilter { get; set; }

		 		 public string UserName2Filter { get; set; }

		 		 public string OrganizationUnitDisplayNameFilter { get; set; }

		 
    }

	public class GetAllDepartmentsForRatingInput : PagedAndSortedResultRequestDto
	{
		public string Filter { get; set; }
		public int RatingFilter { get; set; }
	}
	}