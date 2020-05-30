using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.Ratings.Dtos;
using ICMSDemo.Dto;


namespace ICMSDemo.Ratings
{
    public interface IRatingsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRatingForViewDto>> GetAll(GetAllRatingsInput input);

        Task<GetRatingForViewDto> GetRatingForView(int id);

		Task<GetRatingForEditOutput> GetRatingForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditRatingDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetRatingsToExcel(GetAllRatingsForExcelInput input);

		
    }
}