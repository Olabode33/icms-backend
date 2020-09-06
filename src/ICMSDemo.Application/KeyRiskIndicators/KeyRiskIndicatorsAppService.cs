using ICMSDemo.ExceptionTypes;
using ICMSDemo.Authorization.Users;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.KeyRiskIndicators.Exporting;
using ICMSDemo.KeyRiskIndicators.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ICMSDemo.Risks;
using ICMSDemo.BusinessObjectives;

namespace ICMSDemo.KeyRiskIndicators
{
	[AbpAuthorize(AppPermissions.Pages_KeyRiskIndicators)]
    public class KeyRiskIndicatorsAppService : ICMSDemoAppServiceBase, IKeyRiskIndicatorsAppService
    {
		 private readonly IRepository<KeyRiskIndicator> _keyRiskIndicatorRepository;
		 private readonly IKeyRiskIndicatorsExcelExporter _keyRiskIndicatorsExcelExporter;
		 private readonly IRepository<ExceptionType,int> _lookup_exceptionTypeRepository;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<Risk> _lookup_riskRepository;
		 private readonly IRepository<BusinessObjective> _lookup_businessObjectiveRepository;
		 

		  public KeyRiskIndicatorsAppService(
              IRepository<KeyRiskIndicator> keyRiskIndicatorRepository, IKeyRiskIndicatorsExcelExporter keyRiskIndicatorsExcelExporter , IRepository<ExceptionType, int> lookup_exceptionTypeRepository,
              IRepository<User, long> lookup_userRepository, 
              IRepository<Risk> lookup_riskRepository, IRepository<BusinessObjective> lookup_businessObjectiveRepository
              ) 
		  {
			_keyRiskIndicatorRepository = keyRiskIndicatorRepository;
			_keyRiskIndicatorsExcelExporter = keyRiskIndicatorsExcelExporter;
			_lookup_exceptionTypeRepository = lookup_exceptionTypeRepository;
		_lookup_userRepository = lookup_userRepository;
            _lookup_riskRepository = lookup_riskRepository;
            _lookup_businessObjectiveRepository = lookup_businessObjectiveRepository;
          }

		 public async Task<PagedResultDto<GetKeyRiskIndicatorForViewDto>> GetAll(GetAllKeyRiskIndicatorsInput input)
         {
			
			var filteredKeyRiskIndicators = _keyRiskIndicatorRepository.GetAll()
						.Include( e => e.ExceptionTypeFk)
						.Include( e => e.UserFk)
                        .Include( e => e.RiskFk)
                        .Include( e => e.BusinessObjectiveFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Nature.Contains(input.Filter) || e.LowActionType.Contains(input.Filter) || e.MediumActionType.Contains(input.Filter) || e.HighActionType.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NatureFilter),  e => e.Nature == input.NatureFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ExceptionTypeCodeFilter), e => e.ExceptionTypeFk != null && e.ExceptionTypeFk.Code == input.ExceptionTypeCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var pagedAndFilteredKeyRiskIndicators = filteredKeyRiskIndicators
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var keyRiskIndicators = from o in pagedAndFilteredKeyRiskIndicators
                                    join o1 in _lookup_exceptionTypeRepository.GetAll() on o.ExceptionTypeId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()

                                    select new GetKeyRiskIndicatorForViewDto() {
                                        KeyRiskIndicator = new KeyRiskIndicatorDto
                                        {
                                            Name = o.Name,
                                            Nature = o.Nature,
                                            LowLevel = o.LowLevel,
                                            Id = o.Id
                                        },
                                        ExceptionTypeCode = s1 == null || s1.Code == null ? "" : s1.Code.ToString(),
                                        UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                        RiskName = o.RiskFk != null ? o.RiskFk.Name : "",
                                        BusinessObjectiveName = o.BusinessObjectiveFk != null ? o.BusinessObjectiveFk.Name : ""
						};

            var totalCount = await filteredKeyRiskIndicators.CountAsync();

            return new PagedResultDto<GetKeyRiskIndicatorForViewDto>(
                totalCount,
                await keyRiskIndicators.ToListAsync()
            );
         }
		 
