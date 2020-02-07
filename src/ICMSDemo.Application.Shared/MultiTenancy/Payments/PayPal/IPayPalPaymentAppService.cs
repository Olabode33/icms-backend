using System.Threading.Tasks;
using Abp.Application.Services;
using ICMSDemo.MultiTenancy.Payments.PayPal.Dto;

namespace ICMSDemo.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
