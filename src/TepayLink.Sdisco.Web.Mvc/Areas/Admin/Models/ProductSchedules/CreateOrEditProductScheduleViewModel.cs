using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.ProductSchedules
{
    public class CreateOrEditProductScheduleModalViewModel
    {
       public CreateOrEditProductScheduleDto ProductSchedule { get; set; }

	   		public string ProductName { get; set;}


	   public bool IsEditMode => ProductSchedule.Id.HasValue;
    }
}