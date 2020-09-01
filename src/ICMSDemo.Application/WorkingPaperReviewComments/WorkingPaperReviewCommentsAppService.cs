using ICMSDemo.Authorization.Users;
using ICMSDemo.WorkingPapers;
using ICMSDemo.Authorization.Users;

using ICMSDemo;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.WorkingPaperReviewComments.Exporting;
using ICMSDemo.WorkingPaperReviewComments.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Timing;

namespace ICMSDemo.WorkingPaperReviewComments
{
	[AbpAuthorize(AppPermissions.Pages_WorkingPaperReviewComments)]
    public class WorkingPaperReviewCommentsAppService : ICMSDemoAppServiceBase, IWorkingPaperReviewCommentsAppService
    {
		 private readonly IRepository<WorkingPaperReviewComment> _workingPaperReviewCommentRepository;
		 private readonly IWorkingPaperReviewCommentsExcelExporter _workingPaperReviewCommentsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<WorkingPaper,Guid> _lookup_workingPaperRepository;
		 

		  public WorkingPaperReviewCommentsAppService(IRepository<WorkingPaperReviewComment> workingPaperReviewCommentRepository, IWorkingPaperReviewCommentsExcelExporter workingPaperReviewCommentsExcelExporter , IRepository<User, long> lookup_userRepository, IRepository<WorkingPaper, Guid> lookup_workingPaperRepository) 
		  {
			_workingPaperReviewCommentRepository = workingPaperReviewCommentRepository;
			_workingPaperReviewCommentsExcelExporter = workingPaperReviewCommentsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		_lookup_workingPaperRepository = lookup_workingPaperRepository;
		
		  }

		 public async Task<PagedResultDto<GetWorkingPaperReviewCommentForViewDto>> GetAll(GetAllWorkingPaperReviewCommentsInput input)
         {
			var statusFilter = input.StatusFilter.HasValue
                        ? (Status) input.StatusFilter
                        : default;			
					
			var filteredWorkingPaperReviewComments = _workingPaperReviewCommentRepository.GetAll()
						.Include( e => e.AssigneeUserFk)
						.Include( e => e.WorkingPaperFk)
						.Include( e => e.AssignerUserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Comments.Contains(input.Filter))
					
						.WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
						.WhereIf(input.MinExpectedCompletionDateFilter != null, e => e.ExpectedCompletionDate >= input.MinExpectedCompletionDateFilter)
						.WhereIf(input.MaxExpectedCompletionDateFilter != null, e => e.ExpectedCompletionDate <= input.MaxExpectedCompletionDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.AssigneeUserFk != null && e.AssigneeUserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.WorkingPaperCodeFilter), e => e.WorkingPaperFk != null && e.WorkingPaperFk.Code == input.WorkingPaperCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.AssignerUserFk != null && e.AssignerUserFk.Name == input.UserName2Filter);

			var pagedAndFilteredWorkingPaperReviewComments = filteredWorkingPaperReviewComments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var workingPaperReviewComments = from o in pagedAndFilteredWorkingPaperReviewComments
                         join o1 in _lookup_userRepository.GetAll() on o.AssigneeUserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_workingPaperRepository.GetAll() on o.WorkingPaperId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_userRepository.GetAll() on o.AssignerUserId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetWorkingPaperReviewCommentForViewDto() {
							WorkingPaperReviewComment = new WorkingPaperReviewCommentDto
							{
                                Title = o.Title,
                         
                                Status = o.Status,
                                Severity = o.Severity,
                                ExpectedCompletionDate = o.ExpectedCompletionDate,
                                Id = o.Id
							},
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	WorkingPaperCode = s2 == null || s2.Code == null ? "" : s2.Code.ToString(),
                         	UserName2 = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
						};

            var totalCount = await filteredWorkingPaperReviewComments.CountAsync();

