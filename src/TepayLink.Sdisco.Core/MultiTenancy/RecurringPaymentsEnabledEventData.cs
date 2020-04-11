using Abp.Events.Bus;

namespace TepayLink.Sdisco.MultiTenancy
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}