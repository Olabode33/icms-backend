using ICMSDemo.ExceptionTypes;

using ICMSDemo;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.ExceptionTypeColumns.Exporting;
using ICMSDemo.ExceptionTypeColumns.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ICMSDemo.ExceptionTypeColumns
{
	[AbpAuthorize(AppPermissions.Pages_ExceptionTypeColumns)]
    public class ExceptionTypeColumnsAppService : ICMSDemoAppServiceBase, IExceptionTypeColumnsAppService
    {
		 private readonly IRepository<ExceptionTypeColumn> _exceptionTypeColumnRepository;
		 private readonly IExceptionTypeColumnsExcelExporter _exceptionTypeColumnsExcelExporter;
		 private readonly IRepository<ExceptionType,int> _lookup_exceptionTypeRepository;
		 

		  public ExceptionTypeColumnsAppService(IRepository<ExceptionTypeColumn> exceptionTypeColumnRepository, IExceptionTypeColumnsExcelExporter exceptionTypeColumnsExcelExporter , IRepository<ExceptionType, int> lookup_exceptionTypeRepository) 
		  {
			_exceptionTypeColumnRepository = exceptionTypeColumnRepository;
			_exceptionTypeColumnsExcelExporter = exceptionTypeColumnsExcelExporter;
			_lookup_exceptionTypeRepository = lookup_exceptionTypeRepository;
		
		  }

		 public async Task<PagedResultDto<GetExceptionTypeColumnForViewDto>> GetAll(GetAllExceptionTypeColumnsInput input)
         {
			var dataTypeFilter = (DataTypes) input.DataTypeFilter;
			
			var filteredExceptionTypeColumns = _exceptionTypeColumnRepository.GetAll()
						.Include( e => e.ExceptionTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.DataTypeFilter > -1, e => e.DataType == dataTypeFilter)
						.WhereIf(input.RequiredFilter > -1,  e => (input.RequiredFilter == 1 && e.Required) || (input.RequiredFilter == 0 && !e.Required) )
						.WhereIf(input.MinMinimumFilter != null, e => e.Minimum >= input.MinMinimumFilter)
						.WhereIf(input.MaxMinimumFilter != null, e => e.Minimum <= input.MaxMinimumFilter)
						.WhereIf(input.MinMaximumFilter != null, e => e.Maximum >= input.MinMaximumFilter)
						.WhereIf(input.MaxMaximumFilter != null, e => e.Maximum <= input.MaxMaximumFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ExceptionTypeNameFilter), e => e.ExceptionTypeFk != null && e.ExceptionTypeFk.Name == input.ExceptionTypeNameFilter);

			var pagedAndFilteredExceptionTypeColumns = filteredExceptionTypeColumns
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var exceptionTypeColumns = from o in pagedAndFilteredExceptionTypeColumns
                         join o1 in _lookup_exceptionTypeRepository.GetAll() on o.ExceptionTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetExceptionTypeColumnForViewDto() {
							ExceptionTypeColumn = new ExceptionTypeColumnDto
							{
                                Name = o.Name,
                                DataType = o.DataType,
                                Required = o.Required,
                                Minimum = o.Minimum,
                                Maximum = o.Maximum,
                                Id = o.Id
							},
                         	ExceptionTypeName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredExceptionTypeColumns.CountAsync();

            return new PagedResultDto<GetExceptionTypeColumnForViewDto>(
                totalCount,
                await exceptionTypeColumns.ToListAsync()
            );
         }
		 
		 public async Task<GetExceptionTypeColumnForViewDto> GetExceptionTypeColumnForView(int id)
         {
            var exceptionTypeColumn = await _exceptionTypeColumnRepository.GetAsync(id);

            var output = new GetExceptionTypeColumnForViewDto { ExceptionTypeColumn = ObjectMapper.Map<ExceptionTypeColumnDto>(exceptionTypeColumn) };

		    if (output.ExceptionTypeColumn.ExceptionTypeId != null)
            {
                var _lookupExceptionType = await _lookup_exceptionTypeRepository.FirstOrDefaultAsync((int)output.ExceptionTypeColumn.ExceptionTypeId);
                output.ExceptionTypeName = _lookupExceptionType.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ExceptionTypeColumns_Edit)]
		 public async Task<GetExceptionTypeColumnForEditOutput> GetExceptionTypeColumnForEdit(EntityDto input)
         {
            var exceptionTypeColumn = await _exceptionTypeColumnRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetExceptionTypeColumnForEditOutput {ExceptionTypeColumn = ObjectMapper.Map<CreateOrEditExceptionTypeColumnDto>(exceptionTypeColumn)};

		    if (output.ExceptionTypeColumn.ExceptionTypeId != null)
            {
                var _lookupExceptionType = await _lookup_exceptionTypeRepository.FirstOrDefaultAsync((int)output.ExceptionTypeColumn.ExceptionTypeId);
                output.ExceptionTypeName = _lookupExceptionType.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditExceptionTypeColumnDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ExceptionTypeColumns_Create)]
		 protected virtual async Task Create(CreateOrEditExceptionTypeColumnDto input)
         {
            var exceptionTypeColumn = ObjectMapper.Map<ExceptionTypeColumn>(input);

			
			if (AbpSession.TenantId != null)
			{
				exceptionTypeColumn.TenantId = (int) AbpSession.TenantId;
			}
		

            await _exceptionTypeColumnRepository.InsertAsync(exceptionTypeColumn);
         }

		 [AbpAuthorize(AppPermissions.Pages_ExceptionTypeColumns_Edit)]
		 protected virtual async Task Update(CreateOrEditExceptionTypeColumnDto input)
         {
            var exceptionTypeColumn = await _exceptionTypeColumnRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, exceptionTypeColumn);
         }

		 [AbpAuthorize(AppPermissions.Pages_ExceptionTypeColumns_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _exceptionTypeColumnRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetExceptionTypeColumnsToExcel(GetAllExceptionTypeColumnsForExcelInput input)
         {
			var dataTypeFilter = (DataTypes) input.DataTypeFilter;
			
			var filteredExceptionTypeColumns = _exceptionTypeColumnRepository.GetAll()
						.Include( e => e.ExceptionTypeFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.DataTypeFilter > -1, e => e.DataType == dataTypeFilter)
						.WhereIf(input.RequiredFilter > -1,  e => (input.RequiredFilter == 1 && e.Required) || (input.RequiredFilter == 0 && !e.Required) )
						.WhereIf(input.MinMinimumFilter != null, e => e.Minimum >= input.MinMinimumFilter)
						.WhereIf(input.MaxMinimumFilter != null, e => e.Minimum <= input.MaxMinimumFilter)
						.WhereIf(input.MinMaximumFilter != null, e => e.Maximum >= input.MinMaximumFilter)
						.WhereIf(input.MaxMaximumFilter != null, e => e.Maximum <= input.MaxMaximumFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ExceptionTypeNameFilter), e => e.ExceptionTypeFk != null && e.ExceptionTypeFk.Name == input.ExceptionTypeNameFilter);

			var query = (from o in filteredExceptionTypeColumns
                         join o1 in _lookup_exceptionTypeRepository.GetAll() on o.ExceptionTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetExceptionTypeColumnForViewDto() { 
							ExceptionTypeColumn = new ExceptionTypeColumnDto
							{
                                Name = o.Name,
                                DataType = o.DataType,
                                Required = o.Required,
                                Minimum = o.Minimum,
                                Maximum = o.Maximum,
                                Id = o.Id
							},
                         	ExceptionTypeName = s1 == null ? "" : s1.Name.ToString()
						 });


            var exceptionTypeColumnListDtos = await query.ToListAsync();

            return _exceptionTypeColumnsExcelExporter.ExportToFile(exceptionTypeColumnListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ExceptionTypeColumns)]
         public async Task<PagedResultDto<ExceptionTypeColumnExceptionTypeLookupTableDto>> GetAllExceptionTypeForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_exceptionTypeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var exceptionTypeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ExceptionTypeColumnExceptionTypeLookupTableDto>();
			foreach(var exceptionType in exceptionTypeList){
				lookupTableDtoList.Add(new ExceptionTypeColumnExceptionTypeLookupTableDto
				{
					Id = exceptionType.Id,
					DisplayName = exceptionType.Name?.ToString()
				});
			}

            return new PagedResultDto<ExceptionTypeColumnExceptionTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}