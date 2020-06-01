using ICMSDemo.Authorization.Users;

using Abp.Organizations;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.Departments.Exporting;
using ICMSDemo.Departments.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Authorization.Users;
using ICMSDemo.DepartmentRatingHistory;
using ICMSDemo.Ratings;
using ICMSDemo.DepartmentRatingHistory.Dtos;

namespace ICMSDemo.Departments
{
    
    public class DepartmentsAppService : ICMSDemoAppServiceBase, IDepartmentsAppService
    {
        private readonly IRepository<Department, long> _departmentRepository;
        private readonly IDepartmentsExcelExporter _departmentsExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<Rating> _lookupRating;
        private readonly IRepository<DepartmentRating> _lookupDepartmentRating;


        public DepartmentsAppService(IRepository<Department, long> departmentRepository,
             IRepository<DepartmentRating> lookupDepartmentRating,
             IRepository<Rating> lookupRating,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IDepartmentsExcelExporter departmentsExcelExporter, IRepository<User, long> lookup_userRepository, IRepository<OrganizationUnit, long> lookup_organizationUnitRepository)
        {
            _departmentRepository = departmentRepository;
            _departmentsExcelExporter = departmentsExcelExporter;
            _lookup_userRepository = lookup_userRepository;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _lookupDepartmentRating = lookupDepartmentRating;
            _lookupRating = lookupRating;
        }

        public async Task<PagedResultDto<GetDepartmentForViewDto>> GetAll(GetAllDepartmentsInput input)
        {

            var filteredDepartments = _departmentRepository.GetAll()
                        .Include(e => e.SupervisorUserFk)
            
                        .Include(e => e.ControlTeamFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(input.IsControlTeamFilter > -1, e => (input.IsControlTeamFilter == 1 && e.IsControlTeam) || (input.IsControlTeamFilter == 0 && !e.IsControlTeam))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.SupervisorUserFk != null && e.SupervisorUserFk.Name == input.UserNameFilter)

                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.ControlTeamFk != null && e.ControlTeamFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

            var pagedAndFilteredDepartments = filteredDepartments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var departments = from o in pagedAndFilteredDepartments
                              join o1 in _lookup_userRepository.GetAll() on o.SupervisorUserId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

               

                              join o3 in _lookup_organizationUnitRepository.GetAll() on o.ControlTeamId equals o3.Id into j3
                              from s3 in j3.DefaultIfEmpty()

                              join o4 in _lookup_organizationUnitRepository.GetAll() on o.SupervisingUnitId equals o4.Id into j4
                              from s4 in j4.DefaultIfEmpty()

                              select new GetDepartmentForViewDto()
                              {
                                  Department = new DepartmentDto
                                  {
                                      Code = o.Code,
                                      Name = o.Name,
                                      Description = o.Description,
                                      IsAbstract = o.IsAbstract,
                                      IsControlTeam = o.IsControlTeam,
                                      Id = o.Id
                                  },
                                  UserName = s1 == null ? "" : s1.FullName.ToString(),
                         
                                  OrganizationUnitDisplayName = s3 == null ? "" : s3.DisplayName.ToString(),
                                  SupervsingUnitDisplaName = s4 == null ? "" : s4.DisplayName.ToString()
                              };

            var totalCount = await filteredDepartments.CountAsync();

            return new PagedResultDto<GetDepartmentForViewDto>(
                totalCount,
                await departments.ToListAsync()
            );
        }


