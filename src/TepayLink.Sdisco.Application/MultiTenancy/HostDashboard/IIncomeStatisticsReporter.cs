using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TepayLink.Sdisco.MultiTenancy.HostDashboard.Dto;

namespace TepayLink.Sdisco.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}