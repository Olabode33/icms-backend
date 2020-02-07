using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ICMSDemo.MultiTenancy.HostDashboard.Dto;

namespace ICMSDemo.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}