		 public async Task<GetKeyRiskIndicatorForViewDto> GetKeyRiskIndicatorForView(int id)
         {
            var keyRiskIndicator = await _keyRiskIndicatorRepository.GetAsync(id);

            var output = new GetKeyRiskIndicatorForViewDto { KeyRiskIndicator = ObjectMapper.Map<KeyRiskIndicatorDto>(keyRiskIndicator) };

		    if (output.KeyRiskIndicator.ExceptionTypeId != null)
            {
                var _lookupExceptionType = await _lookup_exceptionTypeRepository.FirstOrDefaultAsync((int)output.KeyRiskIndicator.ExceptionTypeId);
                output.ExceptionTypeCode = _lookupExceptionType?.Code?.ToString();
            }

		    if (output.KeyRiskIndicator.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.KeyRiskIndicator.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }


            if (output.KeyRiskIndicator.RiskId != null)
            {
                var _lookupUser = await _lookup_riskRepository.FirstOrDefaultAsync((int)output.KeyRiskIndicator.RiskId);
                output.RiskName = _lookupUser?.Name?.ToString();
            }
            if (output.KeyRiskIndicator.BusinessObjectiveId != null)
            {
                var _lookupUser = await _lookup_businessObjectiveRepository.FirstOrDefaultAsync((int)output.KeyRiskIndicator.BusinessObjectiveId);
                output.BusinessObjectiveName = _lookupUser?.Name?.ToString();
            }

            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_KeyRiskIndicators_Edit)]
		 public async Task<GetKeyRiskIndicatorForEditOutput> GetKeyRiskIndicatorForEdit(EntityDto input)
         {
            var keyRiskIndicator = await _keyRiskIndicatorRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetKeyRiskIndicatorForEditOutput {KeyRiskIndicator = ObjectMapper.Map<CreateOrEditKeyRiskIndicatorDto>(keyRiskIndicator)};

		    if (output.KeyRiskIndicator.ExceptionTypeId != null)
            {
                var _lookupExceptionType = await _lookup_exceptionTypeRepository.FirstOrDefaultAsync((int)output.KeyRiskIndicator.ExceptionTypeId);
                output.ExceptionTypeCode = _lookupExceptionType?.Code?.ToString();
            }

		    if (output.KeyRiskIndicator.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.KeyRiskIndicator.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }


            if (output.KeyRiskIndicator.RiskId != null)
            {
                var _lookupUser = await _lookup_riskRepository.FirstOrDefaultAsync((int)output.KeyRiskIndicator.RiskId);
                output.RiskName = _lookupUser?.Name?.ToString();
            }

            if (output.KeyRiskIndicator.BusinessObjectiveId != null)
            {
                var _lookupUser = await _lookup_businessObjectiveRepository.FirstOrDefaultAsync((int)output.KeyRiskIndicator.BusinessObjectiveId);
                output.BusinessObjectiveName = _lookupUser?.Name?.ToString();
            }

            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditKeyRiskIndicatorDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_KeyRiskIndicators_Create)]
		 protected virtual async Task Create(CreateOrEditKeyRiskIndicatorDto input)
         {
            var keyRiskIndicator = ObjectMapper.Map<KeyRiskIndicator>(input);

			
			if (AbpSession.TenantId != null)
			{
				keyRiskIndicator.TenantId = (int) AbpSession.TenantId;
			}
		

            await _keyRiskIndicatorRepository.InsertAsync(keyRiskIndicator);
         }

		 [AbpAuthorize(AppPermissions.Pages_KeyRiskIndicators_Edit)]
		 protected virtual async Task Update(CreateOrEditKeyRiskIndicatorDto input)
         {
            var keyRiskIndicator = await _keyRiskIndicatorRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, keyRiskIndicator);
         }

		 [AbpAuthorize(AppPermissions.Pages_KeyRiskIndicators_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _keyRiskIndicatorRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetKeyRiskIndicatorsToExcel(GetAllKeyRiskIndicatorsForExcelInput input)
         {
			
			var filteredKeyRiskIndicators = _keyRiskIndicatorRepository.GetAll()
						.Include( e => e.ExceptionTypeFk)
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Nature.Contains(input.Filter) || e.LowActionType.Contains(input.Filter) || e.MediumActionType.Contains(input.Filter) || e.HighActionType.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NatureFilter),  e => e.Nature == input.NatureFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ExceptionTypeCodeFilter), e => e.ExceptionTypeFk != null && e.ExceptionTypeFk.Code == input.ExceptionTypeCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var query = (from o in filteredKeyRiskIndicators
                         join o1 in _lookup_exceptionTypeRepository.GetAll() on o.ExceptionTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetKeyRiskIndicatorForViewDto() { 
							KeyRiskIndicator = new KeyRiskIndicatorDto
							{
                                Name = o.Name,
                                Nature = o.Nature,
                                LowLevel = o.LowLevel,
                                Id = o.Id
							},
                         	ExceptionTypeCode = s1 == null || s1.Code == null ? "" : s1.Code.ToString(),
                         	UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						 });


            var keyRiskIndicatorListDtos = await query.ToListAsync();

            return _keyRiskIndicatorsExcelExporter.ExportToFile(keyRiskIndicatorListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_KeyRiskIndicators)]
         public async Task<PagedResultDto<KeyRiskIndicatorExceptionTypeLookupTableDto>> GetAllExceptionTypeForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_exceptionTypeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Code != null && e.Code.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var exceptionTypeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<KeyRiskIndicatorExceptionTypeLookupTableDto>();
			foreach(var exceptionType in exceptionTypeList){
				lookupTableDtoList.Add(new KeyRiskIndicatorExceptionTypeLookupTableDto
				{
					Id = exceptionType.Id,
					DisplayName = exceptionType.Name + " [" + exceptionType.Code?.ToString()  + "]"
				});
			}

            return new PagedResultDto<KeyRiskIndicatorExceptionTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_KeyRiskIndicators)]
         public async Task<PagedResultDto<KeyRiskIndicatorUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<KeyRiskIndicatorUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new KeyRiskIndicatorUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<KeyRiskIndicatorUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

        public async Task<PagedResultDto<KeyRiskIndicatorUserLookupTableDto>> GetAllBusinessObjectiveForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_businessObjectiveRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<KeyRiskIndicatorUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new KeyRiskIndicatorUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<KeyRiskIndicatorUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}