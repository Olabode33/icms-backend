using System.Threading.Tasks;
using Abp.Application.Services;
using ICMSDemo.MultiTenancy.Payments.Dto;
using ICMSDemo.MultiTenancy.Payments.Stripe.Dto;

namespace ICMSDemo.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}