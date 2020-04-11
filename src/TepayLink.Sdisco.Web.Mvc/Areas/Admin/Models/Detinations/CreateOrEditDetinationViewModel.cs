using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Detinations
{
    public class CreateOrEditDetinationModalViewModel
    {
       public CreateOrEditDetinationDto Detination { get; set; }

	   
	   public bool IsEditMode => Detination.Id.HasValue;
    }
}