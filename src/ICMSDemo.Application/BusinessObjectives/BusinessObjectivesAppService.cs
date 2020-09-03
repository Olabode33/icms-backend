using ICMSDemo.Authorization.Users;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.BusinessObjectives.Exporting;
using ICMSDemo.BusinessObjectives.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ICMSDemo.BusinessObjectives
{
	[AbpAuthorize(AppPermissions.Pages_BusinessObjectives)]
    public class BusinessObjectivesAppService : ICMSDemoAppServiceBase, IBusinessObjectivesAppService
    {
		 private readonly IRepository<BusinessObjective> _businessObjectiveRepository;
		 private readonly IBusinessObjectivesExcelExporter _businessObjectivesExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public BusinessObjectivesAppService(IRepository<BusinessObjective> businessObjectiveRepository, IBusinessObjectivesExcelExporter businessObjectivesExcelExporter , IRepository<User, long> lookup_userRepository) 
		  {
			_businessObjectiveRepository = businessObjectiveRepository;
			_businessObjectivesExcelExporter = businessObjectivesExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetBusinessObjectiveForViewDto>> GetAll(GetAllBusinessObjectivesInput input)
         {
			
			var filteredBusinessObjectives = _businessObjectiveRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ObjectiveType.Contains(input.Filter))
						.WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
						.WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
						.WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
						.WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ObjectiveTypeFilter),  e => e.ObjectiveType == input.ObjectiveTypeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var pagedAndFilteredBusinessObjectives = filteredBusinessObjectives
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var businessObjectives = from o in pagedAndFilteredBusinessObjectives
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetBusinessObjectiveForViewDto() {
							BusinessObjective = new BusinessObjectiveDto
							{
                                Name = o.Name,
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                ObjectiveType = o.ObjectiveType,
                                Id = o.Id
							},
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredBusinessObjectives.CountAsync();

            return new PagedResultDto<GetBusinessObjectiveForViewDto>(
                totalCount,
                await businessObjectives.ToListAsync()
            );
         }
		 
		 public async Task<GetBusinessObjectiveForViewDto> GetBusinessObjectiveForView(int id)
         {
            var businessObjective = await _businessObjectiveRepository.GetAsync(id);

            var output = new GetBusinessObjectiveForViewDto { BusinessObjective = ObjectMapper.Map<BusinessObjectiveDto>(businessObjective) };

		    if (output.BusinessObjective.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.BusinessObjective.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_BusinessObjectives_Edit)]
		 public async Task<GetBusinessObjectiveForEditOutput> GetBusinessObjectiveForEdit(EntityDto input)
         {
            var businessObjective = await _businessObjectiveRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBusinessObjectiveForEditOutput {BusinessObjective = ObjectMapper.Map<CreateOrEditBusinessObjectiveDto>(businessObjective)};

		    if (output.BusinessObjective.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.BusinessObjective.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBusinessObjectiveDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_BusinessObjectives_Create)]
		 protected virtual async Task Create(CreateOrEditBusinessObjectiveDto input)
         {
            var businessObjective = ObjectMapper.Map<BusinessObjective>(input);

			
			if (AbpSession.TenantId != null)
			{
				businessObjective.TenantId = (int) AbpSession.TenantId;
			}
		

            await _businessObjectiveRepository.InsertAsync(businessObjective);
         }

		 [AbpAuthorize(AppPermissions.Pages_BusinessObjectives_Edit)]
		 protected virtual async Task Update(CreateOrEditBusinessObjectiveDto input)
         {
            var businessObjective = await _businessObjectiveRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, businessObjective);
         }

		 [AbpAuthorize(AppPermissions.Pages_BusinessObjectives_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _businessObjectiveRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBusinessObjectivesToExcel(GetAllBusinessObjectivesForExcelInput input)
         {
			
			var filteredBusinessObjectives = _businessObjectiveRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ObjectiveType.Contains(input.Filter))
						.WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
						.WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
						.WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
						.WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ObjectiveTypeFilter),  e => e.ObjectiveType == input.ObjectiveTypeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var query = (from o in filteredBusinessObjectives
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetBusinessObjectiveForViewDto() { 
							BusinessObjective = new BusinessObjectiveDto
							{
                                Name = o.Name,
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                ObjectiveType = o.ObjectiveType,
                                Id = o.Id
							},
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
						 });


            var businessObjectiveListDtos = await query.ToListAsync();

            return _businessObjectivesExcelExporter.ExportToFile(businessObjectiveListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_BusinessObjectives)]
         public async Task<PagedResultDto<BusinessObjectiveUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BusinessObjectiveUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new BusinessObjectiveUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<BusinessObjectiveUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}