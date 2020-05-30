using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.Ratings.Dtos
{
    public class GetRatingForEditOutput
    {
		public CreateOrEditRatingDto Rating { get; set; }


    }
}