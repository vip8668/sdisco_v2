using System.Linq;
using TepayLink.Sdisco.Editions;
using TepayLink.Sdisco.EntityFrameworkCore;
using TepayLink.Sdisco.MultiTenancy.Payments;

namespace TepayLink.Sdisco.Test.Base.TestData
{
    public class TestSubscriptionPaymentBuilder
    {
        private readonly SdiscoDbContext _context;
        private readonly int _tenantId;

        public TestSubscriptionPaymentBuilder(SdiscoDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreatePayments();
        }

        private void CreatePayments()
        {
            var defaultEdition = _context.Editions.FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);

            CreatePayment(1, defaultEdition.Id, _tenantId, 2, "147741");
            CreatePayment(19, defaultEdition.Id, _tenantId, 29, "1477419");
        }

        private void CreatePayment(decimal amount, int editionId, int tenantId, int dayCount, string paymentId)
        {
            _context.SubscriptionPayments.Add(new SubscriptionPayment
            {
                Amount = amount,
                EditionId = editionId,
                TenantId = tenantId,
                DayCount = dayCount,
                ExternalPaymentId = paymentId
            });
        }
    }

}
