using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Handlers;
using Abp.Threading;
using ICMSDemo.Departments;
using ICMSDemo.Processes;
using ICMSDemo.ProcessRiskControls;
using ICMSDemo.ProcessRisks;
using ICMSDemo.Projects.Events;
using ICMSDemo.TestingTemplates;
using ICMSDemo.WorkingPapers;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICMSDemo.Projects.EventHandlers
{
    public class WorkingPaperCreator : IEventHandler<ProjectActivatedEventData>, ITransientDependency
    {
        private readonly IRepository<WorkingPaper, Guid> _workingPaperRepository;
        private readonly IRepository<Department, long> _departmentRepository;
        private readonly IRepository<Process, long> _processRepository;
        private readonly IRepository<ProcessRiskControl> _processRiskControlRepository;
        private readonly IRepository<ProcessRisk> _processRiskRepository;
        private readonly IRepository<TestingTemplate> _testingTemplateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkingPaperCreator(IRepository<WorkingPaper, Guid> workingPaperRepository, IRepository<Department, long> departmentRepository, IRepository<Process, long> processRepository, IRepository<ProcessRiskControl> processRiskControlRepository, IRepository<ProcessRisk> processRiskRepository, IRepository<TestingTemplate> testingTemplateRepository, IUnitOfWork unitOfWork)
        {
            _workingPaperRepository = workingPaperRepository;
            _departmentRepository = departmentRepository;
            _processRepository = processRepository;
            _processRiskControlRepository = processRiskControlRepository;
            _processRiskRepository = processRiskRepository;
            _testingTemplateRepository = testingTemplateRepository;
            _unitOfWork = unitOfWork;
        }

        [UnitOfWork]
        public void HandleEvent(ProjectActivatedEventData eventData)
        {

            using (var uow = _unitOfWork.SetTenantId(eventData.TenantId))
            {
                if (eventData.Project.ReviewType == ReviewType.Department)
                {
                    AsyncHelper.RunSync(() => DepartmentScope(eventData.Project.ScopeId.Value, eventData.Project.Cascade, eventData.Project.Id));
                }
                else
                {
                    AsyncHelper.RunSync(() => ProcessScope(eventData.Project.ScopeId.Value, eventData.Project.Cascade, eventData.Project.Id));
                }
            }

        }



        public async Task DepartmentScope(long departmentId, bool cascade, int projectId)
        {
            var allDepartments = await _departmentRepository.GetAllListAsync();

            var department =  allDepartments.FirstOrDefault(x => x.Id == departmentId);
          
            var departmentCode = department.Code;

            List<string> roots = new List<string>();

            List<string> codes = new List<string>();

            List<Department> allMyDepartments = new List<Department>();

            roots = departmentCode.Split(".").ToList();
            string previousCode = string.Empty;


            foreach (var item in roots)
            {
                previousCode = previousCode == string.Empty ? item : previousCode + "." + item;
                codes.Add(previousCode);
            }


            if (cascade)
            {
                allMyDepartments = allDepartments.Where(x => x.Code.StartsWith(department.Code)).ToList();
            }
            else
            {
                allMyDepartments.Add(department);
            }


            var departments =  allDepartments.Where(x => codes.Any(e => e == x.Code)).Select(x => x.Id).ToList();

            

            var allProcesses = await _processRepository.GetAllListAsync();
            var allProcessRisks = await _processRiskRepository.GetAllListAsync();
            var allProcessRiskControls = await _processRiskControlRepository.GetAllListAsync();
            var allTestingTemplates = await _testingTemplateRepository.GetAllListAsync(x => x.IsActive);

            foreach (var dept in departments)
            {
                //Get Processes
                var allPossibleProcesses = allProcesses.Where(x => x.DepartmentId.Value == dept || (x.DepartmentId.Value != dept && x.Casade)).ToList();
                var relevantProcesses = allPossibleProcesses.Where(x => departments.Any(y => y == x.DepartmentId)).ToList();

                var relevantProcessRisk = allProcessRisks.Where(x => relevantProcesses.Any(y => y.Id == x.ProcessId)).ToList();

                var relevantProcessRiskControl = allProcessRiskControls.Where(x => relevantProcessRisk.Any(y => y.Id == x.ProcessRiskId)).ToList();

                var relevantTestingTemplates = allTestingTemplates.Where(x => relevantProcessRiskControl.Any(y => y.Id == x.ProcessRiskControlId)).ToList();



                foreach (var d in allMyDepartments)
                {
                    if (!d.IsAbstract)
                    {
                        foreach (var tt in relevantTestingTemplates)
                        {
                            var workingPaperNew = new WorkingPaper();
                            workingPaperNew.ProjectId = projectId;
                            workingPaperNew.OrganizationUnitId = d.Id;
                            workingPaperNew.TestingTemplateId = tt.Id;
                            workingPaperNew.TenantId = tt.TenantId;
                            workingPaperNew.TaskDate = Abp.Timing.Clock.Now;
                            workingPaperNew.Code = DateTime.Now.ToString("ddMMyy") + "-" + Guid.NewGuid().ToString().ToUpper().Substring(1, 7);
                            await _workingPaperRepository.InsertAsync(workingPaperNew);
                        }

                    }
                }

            }


        }


        public async Task ProcessScope(long processId, bool cascade, int projectId)
        {
            var allDepartments = await _departmentRepository.GetAllListAsync();

            var relevantProcess = await _processRepository.FirstOrDefaultAsync(x => x.Id == processId);

           var parentDepartment =  allDepartments.FirstOrDefault(x => x.Id == relevantProcess.DepartmentId);

            List<Department> departmentList = new List<Department>();
            
            if (cascade)
            {
                departmentList =  allDepartments.Where(x => x.Code.StartsWith(parentDepartment.Code)).ToList();
            }
            else
            {
                departmentList.Add(parentDepartment);
            }

            var allProcessRisks = await _processRiskRepository.GetAllListAsync();
            var allProcessRiskControls = await _processRiskControlRepository.GetAllListAsync();
            var allTestingTemplates = await _testingTemplateRepository.GetAllListAsync(x => x.IsActive);



                var relevantProcessRisk = allProcessRisks.Where(x => relevantProcess.Id == x.ProcessId).ToList();

                var relevantProcessRiskControl = allProcessRiskControls.Where(x => relevantProcessRisk.Any(y => y.Id == x.ProcessRiskId)).ToList();

                var relevantTestingTemplates = allTestingTemplates.Where(x => relevantProcessRiskControl.Any(y => y.Id == x.ProcessRiskControlId)).ToList();

       
            foreach (var dept in departmentList)
            {
                if (!dept.IsAbstract)
                {
                    foreach (var tt in relevantTestingTemplates)
                    {

                        var workingPaperNew = new WorkingPaper();
                        workingPaperNew.ProjectId = projectId;
                        workingPaperNew.OrganizationUnitId = dept.Id;
                        workingPaperNew.TestingTemplateId = tt.Id;
                        workingPaperNew.TenantId = tt.TenantId;
                        workingPaperNew.TaskDate = Abp.Timing.Clock.Now;
                        workingPaperNew.Code = DateTime.Now.ToString("yyMMdd") + "-" + Guid.NewGuid().ToString().ToUpper().Substring(1, 7);
                        await _workingPaperRepository.InsertAsync(workingPaperNew);
                    }

                }


            }

        }
    }
}
