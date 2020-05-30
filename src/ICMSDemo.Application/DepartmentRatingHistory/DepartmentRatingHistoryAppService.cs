using Abp.Organizations;
using ICMSDemo.Ratings;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.DepartmentRatingHistory.Exporting;
using ICMSDemo.DepartmentRatingHistory.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ICMSDemo.DepartmentRatingHistory
{
	[AbpAuthorize(AppPermissions.Pages_DepartmentRatingHistory)]
    public class DepartmentRatingHistoryAppService : ICMSDemoAppServiceBase, IDepartmentRatingHistoryAppService
    {
		 private readonly IRepository<DepartmentRating> _departmentRatingRepository;
		 private readonly IDepartmentRatingHistoryExcelExporter _departmentRatingHistoryExcelExporter;
		 private readonly IRepository<OrganizationUnit,long> _lookup_organizationUnitRepository;
		 private readonly IRepository<Rating,int> _lookup_ratingRepository;
		 

		  public DepartmentRatingHistoryAppService(IRepository<DepartmentRating> departmentRatingRepository, IDepartmentRatingHistoryExcelExporter departmentRatingHistoryExcelExporter , IRepository<OrganizationUnit, long> lookup_organizationUnitRepository, IRepository<Rating, int> lookup_ratingRepository) 
		  {
			_departmentRatingRepository = departmentRatingRepository;
			_departmentRatingHistoryExcelExporter = departmentRatingHistoryExcelExporter;
			_lookup_organizationUnitRepository = lookup_organizationUnitRepository;
		_lookup_ratingRepository = lookup_ratingRepository;
		
		  }

		 public async Task<PagedResultDto<GetDepartmentRatingForViewDto>> GetAll(GetAllDepartmentRatingHistoryInput input)
         {
			
			var filteredDepartmentRatingHistory = _departmentRatingRepository.GetAll()
						.Include( e => e.OrganizationUnitFk)
						.Include( e => e.RatingFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ChangeType.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RatingNameFilter), e => e.RatingFk != null && e.RatingFk.Name == input.RatingNameFilter);

			var pagedAndFilteredDepartmentRatingHistory = filteredDepartmentRatingHistory
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var departmentRatingHistory = from o in pagedAndFilteredDepartmentRatingHistory
                         join o1 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_ratingRepository.GetAll() on o.RatingId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetDepartmentRatingForViewDto() {
							DepartmentRating = new DepartmentRatingDto
							{
                                RatingDate = o.RatingDate,
                                ChangeType = o.ChangeType,
                                Id = o.Id
							},
                         	OrganizationUnitDisplayName = s1 == null ? "" : s1.DisplayName.ToString(),
                         	RatingName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredDepartmentRatingHistory.CountAsync();

            return new PagedResultDto<GetDepartmentRatingForViewDto>(
                totalCount,
                await departmentRatingHistory.ToListAsync()
            );
         }
		 
		 public async Task<GetDepartmentRatingForViewDto> GetDepartmentRatingForView(int id)
         {
            var departmentRating = await _departmentRatingRepository.GetAsync(id);

            var output = new GetDepartmentRatingForViewDto { DepartmentRating = ObjectMapper.Map<DepartmentRatingDto>(departmentRating) };

		    if (output.DepartmentRating.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.DepartmentRating.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

		    if (output.DepartmentRating.RatingId != null)
            {
                var _lookupRating = await _lookup_ratingRepository.FirstOrDefaultAsync((int)output.DepartmentRating.RatingId);
                output.RatingName = _lookupRating.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_DepartmentRatingHistory_Edit)]
		 public async Task<GetDepartmentRatingForEditOutput> GetDepartmentRatingForEdit(EntityDto input)
         {
            var departmentRating = await _departmentRatingRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetDepartmentRatingForEditOutput {DepartmentRating = ObjectMapper.Map<CreateOrEditDepartmentRatingDto>(departmentRating)};

		    if (output.DepartmentRating.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.DepartmentRating.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }

		    if (output.DepartmentRating.RatingId != null)
            {
                var _lookupRating = await _lookup_ratingRepository.FirstOrDefaultAsync((int)output.DepartmentRating.RatingId);
                output.RatingName = _lookupRating.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditDepartmentRatingDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_DepartmentRatingHistory_Create)]
		 protected virtual async Task Create(CreateOrEditDepartmentRatingDto input)
         {
            var departmentRating = ObjectMapper.Map<DepartmentRating>(input);

			
			if (AbpSession.TenantId != null)
			{
				departmentRating.TenantId = (int) AbpSession.TenantId;
			}
		

            await _departmentRatingRepository.InsertAsync(departmentRating);
         }

		 [AbpAuthorize(AppPermissions.Pages_DepartmentRatingHistory_Edit)]
		 protected virtual async Task Update(CreateOrEditDepartmentRatingDto input)
         {
            var departmentRating = await _departmentRatingRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, departmentRating);
         }

		 [AbpAuthorize(AppPermissions.Pages_DepartmentRatingHistory_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _departmentRatingRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetDepartmentRatingHistoryToExcel(GetAllDepartmentRatingHistoryForExcelInput input)
         {
			
			var filteredDepartmentRatingHistory = _departmentRatingRepository.GetAll()
						.Include( e => e.OrganizationUnitFk)
						.Include( e => e.RatingFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ChangeType.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RatingNameFilter), e => e.RatingFk != null && e.RatingFk.Name == input.RatingNameFilter);

			var query = (from o in filteredDepartmentRatingHistory
                         join o1 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_ratingRepository.GetAll() on o.RatingId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetDepartmentRatingForViewDto() { 
							DepartmentRating = new DepartmentRatingDto
							{
                                RatingDate = o.RatingDate,
                                ChangeType = o.ChangeType,
                                Id = o.Id
							},
                         	OrganizationUnitDisplayName = s1 == null ? "" : s1.DisplayName.ToString(),
                         	RatingName = s2 == null ? "" : s2.Name.ToString()
						 });


            var departmentRatingListDtos = await query.ToListAsync();

            return _departmentRatingHistoryExcelExporter.ExportToFile(departmentRatingListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_DepartmentRatingHistory)]
         public async Task<PagedResultDto<DepartmentRatingOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_organizationUnitRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.DisplayName != null && e.DisplayName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<DepartmentRatingOrganizationUnitLookupTableDto>();
			foreach(var organizationUnit in organizationUnitList){
				lookupTableDtoList.Add(new DepartmentRatingOrganizationUnitLookupTableDto
				{
					Id = organizationUnit.Id,
					DisplayName = organizationUnit.DisplayName?.ToString()
				});
			}

            return new PagedResultDto<DepartmentRatingOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_DepartmentRatingHistory)]
         public async Task<PagedResultDto<DepartmentRatingRatingLookupTableDto>> GetAllRatingForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_ratingRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var ratingList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<DepartmentRatingRatingLookupTableDto>();
			foreach(var rating in ratingList){
				lookupTableDtoList.Add(new DepartmentRatingRatingLookupTableDto
				{
					Id = rating.Id,
					DisplayName = rating.Name?.ToString()
				});
			}

            return new PagedResultDto<DepartmentRatingRatingLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}