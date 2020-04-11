using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Editions.Dto;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}