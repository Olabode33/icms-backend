using ICMSDemo.DepartmentRatingHistory.Dtos;

namespace ICMSDemo.Departments.Dtos
{
    public class GetDepartmentForViewDto
    {
		public DepartmentDto Department { get; set; }

		public string UserName { get; set;}

		public string UserName2 { get; set;}

		public string OrganizationUnitDisplayName { get; set;}
        public string SupervsingUnitDisplaName { get; set; }
        public string RatingName { get; set; }
        public string RatingCode { get; set; }
        public DepartmentRatingDto[] RatingHistory { get; set; }
    }
}