            return new PagedResultDto<GetWorkingPaperReviewCommentForViewDto>(
                totalCount,
                await workingPaperReviewComments.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_WorkingPaperReviewComments_Edit)]
		 public async Task<GetWorkingPaperReviewCommentForEditOutput> GetWorkingPaperReviewCommentForEdit(EntityDto input)
         {
            var workingPaperReviewComment = await _workingPaperReviewCommentRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetWorkingPaperReviewCommentForEditOutput {WorkingPaperReviewComment = ObjectMapper.Map<CreateOrEditWorkingPaperReviewCommentDto>(workingPaperReviewComment)};

		    if (output.WorkingPaperReviewComment.AssigneeUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.WorkingPaperReviewComment.AssigneeUserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

		    if (output.WorkingPaperReviewComment.WorkingPaperId != null)
            {
                var _lookupWorkingPaper = await _lookup_workingPaperRepository.FirstOrDefaultAsync((Guid)output.WorkingPaperReviewComment.WorkingPaperId);
                output.WorkingPaperCode = _lookupWorkingPaper?.Code?.ToString();
            }

		    if (output.WorkingPaperReviewComment.AssignerUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.WorkingPaperReviewComment.AssignerUserId);
                output.UserName2 = _lookupUser?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditWorkingPaperReviewCommentDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_WorkingPaperReviewComments_Create)]
		 protected virtual async Task Create(CreateOrEditWorkingPaperReviewCommentDto input)
         {
            var workingPaperReviewComment = ObjectMapper.Map<WorkingPaperReviewComment>(input);
            workingPaperReviewComment.AssigneeUserId = AbpSession.UserId;


            if (AbpSession.TenantId != null)
			{
				workingPaperReviewComment.TenantId = (int) AbpSession.TenantId;
			}
		

            await _workingPaperReviewCommentRepository.InsertAsync(workingPaperReviewComment);
         }

		 [AbpAuthorize(AppPermissions.Pages_WorkingPaperReviewComments_Edit)]
		 protected virtual async Task Update(CreateOrEditWorkingPaperReviewCommentDto input)
         {
            var workingPaperReviewComment = await _workingPaperReviewCommentRepository.FirstOrDefaultAsync((int)input.Id);
            workingPaperReviewComment.Response = input.Response;
            workingPaperReviewComment.Status = Status.Closed;
            workingPaperReviewComment.CompletionDate = Clock.Now;          
         }

		 [AbpAuthorize(AppPermissions.Pages_WorkingPaperReviewComments_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _workingPaperReviewCommentRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetWorkingPaperReviewCommentsToExcel(GetAllWorkingPaperReviewCommentsForExcelInput input)
         {
			var statusFilter = input.StatusFilter.HasValue
                        ? (Status) input.StatusFilter
                        : default;			
					
			var filteredWorkingPaperReviewComments = _workingPaperReviewCommentRepository.GetAll()
						.Include( e => e.AssigneeUserFk)
						.Include( e => e.WorkingPaperFk)
						.Include( e => e.AssignerUserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Comments.Contains(input.Filter))
			
						.WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
						.WhereIf(input.MinExpectedCompletionDateFilter != null, e => e.ExpectedCompletionDate >= input.MinExpectedCompletionDateFilter)
						.WhereIf(input.MaxExpectedCompletionDateFilter != null, e => e.ExpectedCompletionDate <= input.MaxExpectedCompletionDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.AssigneeUserFk != null && e.AssigneeUserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.WorkingPaperCodeFilter), e => e.WorkingPaperFk != null && e.WorkingPaperFk.Code == input.WorkingPaperCodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.AssignerUserFk != null && e.AssignerUserFk.Name == input.UserName2Filter);

			var query = (from o in filteredWorkingPaperReviewComments
                         join o1 in _lookup_userRepository.GetAll() on o.AssigneeUserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_workingPaperRepository.GetAll() on o.WorkingPaperId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_userRepository.GetAll() on o.AssignerUserId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetWorkingPaperReviewCommentForViewDto() { 
							WorkingPaperReviewComment = new WorkingPaperReviewCommentDto
							{
                                Title = o.Title,
                          
                                Status = o.Status,
                                Severity = o.Severity,
                                ExpectedCompletionDate = o.ExpectedCompletionDate,
                                Id = o.Id
							},
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	WorkingPaperCode = s2 == null || s2.Code == null ? "" : s2.Code.ToString(),
                         	UserName2 = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
						 });


            var workingPaperReviewCommentListDtos = await query.ToListAsync();

            return _workingPaperReviewCommentsExcelExporter.ExportToFile(workingPaperReviewCommentListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_WorkingPaperReviewComments)]
         public async Task<PagedResultDto<WorkingPaperReviewCommentUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<WorkingPaperReviewCommentUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new WorkingPaperReviewCommentUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<WorkingPaperReviewCommentUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_WorkingPaperReviewComments)]
         public async Task<PagedResultDto<WorkingPaperReviewCommentWorkingPaperLookupTableDto>> GetAllWorkingPaperForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_workingPaperRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Code != null && e.Code.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var workingPaperList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<WorkingPaperReviewCommentWorkingPaperLookupTableDto>();
			foreach(var workingPaper in workingPaperList){
				lookupTableDtoList.Add(new WorkingPaperReviewCommentWorkingPaperLookupTableDto
				{
					Id = workingPaper.Id.ToString(),
					DisplayName = workingPaper.Code?.ToString()
				});
			}

            return new PagedResultDto<WorkingPaperReviewCommentWorkingPaperLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}