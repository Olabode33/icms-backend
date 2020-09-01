using ICMSDemo.WorkingPaperReviewComments.Dtos;
using ICMSDemo.WorkingPaperReviewComments;
using ICMSDemo.LossEventTasks.Dtos;
using ICMSDemo.LossEventTasks;
using ICMSDemo.LossEvents.Dtos;
using ICMSDemo.LossEvents;
using ICMSDemo.LibraryControls.Dtos;
using ICMSDemo.LibraryControls;
using ICMSDemo.LibraryRisks.Dtos;
using ICMSDemo.LibraryRisks;
using ICMSDemo.DepartmentRatingHistory.Dtos;
using ICMSDemo.DepartmentRatingHistory;
using ICMSDemo.Ratings.Dtos;
using ICMSDemo.Ratings;
using ICMSDemo.Projects.Dtos;
using ICMSDemo.Projects;
using ICMSDemo.ProcessRiskControls.Dtos;
using ICMSDemo.ProcessRiskControls;
using ICMSDemo.ProcessRisks.Dtos;
using ICMSDemo.ProcessRisks;
using ICMSDemo.Processes.Dtos;
using ICMSDemo.Processes;
using ICMSDemo.WorkingPaperNews.Dtos;
using ICMSDemo.WorkingPaperNews;

using ICMSDemo.ExceptionIncidents.Dtos;
using ICMSDemo.ExceptionIncidents;
using ICMSDemo.TestingTemplates.Dtos;
using ICMSDemo.TestingTemplates;
using ICMSDemo.DepartmentRiskControls.Dtos;
using ICMSDemo.DepartmentRiskControls;
using ICMSDemo.DepartmentRisks.Dtos;
using ICMSDemo.DepartmentRisks;
using ICMSDemo.Departments.Dtos;
using ICMSDemo.Departments;
using ICMSDemo.DataLists.Dtos;
using ICMSDemo.DataLists;
using ICMSDemo.ExceptionTypeColumns.Dtos;
using ICMSDemo.ExceptionTypeColumns;
using ICMSDemo.ExceptionTypes.Dtos;
using ICMSDemo.ExceptionTypes;
using ICMSDemo.Controls.Dtos;
using ICMSDemo.Controls;
using ICMSDemo.Risks.Dtos;
using ICMSDemo.Risks;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using AutoMapper;
using ICMSDemo.Auditing.Dto;
using ICMSDemo.Authorization.Accounts.Dto;
using ICMSDemo.Authorization.Permissions.Dto;
using ICMSDemo.Authorization.Roles;
using ICMSDemo.Authorization.Roles.Dto;
using ICMSDemo.Authorization.Users;
using ICMSDemo.Authorization.Users.Dto;
using ICMSDemo.Authorization.Users.Importing.Dto;
using ICMSDemo.Authorization.Users.Profile.Dto;
using ICMSDemo.Chat;
using ICMSDemo.Chat.Dto;
using ICMSDemo.Editions;
using ICMSDemo.Editions.Dto;
using ICMSDemo.Friendships;
using ICMSDemo.Friendships.Cache;
using ICMSDemo.Friendships.Dto;
using ICMSDemo.Localization.Dto;
using ICMSDemo.MultiTenancy;
using ICMSDemo.MultiTenancy.Dto;
using ICMSDemo.MultiTenancy.HostDashboard.Dto;
using ICMSDemo.MultiTenancy.Payments;
using ICMSDemo.MultiTenancy.Payments.Dto;
using ICMSDemo.Notifications.Dto;
using ICMSDemo.Organizations.Dto;
using ICMSDemo.Sessions.Dto;
using ICMSDemo.WorkingPapers;

