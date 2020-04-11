using Abp.AutoMapper;
using TepayLink.Sdisco.Localization.Dto;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Languages
{
    [AutoMapFrom(typeof(GetLanguageForEditOutput))]
    public class CreateOrEditLanguageModalViewModel : GetLanguageForEditOutput
    {
        public bool IsEditMode => Language.Id.HasValue;
    }
}