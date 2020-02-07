using ICMSDemo.ExceptionTypes;
using ICMSDemo.Authorization.Users;
using ICMSDemo.TestingTemplates;
using Abp.Organizations;

using ICMSDemo;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.ExceptionIncidents.Exporting;
using ICMSDemo.ExceptionIncidents.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ICMSDemo.ExceptionIncidents
{
	[AbpAuthorize(AppPermissions.Pages_ExceptionIncidents)]
    public class ExceptionIncidentsAppService : ICMSDemoAppServiceBase, IExceptionIncidentsAppService
    {
		 private readonly IRepository<ExceptionIncident> _exceptionIncidentRepository;
		 private readonly IExceptionIncidentsExcelExporter _exceptionIncidentsExcelExporter;
		 private readonly IRepository<ExceptionType,int> _lookup_exceptionTypeRepository;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<TestingTemplate,int> _lookup_testingTemplateRepository;
		 private readonly IRepository<OrganizationUnit,long> _lookup_organizationUnitRepository;
		 

		  public ExceptionIncidentsAppService(IRepository<ExceptionIncident> exceptionIncidentRepository, IExceptionIncidentsExcelExporter exceptionIncidentsExcelExporter , IRepository<ExceptionType, int> lookup_exceptionTypeRepository, IRepository<User, long> lookup_userRepository, IRepository<TestingTemplate, int> lookup_testingTemplateRepository, IRepository<OrganizationUnit, long> lookup_organizationUnitRepository) 
		  {
			_exceptionIncidentRepository = exceptionIncidentRepository;
			_exceptionIncidentsExcelExporter = exceptionIncidentsExcelExporter;
			_lookup_exceptionTypeRepository = lookup_exceptionTypeRepository;
		_lookup_userRepository = lookup_userRepository;
		_lookup_testingTemplateRepository = lookup_testingTemplateRepository;
		_lookup_organizationUnitRepository = lookup_organizationUnitRepository;
		
		  }

		 public async Task<PagedResultDto<GetExceptionIncidentForViewDto>> GetAll(GetAllExceptionIncidentsInput input)
         {
			var statusFilter = (Status) input.StatusFilter;
			
			var filteredExceptionIncidents = _exceptionIncidentRepository.GetAll()
						.Include( e => e.ExceptionTypeFk)
						.Include( e => e.RaisedByFk)
						.Include( e => e.TestingTemplateFk)
						.Include( e => e.OrganizationUnitFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ClosureComments.Contains(input.Filter) || e.RaisedByClosureComments.Contains(input.Filter))
						.WhereIf(input.MinDateFilter != null, e => e.Date >= input.MinDateFilter)
						.WhereIf(input.MaxDateFilter != null, e => e.Date <= input.MaxDateFilter)
						.WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter)
						.WhereIf(input.MinClosureDateFilter != null, e => e.ClosureDate >= input.MinClosureDateFilter)
						.WhereIf(input.MaxClosureDateFilter != null, e => e.ClosureDate <= input.MaxClosureDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ExceptionTypeNameFilter), e => e.ExceptionTypeFk != null && e.ExceptionTypeFk.Name == input.ExceptionTypeNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.RaisedByFk != null && e.RaisedByFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TestingTemplateCodeFilter), e => e.TestingTemplateFk != null && e.TestingTemplateFk.Code == input.TestingTemplateCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

			var pagedAndFilteredExceptionIncidents = filteredExceptionIncidents
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var exceptionIncidents = from o in pagedAndFilteredExceptionIncidents
                         join o1 in _lookup_exceptionTypeRepository.GetAll() on o.ExceptionTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.RaisedById equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_testingTemplateRepository.GetAll() on o.TestingTemplateId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         join o4 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         
                         select new GetExceptionIncidentForViewDto() {
							ExceptionIncident = new ExceptionIncidentDto
							{
                                Code = o.Code,
                                Date = o.Date,
                                Description = o.Description,
                                Status = o.Status,
                                ClosureDate = o.ClosureDate,
                                ClosureComments = o.ClosureComments,
                                RaisedByClosureComments = o.RaisedByClosureComments,
                                Id = o.Id
							},
                         	ExceptionTypeName = s1 == null ? "" : s1.Name.ToString(),
                         	UserName = s2 == null ? "" : s2.Name.ToString(),
                         	TestingTemplateCode = s3 == null ? "" : s3.Code.ToString(),
                         	OrganizationUnitDisplayName = s4 == null ? "" : s4.DisplayName.ToString()
						};

            var totalCount = await filteredExceptionIncidents.CountAsync();

            return new PagedResultDto<GetExceptionIncidentForViewDto>(
                totalCount,
                await exceptionIncidents.ToListAsync()
            );
         }
		 
		 public async Task<GetExceptionIncidentForViewDto> GetExceptionIncidentForView(int id)
         {
            var exceptionIncident = await _exceptionIncidentRepository.GetAsync(id);

            var output = new GetExceptionIncidentForViewDto { ExceptionIncident = ObjectMapper.Map<ExceptionIncidentDto>(exceptionIncident) };

		    if (output.ExceptionIncident.ExceptionTypeId != null)
            {
                var _lookupExceptionType = await _lookup_exceptionTypeRepository.FirstOrDefaultAsync((int)output.ExceptionIncident.ExceptionTypeId);
                output.ExceptionTypeName = _lookupExceptionType.Name.ToString();
            }

		    if (output.ExceptionIncident.RaisedById != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ExceptionIncident.RaisedById);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.ExceptionIncident.TestingTemplateId != null)
            {
                var _lookupTestingTemplate = await _lookup_testingTemplateRepository.FirstOrDefaultAsync((int)output.ExceptionIncident.TestingTemplateId);
                output.TestingTemplateCode = _lookupTestingTemplate.Code.ToString();
            }

		    if (output.ExceptionIncident.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.ExceptionIncident.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents_Edit)]
		 public async Task<GetExceptionIncidentForEditOutput> GetExceptionIncidentForEdit(EntityDto input)
         {
            var exceptionIncident = await _exceptionIncidentRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetExceptionIncidentForEditOutput {ExceptionIncident = ObjectMapper.Map<CreateOrEditExceptionIncidentDto>(exceptionIncident)};

		    if (output.ExceptionIncident.ExceptionTypeId != null)
            {
                var _lookupExceptionType = await _lookup_exceptionTypeRepository.FirstOrDefaultAsync((int)output.ExceptionIncident.ExceptionTypeId);
                output.ExceptionTypeName = _lookupExceptionType.Name.ToString();
            }

		    if (output.ExceptionIncident.RaisedById != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ExceptionIncident.RaisedById);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.ExceptionIncident.TestingTemplateId != null)
            {
                var _lookupTestingTemplate = await _lookup_testingTemplateRepository.FirstOrDefaultAsync((int)output.ExceptionIncident.TestingTemplateId);
                output.TestingTemplateCode = _lookupTestingTemplate.Code.ToString();
            }

		    if (output.ExceptionIncident.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.ExceptionIncident.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit.DisplayName.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditExceptionIncidentDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents_Create)]
		 protected virtual async Task Create(CreateOrEditExceptionIncidentDto input)
         {
            var exceptionIncident = ObjectMapper.Map<ExceptionIncident>(input);

			
			if (AbpSession.TenantId != null)
			{
				exceptionIncident.TenantId = (int) AbpSession.TenantId;
			}
		

            await _exceptionIncidentRepository.InsertAsync(exceptionIncident);
         }

		 [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents_Edit)]
		 protected virtual async Task Update(CreateOrEditExceptionIncidentDto input)
         {
            var exceptionIncident = await _exceptionIncidentRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, exceptionIncident);
         }

		 [AbpAuthorize(AppPermissions.Pages_ExceptionIncidents_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _exceptionIncidentRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetExceptionIncidentsToExcel(GetAllExceptionIncidentsForExcelInput input)
         {
			var statusFilter = (Status) input.StatusFilter;
			
			var filteredExceptionIncidents = _exceptionIncidentRepository.GetAll()
						.Include( e => e.ExceptionTypeFk)
						.Include( e => e.RaisedByFk)
						.Include( e => e.TestingTemplateFk)
						.Include( e => e.OrganizationUnitFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ClosureComments.Contains(input.Filter) || e.RaisedByClosureComments.Contains(input.Filter))
						.WhereIf(input.MinDateFilter != null, e => e.Date >= input.MinDateFilter)
						.WhereIf(input.MaxDateFilter != null, e => e.Date <= input.MaxDateFilter)
						.WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter)
						.WhereIf(input.MinClosureDateFilter != null, e => e.ClosureDate >= input.MinClosureDateFilter)
						.WhereIf(input.MaxClosureDateFilter != null, e => e.ClosureDate <= input.MaxClosureDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ExceptionTypeNameFilter), e => e.ExceptionTypeFk != null && e.ExceptionTypeFk.Name == input.ExceptionTypeNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.RaisedByFk != null && e.RaisedByFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TestingTemplateCodeFilter), e => e.TestingTemplateFk != null && e.TestingTemplateFk.Code == input.TestingTemplateCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

			var query = (from o in filteredExceptionIncidents
                         join o1 in _lookup_exceptionTypeRepository.GetAll() on o.ExceptionTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.RaisedById equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_testingTemplateRepository.GetAll() on o.TestingTemplateId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         join o4 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         
                         select new GetExceptionIncidentForViewDto() { 
							ExceptionIncident = new ExceptionIncidentDto
							{
                                Code = o.Code,
                                Date = o.Date,
                                Description = o.Description,
                                Status = o.Status,
                                ClosureDate = o.ClosureDate,
                                ClosureComments = o.ClosureComments,
                                RaisedByClosureComments = o.RaisedByClosureComments,
                                Id = o.Id
							},
                         	ExceptionTypeName = s1 == null ? "" : s1.Name.ToString(),
                         	UserName = s2 == null ? "" : s2.Name.ToString(),
                         	TestingTemplateCode = s3 == null ? "" : s3.Code.ToString(),
                         	OrganizationUnitDisplayName = s4 == null ? "" : s4.DisplayName.ToString()
						 });


            var exceptionIncidentListDtos = await query.ToListAsync();

            return _exceptionIncidentsExcelExporter.ExportToFile(exceptionIncidentListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ExceptionIncidents)]
         public async Task<PagedResultDto<ExceptionIncidentExceptionTypeLookupTableDto>> GetAllExceptionTypeForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_exceptionTypeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var exceptionTypeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ExceptionIncidentExceptionTypeLookupTableDto>();
			foreach(var exceptionType in exceptionTypeList){
				lookupTableDtoList.Add(new ExceptionIncidentExceptionTypeLookupTableDto
				{
					Id = exceptionType.Id,
					DisplayName = exceptionType.Name?.ToString()
				});
			}

            return new PagedResultDto<ExceptionIncidentExceptionTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ExceptionIncidents)]
         public async Task<PagedResultDto<ExceptionIncidentUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ExceptionIncidentUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new ExceptionIncidentUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<ExceptionIncidentUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ExceptionIncidents)]
         public async Task<PagedResultDto<ExceptionIncidentTestingTemplateLookupTableDto>> GetAllTestingTemplateForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_testingTemplateRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Code.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var testingTemplateList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ExceptionIncidentTestingTemplateLookupTableDto>();
			foreach(var testingTemplate in testingTemplateList){
				lookupTableDtoList.Add(new ExceptionIncidentTestingTemplateLookupTableDto
				{
					Id = testingTemplate.Id,
					DisplayName = testingTemplate.Code?.ToString()
				});
			}

            return new PagedResultDto<ExceptionIncidentTestingTemplateLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ExceptionIncidents)]
         public async Task<PagedResultDto<ExceptionIncidentOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_organizationUnitRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.DisplayName.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ExceptionIncidentOrganizationUnitLookupTableDto>();
			foreach(var organizationUnit in organizationUnitList){
				lookupTableDtoList.Add(new ExceptionIncidentOrganizationUnitLookupTableDto
				{
					Id = organizationUnit.Id,
					DisplayName = organizationUnit.DisplayName?.ToString()
				});
			}

            return new PagedResultDto<ExceptionIncidentOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}