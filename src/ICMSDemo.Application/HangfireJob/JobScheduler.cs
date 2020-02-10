using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Session;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using ICMSDemo.DepartmentRiskControls;
using ICMSDemo.DepartmentRisks;
using ICMSDemo.Departments;
using ICMSDemo.TestingTemplates;
using ICMSDemo.WorkingPapers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMSDemo.HangfireJob
{
    public class ScheduleJob : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IRepository<WorkingPaper, Guid> _wpRepository;
        private readonly IRepository<Department, long> _lookup_departmentRepository;
        private readonly IRepository<DepartmentRiskControl> _departmentRiskControlRepository;
        private readonly IRepository<DepartmentRisk> _departmentRisk;
        private readonly IRepository<DepartmentRiskControl> _departmentRiskControl;
        private readonly IRepository<TestingTemplate> _testingTemplate;

        public IAbpSession _abpSession;

        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        public OrganizationUnitManager _OrganizationUnitManager { get; set; }

        public ScheduleJob(AbpTimer timer, IRepository<TestingTemplate> testingTemplate, IRepository<WorkingPaper, Guid> wpRepository, IRepository<DepartmentRisk> departmentRisk, IRepository<OrganizationUnit, long> organizationUnitRepository, IRepository<Department, long> lookup_departmentRepository, IRepository<DepartmentRiskControl> departmentRiskControlRepository) : base(timer)
        {
            _wpRepository = wpRepository;
            _lookup_departmentRepository = lookup_departmentRepository;
            _departmentRiskControlRepository = departmentRiskControlRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _departmentRisk = departmentRisk;
            _testingTemplate = testingTemplate;
            _abpSession = NullAbpSession.Instance;

            Timer.Period = 5000;  //5 seconds (good for tests, but normally will be more)
        }

        
        //public override void Execute(int args)
        //{


        //}
        [UnitOfWork]
        protected override void DoWork()
        {
            //Logger.Info(DateTime.Now.ToString());

            //if(1!=1)
            //{
                var buIds = _lookup_departmentRepository.GetAll().Where(o => !o.IsAbstract).Select(p => p.Id).ToList();
                var deptRisks = _departmentRisk.GetAll().Where(o => buIds.Contains(o.DepartmentId ?? 0)).ToList();
                var deptRiskIds = deptRisks.Select(o => o.Id).ToList();

                var deptRiskControls = _departmentRiskControlRepository.GetAll().Where(o => deptRiskIds.Contains(o.DepartmentRiskId ?? 0)).ToList();
                var deptRiskControlIds = deptRiskControls.Select(o => o.Id).ToList();

                var testingTemplate = _testingTemplate.GetAll().Where(o => o.IsActive && deptRiskControlIds.Contains(o.DepartmentRiskControlId ?? 0)).ToList();
                var testingTemplateIds = testingTemplate.Select(o => o.Id).ToList();


                var wp = new WorkingPaper();
                foreach (var buId in buIds)
                {
                    var deptR = deptRisks.FirstOrDefault(o => o.DepartmentId == buId);
                    if (deptR == null)
                        continue;

                    var deptRC = deptRiskControls.FirstOrDefault(o => o.DepartmentRiskId == deptR.Id);
                    if (deptRC == null)
                        continue;

                    var tTemplate = testingTemplate.FirstOrDefault(o => o.DepartmentRiskControlId == deptRC.Id);
                    if (tTemplate == null)
                        continue;

                    wp = new WorkingPaper
                    {
                        OrganizationUnitId = buId,
                        TaskStatus = TaskStatus.PendingReview,
                        CreatorUserId = _abpSession.UserId,
                        Score = 0,
                        TestingTemplateId = tTemplate.Id,
                        CreationTime = DateTime.Now,
                        TenantId = _abpSession.TenantId ?? 0
                    };

                    _wpRepository.Insert(wp);
                }

            //}

        }
    }
}
