using Abp.Domain.Repositories;
using ICMSDemo.Authorization.Users;
using ICMSDemo.Departments;
using ICMSDemo.ExceptionIncidents;
using ICMSDemo.ExceptionTypes;
using ICMSDemo.WorkingPapers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services.Dto;
using ICMSDemo.ExceptionIncidents.Dtos;
using ICMSDemo.WorkingPaperNews.Dtos;
using Abp.Authorization;
using System.Runtime.CompilerServices;

namespace ICMSDemo.Common
{
    [AbpAuthorize]
    public class WorkspaceAppService : ICMSDemoAppServiceBase
    {
        private readonly IRepository<ExceptionIncident> _exceptionIncidentRepository;
        private readonly IRepository<UnitOrganizationRole, long> _unitOrganizationRoleRepository;
        private readonly IRepository<ExceptionType, int> _lookup_exceptionTypeRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<WorkingPaper, Guid> _workingPaperRepository;
        private readonly IRepository<WorkingPaperDetail> _workingPaperDetailRepository;
        private readonly IRepository<Department, long> _lookup_organizationUnitRepository;

        public WorkspaceAppService(
            IRepository<ExceptionIncident> exceptionIncidentRepository,
            IRepository<UnitOrganizationRole, long> unitOrganizationRoleRepository,
            IRepository<ExceptionType, int> lookup_exceptionTypeRepository,
            IRepository<User, long> lookup_userRepository,
            IRepository<WorkingPaper, Guid> lookup_workingPaperTemplateRepository,
            IRepository<WorkingPaperDetail> workingPaperDetailRepository,
            IRepository<Department, long> lookup_organizationUnitRepository)
        {
            _exceptionIncidentRepository = exceptionIncidentRepository;
            _unitOrganizationRoleRepository = unitOrganizationRoleRepository;
            _lookup_exceptionTypeRepository = lookup_exceptionTypeRepository;
            _lookup_userRepository = lookup_userRepository;
            _workingPaperRepository = lookup_workingPaperTemplateRepository;
            _workingPaperDetailRepository = workingPaperDetailRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
        }


        public async Task<ListResultDto<GetExceptionIncidentForViewDto>> GetExceptions()
        {
            var departments = await UserManager.GetOrganizationUnitsAsync(await UserManager.GetUserByIdAsync((long)AbpSession.UserId));

            string previousCode = string.Empty;
            List<string> codes = new List<string>();

            List<Department> allDepartments = await _lookup_organizationUnitRepository.GetAllListAsync();

            foreach (var item in departments)
            {
                var departmentCode = await OrganizationUnitManager.GetCodeAsync(item.Id);
                var childrenDept = allDepartments.Where(x => x.Code.StartsWith(item.Code)).Select(x => x.Code).ToList();
                codes.AddRange(childrenDept);
            }

            var exceptionIncidents = _exceptionIncidentRepository.GetAll()
                        .Include(e => e.ExceptionTypeFk)
                        .Include(e => e.RaisedByFk)
                        .Include(e => e.OrganizationUnitFk)
                        .Select(x => new GetExceptionIncidentForViewDto
                        {
                            ExceptionIncident = new ExceptionIncidentDto
                            {
                                Code = x.Code,
                                Date = x.Date,
                                Description = x.Description,
                                Status = x.Status,
                                ClosureDate = x.ClosureDate,
                                ClosureComments = x.ClosureComments,
                                RaisedByClosureComments = x.RaisedByClosureComments,
                                Id = x.Id
                            },
                            ExceptionTypeName = x.ExceptionTypeFk == null ? "" : x.ExceptionTypeFk.Name,
                            UserName = x.RaisedByFk == null ? "" : x.RaisedByFk.FullName,
                            DeptCode = x.OrganizationUnitFk == null ? "" : x.OrganizationUnitFk.Code,
                            OrganizationUnitDisplayName = x.OrganizationUnitFk == null ? "" : x.OrganizationUnitFk.DisplayName.ToString()
                        });

            var exceptions = await exceptionIncidents.ToListAsync();
            var totalCount = 0;

            if (codes.Count > 0)
            {
                exceptions = exceptions.Where(x => codes.Any(e => e == x.DeptCode)).ToList();
                totalCount = exceptions.Count();
            }

            var output = exceptions.Take(10).ToList();

            return new ListResultDto<GetExceptionIncidentForViewDto>(output);
        }


