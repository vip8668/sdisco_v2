﻿using System.Linq;
using System.Threading.Tasks;
using Abp.EntityFrameworkCore;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using TepayLink.Sdisco.EntityFrameworkCore;
using TepayLink.Sdisco.EntityFrameworkCore.Repositories;

namespace TepayLink.Sdisco.MultiTenancy.Payments
{
    public class SubscriptionPaymentRepository : SdiscoRepositoryBase<SubscriptionPayment, long>, ISubscriptionPaymentRepository
    {
        public SubscriptionPaymentRepository(IDbContextProvider<SdiscoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<SubscriptionPayment> GetByGatewayAndPaymentIdAsync(SubscriptionPaymentGatewayType gateway, string paymentId)
        {
            return await SingleAsync(p => p.ExternalPaymentId == paymentId && p.Gateway == gateway);
        }

        public async Task<SubscriptionPayment> GetLastCompletedPaymentOrDefaultAsync(int tenantId, SubscriptionPaymentGatewayType? gateway, bool? isRecurring)
        {
            return (await GetAll()
                    .Where(p => p.TenantId == tenantId)
                    .Where(p => p.Status == SubscriptionPaymentStatus.Completed)
                    .WhereIf(gateway.HasValue, p => p.Gateway == gateway.Value)
                    .WhereIf(isRecurring.HasValue, p => p.IsRecurring == isRecurring.Value)
                    .ToListAsync()
                )
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();
        }

        public async Task<SubscriptionPayment> GetLastPaymentOrDefaultAsync(int tenantId, SubscriptionPaymentGatewayType? gateway, bool? isRecurring)
        {
            return (await GetAll()
                    .Where(p => p.TenantId == tenantId)
                    .WhereIf(gateway.HasValue, p => p.Gateway == gateway.Value)
                    .WhereIf(isRecurring.HasValue, p => p.IsRecurring == isRecurring.Value)
                    .ToListAsync()).OrderByDescending(x => x.Id
                )
                .FirstOrDefault();
        }
    }
}