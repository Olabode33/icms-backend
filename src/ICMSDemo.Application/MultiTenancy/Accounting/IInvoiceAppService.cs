using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using ICMSDemo.MultiTenancy.Accounting.Dto;

namespace ICMSDemo.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