        public async Task<ListResultDto<GetWorkingPaperNewForViewDto>> GetWorkingPapers()
        {
            //var departments = await UserManager.GetOrganizationUnitsAsync(await UserManager.GetUserByIdAsync((long)AbpSession.UserId));

            //string previousCode = string.Empty;
            //List<string> codes = new List<string>();

            //List<Department> allDepartments = await _lookup_organizationUnitRepository.GetAllListAsync();

            //foreach (var item in departments)
            //{
            //    var departmentCode = await OrganizationUnitManager.GetCodeAsync(item.Id);
            //    var childrenDept = allDepartments.Where(x => x.Code.StartsWith(item.Code)).Select(x => x.Code).ToList();
            //    codes.AddRange(childrenDept);
            //}

            var workingPaperDetail = await _workingPaperDetailRepository.GetAll().Where(x => x.WorkingPaperId != null)
                .GroupBy(x => x.WorkingPaperId)
                .Select(g => new
                {
                    workingPaperId = g.Key,
                    lastSequence = (double)g.Max(x => x.Sequence)
                }).ToDictionaryAsync(x => x.workingPaperId, y => y.lastSequence);

            var workingPaperNew = from w in _workingPaperRepository.GetAll()
                                                                   .Include(e => e.TestingTemplate)
                                                                    .Include(e => e.Project)
                                                                    .Where(x => x.AssignedToId == AbpSession.UserId)

                                  join u1 in _lookup_userRepository.GetAll() on w.AssignedToId equals u1.Id into j1
                                  from s2 in j1.DefaultIfEmpty()

                                  join u2 in _lookup_userRepository.GetAll() on w.CompletedById equals u2.Id into j2
                                  from s3 in j2.DefaultIfEmpty()

                                  join u3 in _lookup_userRepository.GetAll() on w.ReviewedById equals u3.Id into j3
                                  from s4 in j3.DefaultIfEmpty()
                                                               
                                                                  
                                                                    
                                  join ou in _lookup_organizationUnitRepository.GetAll() on w.OrganizationUnitId equals ou.Id into ou1
                                  from ou2 in ou1.DefaultIfEmpty()

                                  select new GetWorkingPaperNewForViewDto()
                                  {
                                      WorkingPaperNew = new WorkingPaperNewDto
                                      {
                                          Code = w.Code,
                                          Comment = w.Comment,
                                          TaskDate = w.TaskDate,
                                          DueDate = w.DueDate,
                                          ProjectId = w.ProjectId,
                                          OrganizationUnitId = w.OrganizationUnitId,
                                          CompletedUserId = w.CompletedById,
                                          AssigneeId = w.AssignedToId,
                                          TaskStatus = w.TaskStatus,
                                          Score = w.Score,
                                          ReviewedDate = w.ReviewedDate,
                                          CompletionDate = w.CompletionDate,
                                          Id = w.Id
                                      },
                                      TestingTemplateCode = w.TestingTemplate == null ? "" : w.TestingTemplate.Title.ToString(),
                                      OuCode = ou2 == null ? "" : ou2.Code.ToString(),
                                      ProjectName = w.Project.Title,
                                      OrganizationUnitDisplayName = ou2 == null ? "" : ou2.DisplayName.ToString(),
                                      AssignedTo = w.AssignedToId == null ? "" : s2.FullName,
                                      CompletedBy = w.CompletedById == null ? "" : s3.FullName,
                                      ReviewedBy = w.ReviewedById == null ? "" : s4.FullName,
                                      //Frequency = w.TestingTemplate == null ? Frequency.Continuous : w.TestingTemplate.Frequency,
                                      //SampleSize = w.TestingTemplate == null ? 0 : w.TestingTemplate.SampleSize
                                      //CompletionLevel = workingPaperDetail.ContainsKey(w.Id) && w.TestingTemplate != null && w.TestingTemplate.SampleSize > 0 ? (workingPaperDetail[w.Id] / (double)w.TestingTemplate.SampleSize) : 0
                                  };

            var workingPapers = await workingPaperNew.ToListAsync();
            var totalCount = workingPapers.Count();

            //if (codes.Count > 0)
            //{
            //    workingPapers = workingPapers.Where(x => codes.Any(e => e == x.OuCode)).ToList();
            //    totalCount = workingPapers.Count();
            //}

            var output = workingPapers.Take(10).ToList();

            return new ListResultDto<GetWorkingPaperNewForViewDto>(output);
        }
    }
}
