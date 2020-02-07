using System;
using System.Collections.Generic;
using System.Text;

namespace ICMSDemo.MultiTenancy.Payments.Stripe.Dto
{
    public class StripeGetPaymentInput
    {
        public string StripeSessionId { get; set; }
    }
}
