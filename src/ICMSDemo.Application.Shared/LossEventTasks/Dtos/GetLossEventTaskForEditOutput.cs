using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.LossEventTasks.Dtos
{
    public class GetLossEventTaskForEditOutput
    {
		public CreateOrEditLossEventTaskDto LossEventTask { get; set; }


    }
}