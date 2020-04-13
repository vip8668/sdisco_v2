using TepayLink.Sdisco.KOL.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.PartnerRevenues
{
    public class CreateOrEditPartnerRevenueModalViewModel
    {
       public CreateOrEditPartnerRevenueDto PartnerRevenue { get; set; }

	   
	   public bool IsEditMode => PartnerRevenue.Id.HasValue;
    }
}