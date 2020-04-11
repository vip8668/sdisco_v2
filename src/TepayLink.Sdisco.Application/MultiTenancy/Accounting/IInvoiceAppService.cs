using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.MultiTenancy.Accounting.Dto;

namespace TepayLink.Sdisco.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