namespace ICMSDemo
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditWorkingPaperReviewCommentDto, WorkingPaperReviewComment>().ReverseMap();
            configuration.CreateMap<WorkingPaperReviewCommentDto, WorkingPaperReviewComment>().ReverseMap();
            configuration.CreateMap<CreateOrEditLossEventTaskDto, LossEventTask>().ReverseMap();
            configuration.CreateMap<LossEventTaskDto, LossEventTask>().ReverseMap();
            configuration.CreateMap<LossTypeDto, LossType>().ReverseMap();
            configuration.CreateMap<LossTypeTriggerDto, LossTypeTrigger>().ReverseMap();
            configuration.CreateMap<CreateOrEditLossTypeColumnDto, LossTypeColumn>().ReverseMap();
            configuration.CreateMap<LossTypeColumnDto, LossTypeColumn>().ReverseMap();
            configuration.CreateMap<CreateOrEditLossEventDto, LossEvent>().ReverseMap();
            configuration.CreateMap<LossEventDto, LossEvent>().ReverseMap();
            configuration.CreateMap<CreateOrEditLibraryControlDto, LibraryControl>().ReverseMap();
            configuration.CreateMap<LibraryControlDto, LibraryControl>().ReverseMap();
            configuration.CreateMap<CreateOrEditLibraryRiskDto, LibraryRisk>().ReverseMap();
            configuration.CreateMap<LibraryRiskDto, LibraryRisk>().ReverseMap();
            configuration.CreateMap<CreateOrEditDepartmentRatingDto, DepartmentRating>().ReverseMap();
            configuration.CreateMap<DepartmentRatingDto, DepartmentRating>().ReverseMap();
            configuration.CreateMap<CreateOrEditRatingDto, Rating>().ReverseMap();
            configuration.CreateMap<RatingDto, Rating>().ReverseMap();
            configuration.CreateMap<CreateOrEditProjectDto, Project>().ReverseMap();
            configuration.CreateMap<ProjectDto, Project>().ReverseMap();

            configuration.CreateMap<CreateOrEditWorkingPaperNewDto, WorkingPaper>().ReverseMap()
                         .ForMember(x => x.Attributes, opt => opt.Ignore());

            configuration.CreateMap<CreateOrEditProcessRiskControlDto, ProcessRiskControl>().ReverseMap();
            configuration.CreateMap<ProcessRiskControlDto, ProcessRiskControl>().ReverseMap();
            configuration.CreateMap<CreateOrEditProcessRiskDto, ProcessRisk>().ReverseMap();
            configuration.CreateMap<ProcessRiskDto, ProcessRisk>().ReverseMap();
            configuration.CreateMap<CreateOrEditProcessDto, Process>().ReverseMap();
            configuration.CreateMap<ProcessDto, Process>().ReverseMap();
   
            configuration.CreateMap<CreateOrEditExceptionIncidentDto, ExceptionIncident>()
                         .ForMember(e => e.WorkingPaperFkId, opt => opt.MapFrom(dto => dto.WorkingPaperId))
                         .ReverseMap();
            configuration.CreateMap<ExceptionIncidentDto, ExceptionIncident>()
                         .ForMember(e => e.WorkingPaperFkId, opt => opt.MapFrom(dto => dto.WorkingPaperId))
                         .ReverseMap();
            configuration.CreateMap<CreateOrEditTestingTemplateDto, TestingTemplate>()
                         .ForMember(e => e.ProcessRiskControlId, opt => opt.MapFrom(dto => dto.DepartmentRiskControlId))
                         .ReverseMap();
            configuration.CreateMap<TestingTemplateDto, TestingTemplate>()
                         .ForMember(e => e.ProcessRiskControlId, opt => opt.MapFrom(dto => dto.DepartmentRiskControlId))
                         .ReverseMap();
            configuration.CreateMap<CreateOrEditDepartmentRiskControlDto, DepartmentRiskControl>().ReverseMap();
            configuration.CreateMap<DepartmentRiskControlDto, DepartmentRiskControl>().ReverseMap();
            configuration.CreateMap<CreateOrEditDepartmentRiskDto, DepartmentRisk>().ReverseMap();
            configuration.CreateMap<DepartmentRiskDto, DepartmentRisk>().ReverseMap();
            configuration.CreateMap<CreateOrEditDepartmentDto, Department>().ReverseMap();
            configuration.CreateMap<DepartmentDto, Department>().ReverseMap();
            configuration.CreateMap<CreateOrEditDataListDto, DataList>().ReverseMap();
            configuration.CreateMap<DataListDto, DataList>().ReverseMap();
            configuration.CreateMap<CreateOrEditExceptionTypeColumnDto, ExceptionTypeColumn>().ReverseMap();
            configuration.CreateMap<ExceptionTypeColumnDto, ExceptionTypeColumn>().ReverseMap();
            configuration.CreateMap<CreateOrEditExceptionTypeDto, ExceptionType>().ReverseMap();
            configuration.CreateMap<ExceptionTypeDto, ExceptionType>().ReverseMap();
            configuration.CreateMap<CreateOrEditControlDto, Control>().ReverseMap();
            configuration.CreateMap<ControlDto, Control>().ReverseMap();
            configuration.CreateMap<CreateOrEditRiskDto, Risk>().ReverseMap();
            configuration.CreateMap<RiskDto, Risk>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}
