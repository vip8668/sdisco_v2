using TepayLink.Sdisco.Editions;
using TepayLink.Sdisco.Editions.Dto;
using TepayLink.Sdisco.MultiTenancy.Payments;
using TepayLink.Sdisco.Security;
using TepayLink.Sdisco.MultiTenancy.Payments.Dto;

namespace TepayLink.Sdisco.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