        public async Task<PagedResultDto<GetDepartmentForViewDto>> GetAllForRating(GetAllDepartmentsForRatingInput input)
        {

            var filteredDepartments = _departmentRepository.GetAll().Where(x => !x.IsAbstract && !x.IsControlTeam)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(input.RatingFilter != -1, e => e.RatingId == input.RatingFilter);

            var pagedAndFilteredDepartments = filteredDepartments
                .OrderBy(input.Sorting ?? "name asc")
                .PageBy(input);

            var departments = from o in pagedAndFilteredDepartments
                              join o1 in _lookup_userRepository.GetAll() on o.SupervisorUserId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()


                              join o3 in _lookup_organizationUnitRepository.GetAll() on o.ControlTeamId equals o3.Id into j3
                              from s3 in j3.DefaultIfEmpty()

                              join o4 in _lookup_organizationUnitRepository.GetAll() on o.SupervisingUnitId equals o4.Id into j4
                              from s4 in j4.DefaultIfEmpty()

                              join o5 in _lookupRating.GetAll() on o.RatingId equals o5.Id into j5
                              from s5 in j5.DefaultIfEmpty()

                              select new GetDepartmentForViewDto()
                              {
                                  Department = new DepartmentDto
                                  {
                                      Code = o.DepartmentCode,
                                      Name = o.Name,
                                       RatingId = o.RatingId,
                                   
                                      IsAbstract = o.IsAbstract,
                                      IsControlTeam = o.IsControlTeam,
                                      Id = o.Id
                                  },
                                  UserName = s1 == null ? "" : s1.FullName.ToString(),
                                   RatingCode = s5 == null ? "" : s5.Code,
                                   RatingName = s5 == null ? "" : s5.Name,
                                  OrganizationUnitDisplayName = s3 == null ? "" : s3.DisplayName.ToString(),
                                  SupervsingUnitDisplaName = s4 == null ? "" : s4.DisplayName.ToString()
                              };

            var totalCount = await filteredDepartments.CountAsync();

            return new PagedResultDto<GetDepartmentForViewDto>(
                totalCount,
                await departments.ToListAsync()
            );
        }

        public async Task<GetDepartmentForViewDto> GetDepartmentForView(int id)
        {
            var department = await _departmentRepository.GetAsync(id);

            var output = new GetDepartmentForViewDto { Department = ObjectMapper.Map<DepartmentDto>(department) };

            if (output.Department.SupervisorUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Department.SupervisorUserId);
                output.UserName = _lookupUser.Name.ToString();
            }

            if (output.Department.ControlOfficerUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Department.ControlOfficerUserId);
                output.UserName2 = _lookupUser.Name.ToString();
            }

            if (output.Department.ControlTeamId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Department.ControlTeamId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }


