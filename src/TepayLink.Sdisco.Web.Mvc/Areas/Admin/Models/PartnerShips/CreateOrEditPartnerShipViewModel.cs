using TepayLink.Sdisco.AdminConfig.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.PartnerShips
{
    public class CreateOrEditPartnerShipModalViewModel
    {
       public CreateOrEditPartnerShipDto PartnerShip { get; set; }

	   
	   public bool IsEditMode => PartnerShip.Id.HasValue;
    }
}