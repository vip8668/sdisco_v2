using TepayLink.Sdisco.Reports.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.RevenueByMonths
{
    public class CreateOrEditRevenueByMonthModalViewModel
    {
       public CreateOrEditRevenueByMonthDto RevenueByMonth { get; set; }

	   
	   public bool IsEditMode => RevenueByMonth.Id.HasValue;
    }
}