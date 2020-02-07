
using ICMSDemo;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.ExceptionTypes.Exporting;
using ICMSDemo.ExceptionTypes.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ICMSDemo.ExceptionTypes
{
	[AbpAuthorize(AppPermissions.Pages_ExceptionTypes)]
    public class ExceptionTypesAppService : ICMSDemoAppServiceBase, IExceptionTypesAppService
    {
		 private readonly IRepository<ExceptionType> _exceptionTypeRepository;
		 private readonly IExceptionTypesExcelExporter _exceptionTypesExcelExporter;
		 

		  public ExceptionTypesAppService(IRepository<ExceptionType> exceptionTypeRepository, IExceptionTypesExcelExporter exceptionTypesExcelExporter ) 
		  {
			_exceptionTypeRepository = exceptionTypeRepository;
			_exceptionTypesExcelExporter = exceptionTypesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetExceptionTypeForViewDto>> GetAll(GetAllExceptionTypesInput input)
         {
			var severityFilter = (Severity) input.SeverityFilter;
			
			var filteredExceptionTypes = _exceptionTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.SeverityFilter > -1, e => e.Severity == severityFilter);

			var pagedAndFilteredExceptionTypes = filteredExceptionTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var exceptionTypes = from o in pagedAndFilteredExceptionTypes
                         select new GetExceptionTypeForViewDto() {
							ExceptionType = new ExceptionTypeDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Description = o.Description,
                                Severity = o.Severity,
                                TargetRemediation = o.TargetRemediation,
                                Id = o.Id
							}
						};

            var totalCount = await filteredExceptionTypes.CountAsync();

            return new PagedResultDto<GetExceptionTypeForViewDto>(
                totalCount,
                await exceptionTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetExceptionTypeForViewDto> GetExceptionTypeForView(int id)
         {
            var exceptionType = await _exceptionTypeRepository.GetAsync(id);

            var output = new GetExceptionTypeForViewDto { ExceptionType = ObjectMapper.Map<ExceptionTypeDto>(exceptionType) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ExceptionTypes_Edit)]
		 public async Task<GetExceptionTypeForEditOutput> GetExceptionTypeForEdit(EntityDto input)
         {
            var exceptionType = await _exceptionTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetExceptionTypeForEditOutput {ExceptionType = ObjectMapper.Map<CreateOrEditExceptionTypeDto>(exceptionType)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditExceptionTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ExceptionTypes_Create)]
		 protected virtual async Task Create(CreateOrEditExceptionTypeDto input)
         {
            var exceptionType = ObjectMapper.Map<ExceptionType>(input);

			
			if (AbpSession.TenantId != null)
			{
				exceptionType.TenantId = (int) AbpSession.TenantId;
			}
		

            await _exceptionTypeRepository.InsertAsync(exceptionType);
         }

		 [AbpAuthorize(AppPermissions.Pages_ExceptionTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditExceptionTypeDto input)
         {
            var exceptionType = await _exceptionTypeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, exceptionType);
         }

		 [AbpAuthorize(AppPermissions.Pages_ExceptionTypes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _exceptionTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetExceptionTypesToExcel(GetAllExceptionTypesForExcelInput input)
         {
			var severityFilter = (Severity) input.SeverityFilter;
			
			var filteredExceptionTypes = _exceptionTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.SeverityFilter > -1, e => e.Severity == severityFilter);

			var query = (from o in filteredExceptionTypes
                         select new GetExceptionTypeForViewDto() { 
							ExceptionType = new ExceptionTypeDto
							{
                                Code = o.Code,
                                Name = o.Name,
                                Description = o.Description,
                                Severity = o.Severity,
                                TargetRemediation = o.TargetRemediation,
                                Id = o.Id
							}
						 });


            var exceptionTypeListDtos = await query.ToListAsync();

            return _exceptionTypesExcelExporter.ExportToFile(exceptionTypeListDtos);
         }


    }
}