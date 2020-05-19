using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ICMSDemo.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var processRiskControls = pages.CreateChildPermission(AppPermissions.Pages_ProcessRiskControls, L("ProcessRiskControls"));
            processRiskControls.CreateChildPermission(AppPermissions.Pages_ProcessRiskControls_Create, L("CreateNewProcessRiskControl"));
            processRiskControls.CreateChildPermission(AppPermissions.Pages_ProcessRiskControls_Edit, L("EditProcessRiskControl"));
            processRiskControls.CreateChildPermission(AppPermissions.Pages_ProcessRiskControls_Delete, L("DeleteProcessRiskControl"));



            var processRisks = pages.CreateChildPermission(AppPermissions.Pages_ProcessRisks, L("ProcessRisks"));
            processRisks.CreateChildPermission(AppPermissions.Pages_ProcessRisks_Create, L("CreateNewProcessRisk"));
            processRisks.CreateChildPermission(AppPermissions.Pages_ProcessRisks_Edit, L("EditProcessRisk"));
            processRisks.CreateChildPermission(AppPermissions.Pages_ProcessRisks_Delete, L("DeleteProcessRisk"));



            var processes = pages.CreateChildPermission(AppPermissions.Pages_Processes, L("Processes"));
            processes.CreateChildPermission(AppPermissions.Pages_Processes_Create, L("CreateNewProcess"));
            processes.CreateChildPermission(AppPermissions.Pages_Processes_Edit, L("EditProcess"));
            processes.CreateChildPermission(AppPermissions.Pages_Processes_Delete, L("DeleteProcess"));



            var workingPaperNews = pages.CreateChildPermission(AppPermissions.Pages_WorkingPaperNews, L("WorkingPaperNews"), multiTenancySides: MultiTenancySides.Tenant);
            workingPaperNews.CreateChildPermission(AppPermissions.Pages_WorkingPaperNews_Create, L("CreateNewWorkingPaperNew"), multiTenancySides: MultiTenancySides.Tenant);
            workingPaperNews.CreateChildPermission(AppPermissions.Pages_WorkingPaperNews_Edit, L("EditWorkingPaperNew"), multiTenancySides: MultiTenancySides.Tenant);
            workingPaperNews.CreateChildPermission(AppPermissions.Pages_WorkingPaperNews_Delete, L("DeleteWorkingPaperNew"), multiTenancySides: MultiTenancySides.Tenant);



            var homePage = pages.CreateChildPermission(AppPermissions.HomePage, L("HomePage"), multiTenancySides: MultiTenancySides.Tenant );


            var exceptionIncidents = pages.CreateChildPermission(AppPermissions.Pages_ExceptionIncidents, L("ExceptionIncidents"), multiTenancySides: MultiTenancySides.Tenant);
            exceptionIncidents.CreateChildPermission(AppPermissions.Pages_ExceptionIncidents_Create, L("CreateNewExceptionIncident"), multiTenancySides: MultiTenancySides.Tenant);
            exceptionIncidents.CreateChildPermission(AppPermissions.Pages_ExceptionIncidents_Edit, L("EditExceptionIncident"), multiTenancySides: MultiTenancySides.Tenant);
            exceptionIncidents.CreateChildPermission(AppPermissions.Pages_ExceptionIncidents_Delete, L("DeleteExceptionIncident"), multiTenancySides: MultiTenancySides.Tenant);



            var testingTemplates = pages.CreateChildPermission(AppPermissions.Pages_TestingTemplates, L("TestingTemplates"), multiTenancySides: MultiTenancySides.Tenant);
            testingTemplates.CreateChildPermission(AppPermissions.Pages_TestingTemplates_Create, L("CreateNewTestingTemplate"), multiTenancySides: MultiTenancySides.Tenant);
            testingTemplates.CreateChildPermission(AppPermissions.Pages_TestingTemplates_Edit, L("EditTestingTemplate"), multiTenancySides: MultiTenancySides.Tenant);
            testingTemplates.CreateChildPermission(AppPermissions.Pages_TestingTemplates_Delete, L("DeleteTestingTemplate"), multiTenancySides: MultiTenancySides.Tenant);



            var departmentRiskControls = pages.CreateChildPermission(AppPermissions.Pages_DepartmentRiskControls, L("DepartmentRiskControls"), multiTenancySides: MultiTenancySides.Tenant);
            departmentRiskControls.CreateChildPermission(AppPermissions.Pages_DepartmentRiskControls_Create, L("CreateNewDepartmentRiskControl"), multiTenancySides: MultiTenancySides.Tenant);
            departmentRiskControls.CreateChildPermission(AppPermissions.Pages_DepartmentRiskControls_Edit, L("EditDepartmentRiskControl"), multiTenancySides: MultiTenancySides.Tenant);
            departmentRiskControls.CreateChildPermission(AppPermissions.Pages_DepartmentRiskControls_Delete, L("DeleteDepartmentRiskControl"), multiTenancySides: MultiTenancySides.Tenant);



            var departmentRisks = pages.CreateChildPermission(AppPermissions.Pages_DepartmentRisks, L("DepartmentRisks"), multiTenancySides: MultiTenancySides.Tenant);
            departmentRisks.CreateChildPermission(AppPermissions.Pages_DepartmentRisks_Create, L("CreateNewDepartmentRisk"), multiTenancySides: MultiTenancySides.Tenant);
            departmentRisks.CreateChildPermission(AppPermissions.Pages_DepartmentRisks_Edit, L("EditDepartmentRisk"), multiTenancySides: MultiTenancySides.Tenant);
            departmentRisks.CreateChildPermission(AppPermissions.Pages_DepartmentRisks_Delete, L("DeleteDepartmentRisk"), multiTenancySides: MultiTenancySides.Tenant);



            var departments = pages.CreateChildPermission(AppPermissions.Pages_Departments, L("Departments"), multiTenancySides: MultiTenancySides.Tenant);
            departments.CreateChildPermission(AppPermissions.Pages_Departments_Create, L("CreateNewDepartment"), multiTenancySides: MultiTenancySides.Tenant);
            departments.CreateChildPermission(AppPermissions.Pages_Departments_Edit, L("EditDepartment"), multiTenancySides: MultiTenancySides.Tenant);
            departments.CreateChildPermission(AppPermissions.Pages_Departments_Delete, L("DeleteDepartment"), multiTenancySides: MultiTenancySides.Tenant);



            var dataLists = pages.CreateChildPermission(AppPermissions.Pages_DataLists, L("DataLists"), multiTenancySides: MultiTenancySides.Tenant);
            dataLists.CreateChildPermission(AppPermissions.Pages_DataLists_Create, L("CreateNewDataList"), multiTenancySides: MultiTenancySides.Tenant);
            dataLists.CreateChildPermission(AppPermissions.Pages_DataLists_Edit, L("EditDataList"), multiTenancySides: MultiTenancySides.Tenant);
            dataLists.CreateChildPermission(AppPermissions.Pages_DataLists_Delete, L("DeleteDataList"), multiTenancySides: MultiTenancySides.Tenant);



            var exceptionTypeColumns = pages.CreateChildPermission(AppPermissions.Pages_ExceptionTypeColumns, L("ExceptionTypeColumns"), multiTenancySides: MultiTenancySides.Tenant);
            exceptionTypeColumns.CreateChildPermission(AppPermissions.Pages_ExceptionTypeColumns_Create, L("CreateNewExceptionTypeColumn"), multiTenancySides: MultiTenancySides.Tenant);
            exceptionTypeColumns.CreateChildPermission(AppPermissions.Pages_ExceptionTypeColumns_Edit, L("EditExceptionTypeColumn"), multiTenancySides: MultiTenancySides.Tenant);
            exceptionTypeColumns.CreateChildPermission(AppPermissions.Pages_ExceptionTypeColumns_Delete, L("DeleteExceptionTypeColumn"), multiTenancySides: MultiTenancySides.Tenant);



            var exceptionTypes = pages.CreateChildPermission(AppPermissions.Pages_ExceptionTypes, L("ExceptionTypes"), multiTenancySides: MultiTenancySides.Tenant);
            exceptionTypes.CreateChildPermission(AppPermissions.Pages_ExceptionTypes_Create, L("CreateNewExceptionType"), multiTenancySides: MultiTenancySides.Tenant);
            exceptionTypes.CreateChildPermission(AppPermissions.Pages_ExceptionTypes_Edit, L("EditExceptionType"), multiTenancySides: MultiTenancySides.Tenant);
            exceptionTypes.CreateChildPermission(AppPermissions.Pages_ExceptionTypes_Delete, L("DeleteExceptionType"), multiTenancySides: MultiTenancySides.Tenant);



            var controls = pages.CreateChildPermission(AppPermissions.Pages_Controls, L("Controls"), multiTenancySides: MultiTenancySides.Tenant);
            controls.CreateChildPermission(AppPermissions.Pages_Controls_Create, L("CreateNewControl"), multiTenancySides: MultiTenancySides.Tenant);
            controls.CreateChildPermission(AppPermissions.Pages_Controls_Edit, L("EditControl"), multiTenancySides: MultiTenancySides.Tenant);
            controls.CreateChildPermission(AppPermissions.Pages_Controls_Delete, L("DeleteControl"), multiTenancySides: MultiTenancySides.Tenant);



            var risks = pages.CreateChildPermission(AppPermissions.Pages_Risks, L("Risks"), multiTenancySides: MultiTenancySides.Tenant);
            risks.CreateChildPermission(AppPermissions.Pages_Risks_Create, L("CreateNewRisk"), multiTenancySides: MultiTenancySides.Tenant);
            risks.CreateChildPermission(AppPermissions.Pages_Risks_Edit, L("EditRisk"), multiTenancySides: MultiTenancySides.Tenant);
            risks.CreateChildPermission(AppPermissions.Pages_Risks_Delete, L("DeleteRisk"), multiTenancySides: MultiTenancySides.Tenant);


            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host); 

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ICMSDemoConsts.LocalizationSourceName);
        }
    }
}
