using Abp.Organizations;
using Abp.Organizations;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.Projects.Exporting;
using ICMSDemo.Projects.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ICMSDemo.Processes;
using ICMSDemo.Departments;
using Abp.Timing;
using ICMSDemo.Authorization.Users;
using Abp.UI;
using Stripe;
using ICMSDemo.Projects.Events;
using ICMSDemo.WorkingPapers;
using ICMSDemo.ExceptionIncidents;
using ICMSDemo.Ratings;
using ICMSDemo.DepartmentRatingHistory;
using static ICMSDemo.IcmsEnums;

namespace ICMSDemo.Projects
{
    [AbpAuthorize]
    public class RcsaProgramAssessmentAppService : ICMSDemoAppServiceBase
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<Department, long> _lookup_departmentRepository;
        private readonly IRepository<RcsaProgramAssessment> _rcsaAssessmentRepository;


        public RcsaProgramAssessmentAppService(
            IRepository<Project> projectRepository,
            IRepository<Department, long> lookup_departmentRepository,
            IRepository<RcsaProgramAssessment> rcsaAssessmentRepository)
        {
            _projectRepository = projectRepository;
            _lookup_departmentRepository = lookup_departmentRepository;
            _rcsaAssessmentRepository = rcsaAssessmentRepository;
        }


        public async Task<RcsaProgramCheck> GetActiveRcsaProgram()
        {
            var program = await _projectRepository.FirstOrDefaultAsync(e => e.ProjectOwner == ProjectOwner.OperationRisk && e.Commenced == true);

            var check = new RcsaProgramCheck();
            if (program != null)
            {
                check.Active = true;
                check.ProjectId = program.Id;
            }
            else
            {
                check.Active = false;
                check.ProjectId = -1;
            }

            return check;
        }


        public async Task<PagedResultDto<GetRcsaProgramAssessmentForViewDto>> GetProgramAssessment(GetAllRcsaProgramAssessmentInput input)
        {
            var filtered = _rcsaAssessmentRepository.GetAll().Include(e => e.VerifiedByUserFk).Where(e => e.ProjectId == input.ProjectId);

            var pagedAndFilteredProjects = filtered
                .OrderBy(input.Sorting ?? "businessUnitId")
                .PageBy(input);

            var query = from rcsa in pagedAndFilteredProjects
                        join ou in _lookup_departmentRepository.GetAll().Include(e => e.SupervisorUserFk) on rcsa.BusinessUnitId equals ou.Id into ou1
                        from ou2 in ou1.DefaultIfEmpty()
                        select new GetRcsaProgramAssessmentForViewDto()
                        {
                            Assessment = ObjectMapper.Map<RcsaProgramAssessmentDto>(rcsa),
                            DepartmentName = ou2 == null ? "" : ou2.DisplayName,
                            UnitHead = ou2 == null ? "" : (ou2.SupervisorUserFk == null ? "" : ou2.SupervisorUserFk.FullName),
                            VerifiedByUserName = rcsa.VerifiedByUserFk == null ? "" : rcsa.VerifiedByUserFk.FullName
                        };

            var totalCount = await filtered.CountAsync();

            return new PagedResultDto<GetRcsaProgramAssessmentForViewDto>(
                totalCount,
                await query.ToListAsync()
            );
        }



        public async Task SaveRcsaProgramAssessment(RcsaProgramAssessmentDto input)
        {
            var assesment = await _rcsaAssessmentRepository.FirstOrDefaultAsync(e => e.ProjectId == input.ProjectId && e.BusinessUnitId == input.BusinessUnitId);

            if (input.Id == null && assesment == null)
            {
                await CreateRcsaProgramAssessment(input);
            }
            else
            {
                input.Id = assesment.Id;
                await UpdateRcsaProgramAssessment(input);
            }
        }

        protected async Task CreateRcsaProgramAssessment(RcsaProgramAssessmentDto input)
        {
            var assesment = ObjectMapper.Map<RcsaProgramAssessment>(input);
            if (AbpSession.TenantId != null)
            {
                assesment.TenantId = (int)AbpSession.TenantId;
            }
            if (AbpSession.UserId != null)
            {
                assesment.VerifiedByUserId = (int)AbpSession.UserId;
            }
            assesment.DateVerified = DateTime.Now;

            await _rcsaAssessmentRepository.InsertAsync(assesment);
        }

        protected async Task UpdateRcsaProgramAssessment(RcsaProgramAssessmentDto input)
        {
            var assesment = await _rcsaAssessmentRepository.FirstOrDefaultAsync((int)input.Id);

            if (AbpSession.UserId != null)
            {
                assesment.VerifiedByUserId = (int)AbpSession.UserId;
            }
            assesment.DateVerified = DateTime.Now;
            assesment.VerificationStatus = input.VerificationStatus;
            await _rcsaAssessmentRepository.UpdateAsync(assesment);
        }

        public async Task DeleteRcsaProgramAssessment(EntityDto input)
        {
            await _rcsaAssessmentRepository.DeleteAsync(input.Id);
        }
    }
}