            if (output.Department.RatingId != null)
            {
                var rating = await _lookupRating.FirstOrDefaultAsync((int)output.Department.RatingId);
                output.RatingName = rating.Name;
                output.RatingCode = rating.Code;


                var ratingHistory = await _lookupDepartmentRating.GetAllListAsync(x => x.OrganizationUnitId ==  (int)output.Department.Id);
                var ratingHistoryDtoList = ObjectMapper.Map<List<DepartmentRatingDto>>(ratingHistory);
                output.RatingHistory = ratingHistoryDtoList.ToArray();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Departments_Edit)]
        public async Task<GetDepartmentForEditOutput> GetDepartmentForEdit(EntityDto input)
        {
            var department = await _departmentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDepartmentForEditOutput { Department = ObjectMapper.Map<CreateOrEditDepartmentDto>(department) };
            output.Department.Code = department.DepartmentCode;

            if (output.Department.SupervisorUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Department.SupervisorUserId);
                output.UserName = _lookupUser.FullName.ToString();
            }

            if (output.Department.ControlTeamId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Department.ControlTeamId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }



            if (department.RatingId != null)
            {
                var rating = await _lookupRating.FirstOrDefaultAsync((int)department.RatingId);
                output.RatingName = rating.Name;
                output.RatingCode = rating.Code;


                var ratingHistory = await _lookupDepartmentRating.GetAllListAsync(x => x.OrganizationUnitId == (int)output.Department.Id);
                var ratingHistoryDtoList = ObjectMapper.Map<List<DepartmentRatingDto>>(ratingHistory);
                output.RatingHistory = ratingHistoryDtoList.ToArray();
            }


            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDepartmentDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Departments_Create)]
        protected virtual async Task Create(CreateOrEditDepartmentDto input)
        {
            var department = ObjectMapper.Map<Department>(input);

            department.DisplayName = input.Name;

            department.ParentId = input.SupervisingUnitId;

            department.DepartmentCode = input.Code;


            if (AbpSession.TenantId != null)
            {
                department.TenantId = (int)AbpSession.TenantId;
            }

            await OrganizationUnitManager.CreateAsync(department);
            await CurrentUnitOfWork.SaveChangesAsync();

            await AddMembersToNewDepartment(department, department.Id );
        }

        private async Task AddMembersToNewDepartment(Department input, long id)
        {
            List<UnitOrganizationRole> unitOrganizationRoles = new List<UnitOrganizationRole>();

            if (input.SupervisorUserId != null)
                unitOrganizationRoles.Add(new UnitOrganizationRole()
                {
                    DepartmentRole = DepartmentRole.UnitHead,
                    OrganizationUnitId = id,
                    UserId = (long)input.SupervisorUserId,
                    TenantId = AbpSession.TenantId
                });

            if (input.ControlTeamId != null)
            {
                var query = from uo in _userOrganizationUnitRepository.GetAll()
                                        join d in _departmentRepository.GetAll() on uo.OrganizationUnitId equals d.Id
                                     where d.Id == input.ControlTeamId
                                        select  (new UnitOrganizationRole()
                                        {
                                            DepartmentRole = uo.UserId == d.SupervisorUserId ? DepartmentRole.ControlHead : DepartmentRole.ControlTeamMember,
                                            OrganizationUnitId = id,
                                            UserId = uo.UserId,
                                            TenantId = AbpSession.TenantId
                                        });
              var  unitOrganizationRolesCo = await query.ToListAsync();

                if (unitOrganizationRolesCo.Count != 0)
                unitOrganizationRoles.AddRange(unitOrganizationRolesCo);

            }



            foreach (var unitOrganizationRole in unitOrganizationRoles)
            {
                await _userOrganizationUnitRepository.InsertAsync(unitOrganizationRole);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Departments_Edit)]
        protected virtual async Task Update(CreateOrEditDepartmentDto input)
        {
            var department = await _departmentRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, department);
        }

        [AbpAuthorize(AppPermissions.Pages_Departments_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _departmentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDepartmentsToExcel(GetAllDepartmentsForExcelInput input)
        {

            var filteredDepartments = _departmentRepository.GetAll()
                        .Include(e => e.SupervisorUserFk)
                      
                        .Include(e => e.ControlTeamFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(input.IsControlTeamFilter > -1, e => (input.IsControlTeamFilter == 1 && e.IsControlTeam) || (input.IsControlTeamFilter == 0 && !e.IsControlTeam))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.SupervisorUserFk != null && e.SupervisorUserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.ControlTeamFk != null && e.ControlTeamFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

            var query = (from o in filteredDepartments
                         join o1 in _lookup_userRepository.GetAll() on o.SupervisorUserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                    

                         join o3 in _lookup_organizationUnitRepository.GetAll() on o.ControlTeamId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetDepartmentForViewDto()
                         {
                             Department = new DepartmentDto
                             {
                                 Code = o.Code,
                                 Name = o.Name,
                                 Description = o.Description,
                                 IsAbstract = o.IsAbstract,
                                 IsControlTeam = o.IsControlTeam,
                                 Id = o.Id
                             },
                             UserName = s1 == null ? "" : s1.FullName.ToString(),
                      
                             OrganizationUnitDisplayName = s3 == null ? "" : s3.DisplayName.ToString()
                         });


            var departmentListDtos = await query.ToListAsync();

            return _departmentsExcelExporter.ExportToFile(departmentListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_Departments)]
        public async Task<PagedResultDto<DepartmentUserLookupTableDto>> GetAllUserForLookupTable(Departments.Dtos.GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DepartmentUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new DepartmentUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.FullName?.ToString()
                });
            }

            return new PagedResultDto<DepartmentUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Departments)]
        public async Task<PagedResultDto<DepartmentOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(Departments.Dtos.GetAllForLookupTableInput input)
        {
            var query = _departmentRepository.GetAll()
                .Where(x => x.IsControlTeam)
                .WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DepartmentOrganizationUnitLookupTableDto>();
            foreach (var organizationUnit in organizationUnitList)
            {
                lookupTableDtoList.Add(new DepartmentOrganizationUnitLookupTableDto
                {
                    Id = organizationUnit.Id,
                    DisplayName = organizationUnit.DisplayName?.ToString() + "  [" + organizationUnit.DepartmentCode + "]"
                });
            }

            return new PagedResultDto<DepartmentOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}