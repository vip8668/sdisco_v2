using System;
using System.Transactions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using TepayLink.Sdisco.EntityFrameworkCore;
using TepayLink.Sdisco.Migrations.Seed.Host;
using TepayLink.Sdisco.Migrations.Seed.Tenants;

namespace TepayLink.Sdisco.Migrations.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<SdiscoDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(SdiscoDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            //Host seed
            new InitialHostDbBuilder(context).Create();

            //Default tenant seed (in host database).
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}
