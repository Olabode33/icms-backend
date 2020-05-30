
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.Ratings.Dtos
{
    public class CreateOrEditRatingDto : EntityDto<int?>
    {

		[Required]
		public string Name { get; set; }
		
		
		public string Code { get; set; }
		
		
		public string Description { get; set; }	
		
		
		
		[Range(RatingConsts.MinUpperBoundaryValue, RatingConsts.MaxUpperBoundaryValue)]
		public decimal UpperBoundary { get; set; }
		
		

    }
}