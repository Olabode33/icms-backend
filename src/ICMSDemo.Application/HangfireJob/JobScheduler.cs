using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using ICMSDemo.DepartmentRiskControls;
using ICMSDemo.Departments;
using ICMSDemo.WorkingPapers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMSDemo.HangfireJob
{
    public class ScheduleJob : BackgroundJob<int>, ITransientDependency
    {
        private readonly IRepository<WorkingPaper, Guid> _wpRepository;
        private readonly IRepository<Department, long> _lookup_departmentRepository;
        private readonly IRepository<DepartmentRiskControl> _departmentRiskControlRepository;

        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        public OrganizationUnitManager _OrganizationUnitManager { get; set; }

        public ScheduleJob(IRepository<WorkingPaper, Guid> wpRepository, IRepository<OrganizationUnit, long> organizationUnitRepository, IRepository<Department, long> lookup_departmentRepository, IRepository<DepartmentRiskControl> departmentRiskControlRepository)
        {
            _wpRepository = wpRepository;
            _lookup_departmentRepository = lookup_departmentRepository;
            _departmentRiskControlRepository = departmentRiskControlRepository;
            _organizationUnitRepository = organizationUnitRepository;
        }

        [UnitOfWork]
        public override void Execute(int args)
        {

            var departments = _organizationUnitRepository.GetAllList();
            //var departmentCodex = _OrganizationUnitManager.GetCode(0);

            var departmentCodes= departments.Select(o => o.Code).ToList();


        //string[] roots = departmentCode.Split(".");
        //    string previousCode = string.Empty;
        //    List<string> codes = new List<string>();

        //    foreach (var item in roots)
        //    {
        //        previousCode = previousCode == string.Empty ? item : previousCode + "." + item;
        //        codes.Add(previousCode);
        //    }

            var departments = _lookup_departmentRepository.GetAllList(x => codes.Any(e => e == x.Code));


            var filteredDepartmentRiskControls = _departmentRiskControlRepository.GetAll()
                                                .Include(e => e.ControlFk)
                                                .Include(e => e.DepartmentRiskFk)
                                                .ThenInclude(x => x.DepartmentFk)
                                                .Where(x => x.DepartmentId == input.DepartmentId || (x.DepartmentId != input.DepartmentId && x.Cascade))
            .Select(x => new GetDepartmentRiskControlForViewDto()
            {
                DepartmentRiskControl = new DepartmentRiskControlDto
                {
                    Code = x.Code,
                    Notes = x.Notes,
                    Frequency = x.Frequency,
                    Id = x.Id,
                    DepartmentId = x.DepartmentId,
                    Inherited = x.DepartmentId == null ? true : false,
                    DepartmentRiskId = x.DepartmentRiskId,
                    ControlId = x.ControlId,
                    DepartmentCode = x.DepartmentRiskFk.DepartmentFk.Code
                },
                DepartmentRiskCode = x.DepartmentRiskFk.Code,
                ControlCode = x.ControlFk.Name + " [" + x.ControlFk.Code + "]"
            });


        }

        //public override void Execute(SimpleSendEmailJobArgs args)
        //{
        //    var departmentCode = await OrganizationUnitManager.GetCodeAsync((long)input.DepartmentId);
        //    var senderUser = _userRepository.Get(args.SenderUserId);
        //    var targetUser = _userRepository.Get(args.TargetUserId);

        //    _emailSender.Send(senderUser.EmailAddress, targetUser.EmailAddress, args.Subject, args.Body);
        //}
    }
}
