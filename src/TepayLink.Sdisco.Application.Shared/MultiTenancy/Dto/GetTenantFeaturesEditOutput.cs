using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Editions.Dto;

namespace TepayLink.Sdisco.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}