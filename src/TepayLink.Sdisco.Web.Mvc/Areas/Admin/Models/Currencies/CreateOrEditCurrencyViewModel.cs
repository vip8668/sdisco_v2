using TepayLink.Sdisco.AdminConfig.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Currencies
{
    public class CreateOrEditCurrencyModalViewModel
    {
       public CreateOrEditCurrencyDto Currency { get; set; }

	   
	   public bool IsEditMode => Currency.Id.HasValue;
    }
}