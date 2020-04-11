using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using TepayLink.Sdisco.Editions.Dto;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Common;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Editions
{
    [AutoMapFrom(typeof(GetEditionEditOutput))]
    public class CreateEditionModalViewModel : GetEditionEditOutput, IFeatureEditViewModel
    {
        public IReadOnlyList<ComboboxItemDto> EditionItems { get; set; }

        public IReadOnlyList<ComboboxItemDto> FreeEditionItems { get; set; }
    }
}