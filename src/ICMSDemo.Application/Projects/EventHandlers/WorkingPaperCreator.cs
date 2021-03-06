﻿using Abp.Authorization.Users;
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
using static ICMSDemo.IcmsEnums;

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
        private readonly IRepository<UnitOrganizationRole, long> _unitOrganizationRoleRepository;

        public WorkingPaperCreator(IRepository<WorkingPaper, Guid> workingPaperRepository,
            IRepository<UnitOrganizationRole, long> unitOrganizationRoleRepository,
            IRepository<Department, long> departmentRepository, IRepository<Process, long> processRepository, IRepository<ProcessRiskControl> processRiskControlRepository, IRepository<ProcessRisk> processRiskRepository, IRepository<TestingTemplate> testingTemplateRepository, IUnitOfWork unitOfWork)
        {
            _workingPaperRepository = workingPaperRepository;
            _departmentRepository = departmentRepository;
            _processRepository = processRepository;
            _processRiskControlRepository = processRiskControlRepository;
            _processRiskRepository = processRiskRepository;
            _testingTemplateRepository = testingTemplateRepository;
            _unitOfWork = unitOfWork;
            _unitOrganizationRoleRepository = unitOrganizationRoleRepository;
        }

        [UnitOfWork]
        public void HandleEvent(ProjectActivatedEventData eventData)
        {

            using (var uow = _unitOfWork.SetTenantId(eventData.TenantId))
            {
                if (eventData.Project.ReviewType == ReviewType.Department)
                {
                    AsyncHelper.RunSync(() => DepartmentScope(eventData.Project.ScopeId.Value, eventData.Project.Cascade, eventData.Project,eventData.ProjectOwner));
                }
                else
                {
                    AsyncHelper.RunSync(() => ProcessScope(eventData.Project.ScopeId.Value, eventData.Project.Cascade, eventData.Project));
                }
            }

        }



        public async Task DepartmentScope(long departmentId, bool cascade, Project project,ProjectOwner? projectowner)
        {
            var allDepartments = await _departmentRepository.GetAllListAsync();

            var department =  allDepartments.FirstOrDefault(x => x.Id == departmentId);
          
            var departmentCode = department.Code;

            List<string> roots = new List<string>();

            List<string> codes = new List<string>();

            List<Department> allMyDepartments = new List<Department>();
            List<TestingTemplate> relevantTestingTemplates = new List<TestingTemplate>();

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

            //Get the ID of the current department and all the parents
            var departments =  allDepartments.Where(x => codes.Any(e => e == x.Code)).Select(x => x.Id).ToList();

            

            var allProcesses = await _processRepository.GetAllListAsync();
            var allProcessRisks = await _processRiskRepository.GetAllListAsync();
            var allProcessRiskControls = await _processRiskControlRepository.GetAllListAsync();
            var allTestingTemplates = await _testingTemplateRepository.GetAllListAsync(x => x.IsActive);
            var allUserRolesInDepartments = await _unitOrganizationRoleRepository.GetAllListAsync();

            //    foreach (var dept in departments)
            //  {


            //Get All Processes for that department or for those that are cascaded
            var allPossibleProcesses = allProcesses.Where(x => x.DepartmentId.Value == departmentId || (x.DepartmentId.Value != departmentId && x.Casade)).ToList();

            // Get all processes that belong to the department or that have been inherited
            var relevantProcesses = allPossibleProcesses.Where(x => departments.Any(y => y == x.DepartmentId)).ToList();

            var relevantProcessRisk = allProcessRisks.Where(x => relevantProcesses.Any(y => y.Id == x.ProcessId)).ToList();

            var relevantProcessRiskControl = allProcessRiskControls.Where(x => relevantProcessRisk.Any(y => y.Id == x.ProcessRiskId)).ToList();

            // relevent hint
            if (projectowner == ProjectOwner.General)
            {
                relevantTestingTemplates = allTestingTemplates.Where(x => relevantProcessRiskControl.Any(y => y.Id == x.ProcessRiskControlId && x.ProjectOwner == ProjectOwner.General)).ToList();

            }
            else 
            {
                relevantTestingTemplates = allTestingTemplates.Where(x => relevantProcessRiskControl.Any(y => y.Id == x.ProcessRiskControlId && (x.ProjectOwner == projectowner || x.ProjectOwner == ProjectOwner.General))).ToList();

            }
           
            foreach (var d in allMyDepartments)
                {

                if (!d.IsAbstract)
                    {
                        var controlHeadInDepartment = allUserRolesInDepartments.FirstOrDefault(x => x.OrganizationUnitId == d.Id && x.DepartmentRole == DepartmentRole.ControlHead);

                        foreach (var tt in relevantTestingTemplates)
                        {
                            var workingPaperNew = new WorkingPaper();
                            workingPaperNew.ProjectId = project.Id;
                            workingPaperNew.OrganizationUnitId = d.Id;
                            workingPaperNew.TestingTemplateId = tt.Id;
                            workingPaperNew.TenantId = tt.TenantId;
                            workingPaperNew.TaskDate = Abp.Timing.Clock.Now;
                            workingPaperNew.DueDate = project.BudgetedEndDate;
                            workingPaperNew.Code = DateTime.Now.ToString("ddMMyy") + "-" + Guid.NewGuid().ToString().ToUpper().Substring(1, 7);
                           
                            if (controlHeadInDepartment != null)
                            {
                                workingPaperNew.AssignedToId = controlHeadInDepartment.UserId;
                            }
                   
                            await _workingPaperRepository.InsertAsync(workingPaperNew);
                        }

                    }
                }

           


        }


        public async Task ProcessScope(long processId, bool cascade, Project project)
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
                        workingPaperNew.ProjectId = project.Id;
                        workingPaperNew.OrganizationUnitId = dept.Id;
                        workingPaperNew.TestingTemplateId = tt.Id;
                        workingPaperNew.DueDate = project.BudgetedEndDate;
                        
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
