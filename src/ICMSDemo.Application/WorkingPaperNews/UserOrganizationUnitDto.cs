using ICMSDemo.Authorization.Users;
using ICMSDemo.Departments;

namespace ICMSDemo.WorkingPaperNews
{
    public class UserOrganizationUnitDto
    {
        public User User { get; set; }
        public UnitOrganizationRole UnitOrganizationRole { get; set; }
    }
}