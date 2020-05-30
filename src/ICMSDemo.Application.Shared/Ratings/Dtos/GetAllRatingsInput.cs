using Abp.Application.Services.Dto;
using System;

namespace ICMSDemo.Ratings.Dtos
{
    public class GetAllRatingsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }



    }
}