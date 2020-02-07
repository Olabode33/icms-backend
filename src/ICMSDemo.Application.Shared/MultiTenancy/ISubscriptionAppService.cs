﻿using System.Threading.Tasks;
using Abp.Application.Services;

namespace ICMSDemo.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
