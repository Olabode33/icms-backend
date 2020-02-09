using ICMSDemo.Departments;
using ICMSDemo.Risks;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.DepartmentRisks.Exporting;
using ICMSDemo.DepartmentRisks.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Organizations;

namespace ICMSDemo.DepartmentRisks
{
	[AbpAuthorize(AppPermissions.Pages_DepartmentRisks)]
    public class DepartmentRisksAppService : ICMSDemoAppServiceBase, IDepartmentRisksAppService
    {
		 private readonly IRepository<DepartmentRisk> _departmentRiskRepository;
		 private readonly IDepartmentRisksExcelExporter _departmentRisksExcelExporter;
		 private readonly IRepository<Department,long> _lookup_departmentRepository;
		 private readonly IRepository<Risk,int> _lookup_riskRepository;
		 

		  public DepartmentRisksAppService(IRepository<DepartmentRisk> departmentRiskRepository, IDepartmentRisksExcelExporter departmentRisksExcelExporter , IRepository<Department, long> lookup_departmentRepository, IRepository<Risk, int> lookup_riskRepository) 
		  {
			_departmentRiskRepository = departmentRiskRepository;
			_departmentRisksExcelExporter = departmentRisksExcelExporter;
			_lookup_departmentRepository = lookup_departmentRepository;
		_lookup_riskRepository = lookup_riskRepository;
		
		  }

