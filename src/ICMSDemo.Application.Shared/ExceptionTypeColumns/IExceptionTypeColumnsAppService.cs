using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ICMSDemo.ExceptionTypeColumns.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.ExceptionTypeColumns
{
    public interface IExceptionTypeColumnsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetExceptionTypeColumnForViewDto>> GetAll(GetAllExceptionTypeColumnsInput input);

        Task<GetExceptionTypeColumnForViewDto> GetExceptionTypeColumnForView(int id);

		Task<GetExceptionTypeColumnForEditOutput> GetExceptionTypeColumnForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditExceptionTypeColumnDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetExceptionTypeColumnsToExcel(GetAllExceptionTypeColumnsForExcelInput input);

		
		Task<PagedResultDto<ExceptionTypeColumnExceptionTypeLookupTableDto>> GetAllExceptionTypeForLookupTable(GetAllForLookupTableInput input);
		
    }
}