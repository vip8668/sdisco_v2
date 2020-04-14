using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Help.Dto;
using TepayLink.Sdisco.Help.Dtos;

namespace TepayLink.Sdisco.Help
{
    public interface IHelpAppService : IApplicationService
    {
        Task<List<HelpCategoryDtoV1>> GetCategories(HelpTypeEnum type);
        Task<PagedResultDto<HelpContentDto>> GetHelpContent(HelpContentSearchInputDto input);

    }
}