		 public async Task<PagedResultDto<GetDepartmentRiskForViewDto>> GetAll(GetAllDepartmentRisksInput input)
         {
			
			var filteredDepartmentRisks = _departmentRiskRepository.GetAll()
						.Include( e => e.DepartmentFk)
						.Include( e => e.RiskFk)

						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Comments.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DepartmentNameFilter), e => e.DepartmentFk != null && e.DepartmentFk.Name == input.DepartmentNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RiskNameFilter), e => e.RiskFk != null && e.RiskFk.Name == input.RiskNameFilter);

			var pagedAndFilteredDepartmentRisks = filteredDepartmentRisks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var departmentRisks = from o in pagedAndFilteredDepartmentRisks
                         join o1 in _lookup_departmentRepository.GetAll() on o.DepartmentId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_riskRepository.GetAll() on o.RiskId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetDepartmentRiskForViewDto() {
							DepartmentRisk = new DepartmentRiskDto
							{
                                Code = o.Code,
                                Comments = o.Comments,
                                Id = o.Id,
                                Cascade = o.Cascade
							},
                         	DepartmentName = s1 == null ? "" : s1.Name.ToString(),
                         	RiskName = s2 == null ? "" : s2.Name.ToString(),
                            Severity = s2 == null ? "" : s2.Severity.ToString()
						};

            var totalCount = await filteredDepartmentRisks.CountAsync();

            return new PagedResultDto<GetDepartmentRiskForViewDto>(
                totalCount,
                await departmentRisks.ToListAsync()
            );
         }


        public async Task<PagedResultDto<GetDepartmentRiskForViewDto>> GetRiskForDepartment(GetAllDepartmentRisksInput input)
        {
           var departmentCode = await OrganizationUnitManager.GetCodeAsync((long)input.DepartmentId);

            string[] roots = departmentCode.Split(".");
            string previousCode = string.Empty;
            List<string> codes = new List<string>();

            foreach (var item in roots)
            {
                previousCode = previousCode == string.Empty ? item : previousCode + "." + item;
                codes.Add(previousCode);
            }

           var departments =  await _lookup_departmentRepository.GetAllListAsync(x => codes.Any(e => e == x.Code));


            var filteredDepartmentRisks = from o in _departmentRiskRepository
                                          .GetAll()
                                          .Include(e => e.RiskFk)
                                          .Include(x => x.DepartmentFk)
                                          .Where(x => x.DepartmentId == input.DepartmentId || (x.DepartmentId != input.DepartmentId && x.Cascade))
                                          select new GetDepartmentRiskForViewDto()
                                          {
                                              DepartmentRisk = new DepartmentRiskDto
                                              {
                                                  DepartmentId = (int)o.DepartmentId,
                                                  DeptCode = o.DepartmentFk.Code,
                                                  Code = o.Code,
                                                  Comments = o.Comments,
                                                  Id = o.Id,
                                                  Cascade = o.Cascade,
                                                  Inherited = o.DepartmentId == input.DepartmentId ? false : true
                                              },
                                              DepartmentName = o.DepartmentFk.Name.ToString(),
                                              RiskName = o.RiskFk.Name.ToString(),
                                              Severity = o.RiskFk.Severity.ToString()
                                          } ;
            

            //var pagedAndFilteredDepartmentRisks = filteredDepartmentRisks
            //    .PageBy(input);

          //  var totalCount = await filteredDepartmentRisks.CountAsync();

            var lists = await filteredDepartmentRisks.ToListAsync();

            //fix later 
            lists = lists.Where(x => codes.Any(e => e == x.DepartmentRisk.DeptCode)).ToList();

            var totalCount = lists.Count;

            var output = lists.Skip(input.SkipCount).Take(input.MaxResultCount);

            return new PagedResultDto<GetDepartmentRiskForViewDto>(totalCount, lists);
        }



        public async Task<GetDepartmentRiskForViewDto> GetDepartmentRiskForView(int id)
         {
            var departmentRisk = await _departmentRiskRepository.GetAsync(id);

            var output = new GetDepartmentRiskForViewDto { DepartmentRisk = ObjectMapper.Map<DepartmentRiskDto>(departmentRisk) };

		    if (output.DepartmentRisk.DepartmentId != null)
            {
                var _lookupDepartment = await _lookup_departmentRepository.FirstOrDefaultAsync((int)output.DepartmentRisk.DepartmentId);
                output.DepartmentName = _lookupDepartment.Name.ToString();
            }

		    if (output.DepartmentRisk.RiskId != null)
            {
                var _lookupRisk = await _lookup_riskRepository.FirstOrDefaultAsync((int)output.DepartmentRisk.RiskId);
                output.RiskName = _lookupRisk.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_DepartmentRisks_Edit)]
		 public async Task<GetDepartmentRiskForEditOutput> GetDepartmentRiskForEdit(EntityDto input)
         {
            var departmentRisk = await _departmentRiskRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetDepartmentRiskForEditOutput {DepartmentRisk = ObjectMapper.Map<CreateOrEditDepartmentRiskDto>(departmentRisk)};

		    if (output.DepartmentRisk.DepartmentId != null)
            {
                var _lookupDepartment = await _lookup_departmentRepository.FirstOrDefaultAsync((int)output.DepartmentRisk.DepartmentId);
                output.DepartmentName = _lookupDepartment.Name.ToString();
            }

		    if (output.DepartmentRisk.RiskId != null)
            {
                var _lookupRisk = await _lookup_riskRepository.FirstOrDefaultAsync((int)output.DepartmentRisk.RiskId);
                output.RiskName = _lookupRisk.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditDepartmentRiskDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_DepartmentRisks_Create)]
		 protected virtual async Task Create(CreateOrEditDepartmentRiskDto input)
         {
            var departmentRisk = ObjectMapper.Map<DepartmentRisk>(input);
            var previousCount = await _departmentRiskRepository.CountAsync(x => x.DepartmentId == input.DepartmentId);
            previousCount++;
            departmentRisk.Code = "DR-" + previousCount.ToString();

            if (AbpSession.TenantId != null)
			{
				departmentRisk.TenantId = (int) AbpSession.TenantId;
			}
		

            await _departmentRiskRepository.InsertAsync(departmentRisk);
         }

		 [AbpAuthorize(AppPermissions.Pages_DepartmentRisks_Edit)]
		 protected virtual async Task Update(CreateOrEditDepartmentRiskDto input)
         {
            var departmentRisk = await _departmentRiskRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, departmentRisk);
         }

		 [AbpAuthorize(AppPermissions.Pages_DepartmentRisks_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _departmentRiskRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetDepartmentRisksToExcel(GetAllDepartmentRisksForExcelInput input)
         {
			
			var filteredDepartmentRisks = _departmentRiskRepository.GetAll()
						.Include( e => e.DepartmentFk)
						.Include( e => e.RiskFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Comments.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DepartmentNameFilter), e => e.DepartmentFk != null && e.DepartmentFk.Name == input.DepartmentNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RiskNameFilter), e => e.RiskFk != null && e.RiskFk.Name == input.RiskNameFilter);

			var query = (from o in filteredDepartmentRisks
                         join o1 in _lookup_departmentRepository.GetAll() on o.DepartmentId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_riskRepository.GetAll() on o.RiskId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetDepartmentRiskForViewDto() { 
							DepartmentRisk = new DepartmentRiskDto
							{
                                Code = o.Code,
                                Comments = o.Comments,
                                Id = o.Id
							},
                         	DepartmentName = s1 == null ? "" : s1.Name.ToString(),
                         	RiskName = s2 == null ? "" : s2.Name.ToString()
						 });


            var departmentRiskListDtos = await query.ToListAsync();

            return _departmentRisksExcelExporter.ExportToFile(departmentRiskListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_DepartmentRisks)]
         public async Task<PagedResultDto<DepartmentRiskDepartmentLookupTableDto>> GetAllDepartmentForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_departmentRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var departmentList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<DepartmentRiskDepartmentLookupTableDto>();
			foreach(var department in departmentList){
				lookupTableDtoList.Add(new DepartmentRiskDepartmentLookupTableDto
				{
					Id = department.Id,
					DisplayName = department.Name?.ToString()
				});
			}

            return new PagedResultDto<DepartmentRiskDepartmentLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_DepartmentRisks)]
         public async Task<PagedResultDto<DepartmentRiskRiskLookupTableDto>> GetAllRiskForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_riskRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var riskList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<DepartmentRiskRiskLookupTableDto>();
			foreach(var risk in riskList){
				lookupTableDtoList.Add(new DepartmentRiskRiskLookupTableDto
				{
					Id = risk.Id,
					DisplayName = risk.Name?.ToString()
				});
			}

            return new PagedResultDto<DepartmentRiskRiskLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}