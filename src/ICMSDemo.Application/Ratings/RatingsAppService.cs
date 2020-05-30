

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ICMSDemo.Ratings.Exporting;
using ICMSDemo.Ratings.Dtos;
using ICMSDemo.Dto;
using Abp.Application.Services.Dto;
using ICMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;

namespace ICMSDemo.Ratings
{
	[AbpAuthorize(AppPermissions.Pages_Ratings)]
    public class RatingsAppService : ICMSDemoAppServiceBase, IRatingsAppService
    {
		 private readonly IRepository<Rating> _ratingRepository;
		 private readonly IRatingsExcelExporter _ratingsExcelExporter;
		 

		  public RatingsAppService(IRepository<Rating> ratingRepository, IRatingsExcelExporter ratingsExcelExporter ) 
		  {
			_ratingRepository = ratingRepository;
			_ratingsExcelExporter = ratingsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetRatingForViewDto>> GetAll(GetAllRatingsInput input)
         {
			
			var filteredRatings = _ratingRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter));

			var pagedAndFilteredRatings = filteredRatings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var ratings = from o in pagedAndFilteredRatings
                         select new GetRatingForViewDto() {
							Rating = new RatingDto
							{
                                Name = o.Name,
                                Code = o.Code,
                                Description = o.Description,
                               
                                UpperBoundary = o.UpperBoundary,
                                Id = o.Id
							}
						};

            var totalCount = await filteredRatings.CountAsync();

            return new PagedResultDto<GetRatingForViewDto>(
                totalCount,
                await ratings.ToListAsync()
            );
         }
		 
		 public async Task<GetRatingForViewDto> GetRatingForView(int id)
         {
            var rating = await _ratingRepository.GetAsync(id);

            var output = new GetRatingForViewDto { Rating = ObjectMapper.Map<RatingDto>(rating) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Ratings_Edit)]
		 public async Task<GetRatingForEditOutput> GetRatingForEdit(EntityDto input)
         {
            var rating = await _ratingRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRatingForEditOutput {Rating = ObjectMapper.Map<CreateOrEditRatingDto>(rating)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRatingDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Ratings_Create)]
		 protected virtual async Task Create(CreateOrEditRatingDto input)
         {
            var rating = ObjectMapper.Map<Rating>(input);

			
			if (AbpSession.TenantId != null)
			{
				rating.TenantId = (int) AbpSession.TenantId;
			}

            var previousRating = await _ratingRepository.FirstOrDefaultAsync(x => input.UpperBoundary <= x.UpperBoundary );

            if (previousRating != null)
            {
                throw new UserFriendlyException("A rating exists within this band already.");
            }

            await _ratingRepository.InsertAsync(rating);
         }

		 [AbpAuthorize(AppPermissions.Pages_Ratings_Edit)]
		 protected virtual async Task Update(CreateOrEditRatingDto input)
         {
            var rating = await _ratingRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, rating);
         }

		 [AbpAuthorize(AppPermissions.Pages_Ratings_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _ratingRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetRatingsToExcel(GetAllRatingsForExcelInput input)
         {
			
			var filteredRatings = _ratingRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter));

			var query = (from o in filteredRatings
                         select new GetRatingForViewDto() { 
							Rating = new RatingDto
							{
                                Name = o.Name,
                                Code = o.Code,
                                Description = o.Description,
               
                                UpperBoundary = o.UpperBoundary,
                                Id = o.Id
							}
						 });


            var ratingListDtos = await query.ToListAsync();

            return _ratingsExcelExporter.ExportToFile(ratingListDtos);
         }


    }
}