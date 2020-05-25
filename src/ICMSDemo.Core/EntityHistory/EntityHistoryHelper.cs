using ICMSDemo.Projects;
using ICMSDemo.Processes;
using ICMSDemo.TestingTemplates;
using ICMSDemo.DepartmentRiskControls;
using ICMSDemo.Departments;
using ICMSDemo.ExceptionTypeColumns;
using ICMSDemo.Controls;
using ICMSDemo.Risks;
using System;
using System.Linq;
using Abp.Organizations;
using ICMSDemo.Authorization.Roles;
using ICMSDemo.MultiTenancy;

namespace ICMSDemo.EntityHistory
{
    public static class EntityHistoryHelper
    {
        public const string EntityHistoryConfigurationName = "EntityHistory";

        public static readonly Type[] HostSideTrackedTypes =
        {
            typeof(Process),
            typeof(OrganizationUnit), typeof(Role), typeof(Tenant)
        };

        public static readonly Type[] TenantSideTrackedTypes =
        {
            typeof(Project),
            typeof(Process),
            typeof(TestingTemplate),
            typeof(DepartmentRiskControl),
            typeof(Department),
            typeof(ExceptionTypeColumn),
            typeof(Control),
            typeof(Risk),
            typeof(OrganizationUnit), typeof(Role)
        };

        public static readonly Type[] TrackedTypes =
            HostSideTrackedTypes
                .Concat(TenantSideTrackedTypes)
                .GroupBy(type => type.FullName)
                .Select(types => types.First())
                .ToArray();
    }
}
