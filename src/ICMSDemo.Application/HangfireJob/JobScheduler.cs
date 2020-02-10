using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using ICMSDemo.WorkingPapers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICMSDemo.HangfireJob
{
    public class ScheduleJob : BackgroundJob<int>, ITransientDependency
    {
        private readonly IRepository<WorkingPaper, long> _wpRepository;
        public OrganizationUnitManager OrganizationUnitManager { get; set; }

        public ScheduleJob(IRepository<WorkingPaper, long> userRepository, IEmailSender emailSender)
        {
            _wpRepository = userRepository;
            _emailSender = emailSender;
        }

        [UnitOfWork]
        public override void Execute(SimpleSendEmailJobArgs args)
        {
            var departmentCode = await OrganizationUnitManager.GetCodeAsync((long)input.DepartmentId);
            var senderUser = _userRepository.Get(args.SenderUserId);
            var targetUser = _userRepository.Get(args.TargetUserId);

            _emailSender.Send(senderUser.EmailAddress, targetUser.EmailAddress, args.Subject, args.Body);
        }
    }
}
