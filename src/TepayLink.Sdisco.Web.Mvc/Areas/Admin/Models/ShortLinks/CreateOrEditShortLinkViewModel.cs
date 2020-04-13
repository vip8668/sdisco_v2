using TepayLink.Sdisco.Affiliate.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.ShortLinks
{
    public class CreateOrEditShortLinkModalViewModel
    {
       public CreateOrEditShortLinkDto ShortLink { get; set; }

	   
	   public bool IsEditMode => ShortLink.Id.HasValue;
    }
}