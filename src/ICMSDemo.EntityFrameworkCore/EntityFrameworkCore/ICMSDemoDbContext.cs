using ICMSDemo.LossEventTasks;
using ICMSDemo.LossEvents;
using ICMSDemo.LibraryControls;
using ICMSDemo.LibraryRisks;
using ICMSDemo.DepartmentRatingHistory;
using ICMSDemo.Ratings;
using ICMSDemo.Projects;
using ICMSDemo.ProcessRiskControls;
using ICMSDemo.ProcessRisks;
using ICMSDemo.Processes;
using ICMSDemo.WorkingPaperNews;
using ICMSDemo.ExceptionIncidents;
using ICMSDemo.TestingTemplates;
using ICMSDemo.DepartmentRiskControls;
using ICMSDemo.DepartmentRisks;
using ICMSDemo.Departments;
using ICMSDemo.DataLists;
using ICMSDemo.ExceptionTypeColumns;
using ICMSDemo.ExceptionTypes;
using ICMSDemo.Controls;
using ICMSDemo.Risks;
using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ICMSDemo.Authorization.Roles;
using ICMSDemo.Authorization.Users;
using ICMSDemo.Chat;
using ICMSDemo.Editions;
using ICMSDemo.Friendships;
using ICMSDemo.MultiTenancy;
using ICMSDemo.MultiTenancy.Accounting;
using ICMSDemo.MultiTenancy.Payments;
using ICMSDemo.Storage;
using ICMSDemo.WorkingPapers;

namespace ICMSDemo.EntityFrameworkCore
{
    public class ICMSDemoDbContext : AbpZeroDbContext<Tenant, Role, User, ICMSDemoDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<LossEventTask> LossEventTasks { get; set; }

        public virtual DbSet<LossType> LossType { get; set; }
        public virtual DbSet<LossTypeTrigger> LossTypeTrigger { get; set; }
        public virtual DbSet<LossTypeColumn> LossTypeColumns { get; set; }

        public virtual DbSet<LossEvent> LossEvents { get; set; }

        public virtual DbSet<LibraryControl> LibraryControls { get; set; }

        public virtual DbSet<LibraryRisk> LibraryRisks { get; set; }

        public virtual DbSet<DepartmentRating> DepartmentRatingHistory { get; set; }

        public virtual DbSet<Rating> Ratings { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<ProcessRiskControl> ProcessRiskControls { get; set; }

        public virtual DbSet<ProcessRisk> ProcessRisks { get; set; }

        public virtual DbSet<Process> Processes { get; set; }

        public virtual DbSet<ExceptionTypeEscalation> ExceptionTypeEscalations { get; set; }
        public virtual DbSet<ExceptionIncidentColumn> ExceptionIncidentColumns { get; set; }
        public virtual DbSet<TestingAttrribute> TestingAttrributesList { get; set; }
        public virtual DbSet<WorkingPaperDetail> WorkingPaperDetails { get; set; }
        public virtual DbSet<WorkingPaper> WorkingPapers { get; set; }
        public virtual DbSet<ExceptionIncident> ExceptionIncidents { get; set; }

        public virtual DbSet<TestingTemplate> TestingTemplates { get; set; }

        public virtual DbSet<DepartmentRiskControl> DepartmentRiskControls { get; set; }

        public virtual DbSet<DepartmentRisk> DepartmentRisks { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<UnitOrganizationRole> UnitOrganizationRoles { get; set; }

        public virtual DbSet<DataList> DataLists { get; set; }

        public virtual DbSet<ExceptionTypeColumn> ExceptionTypeColumns { get; set; }

        public virtual DbSet<ExceptionType> ExceptionTypes { get; set; }

        public virtual DbSet<Control> Controls { get; set; }

        public virtual DbSet<Risk> Risks { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public ICMSDemoDbContext(DbContextOptions<ICMSDemoDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
           
           
           
           
           
           
           
           
 

           
           
           
           
           
           
           
           
           
           
            modelBuilder.Entity<LossEventTask>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<LossTypeColumn>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<LossEvent>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<LibraryControl>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<LibraryRisk>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<DepartmentRating>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Rating>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Project>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ProcessRiskControl>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ProcessRisk>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Process>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ExceptionIncident>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<TestingTemplate>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<DepartmentRiskControl>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<DepartmentRisk>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Department>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<DataList>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ExceptionTypeColumn>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ExceptionType>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Control>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Risk>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
