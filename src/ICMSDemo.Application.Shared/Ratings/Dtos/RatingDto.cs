
using System;
using Abp.Application.Services.Dto;

namespace ICMSDemo.Ratings.Dtos
{
    public class RatingDto : EntityDto
    {
		public string Name { get; set; }

		public string Code { get; set; }

		public string Description { get; set; }

		public decimal UpperBoundary { get; set; }



    }
}