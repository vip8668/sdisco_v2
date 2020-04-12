using TepayLink.Sdisco.AdminConfig.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Countries
{
    public class CreateOrEditCountryModalViewModel
    {
       public CreateOrEditCountryDto Country { get; set; }

	   
	   public bool IsEditMode => Country.Id.HasValue;
    }
}