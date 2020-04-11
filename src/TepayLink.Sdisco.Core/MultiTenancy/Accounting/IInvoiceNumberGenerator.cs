using System.Threading.Tasks;
using Abp.Dependency;

namespace TepayLink.Sdisco.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}