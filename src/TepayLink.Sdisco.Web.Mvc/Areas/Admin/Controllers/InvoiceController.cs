using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.MultiTenancy.Accounting;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Accounting;
using TepayLink.Sdisco.Web.Controllers;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InvoiceController : SdiscoControllerBase
    {
        private readonly IInvoiceAppService _invoiceAppService;

        public InvoiceController(IInvoiceAppService invoiceAppService)
        {
            _invoiceAppService = invoiceAppService;
        }


        [HttpGet]
        public async Task<ActionResult> Index(long paymentId)
        {
            var invoice = await _invoiceAppService.GetInvoiceInfo(new EntityDto<long>(paymentId));
            var model = new InvoiceViewModel
            {
                Invoice = invoice
            };

            return View(model);
        }
    }
}