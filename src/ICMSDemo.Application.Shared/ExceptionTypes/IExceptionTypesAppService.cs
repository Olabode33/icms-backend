using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.ExceptionTypes.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.ExceptionTypes
{
    public interface IExceptionTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetExceptionTypeForViewDto>> GetAll(GetAllExceptionTypesInput input);

        Task<GetExceptionTypeForViewDto> GetExceptionTypeForView(int id);

		Task<GetExceptionTypeForEditOutput> GetExceptionTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditExceptionTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetExceptionTypesToExcel(GetAllExceptionTypesForExcelInput input);

		
    }
}