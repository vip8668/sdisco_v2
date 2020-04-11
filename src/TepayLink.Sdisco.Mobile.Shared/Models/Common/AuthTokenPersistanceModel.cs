using System;
using Abp.AutoMapper;
using TepayLink.Sdisco.Sessions.Dto;

namespace TepayLink.Sdisco.Models.Common
{
    [AutoMapFrom(typeof(ApplicationInfoDto)),
     AutoMapTo(typeof(ApplicationInfoDto))]
    public class ApplicationInfoPersistanceModel
    {
        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}