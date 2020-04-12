using TepayLink.Sdisco.Bookings.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Orders
{
    public class CreateOrEditOrderModalViewModel
    {
       public CreateOrEditOrderDto Order { get; set; }

	   
	   public bool IsEditMode => Order.Id.HasValue;
    }
}