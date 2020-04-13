using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Help.Dto;

namespace TepayLink.Sdisco.Help
{
    public interface IHelpAppService : IApplicationService
    {
        Task<List<HelpCategoryDto>> GetCategories(HelpTypeEnum type);
        Task<PagedResultDto<HelpContentDto>> GetHelpContent(HelpContentSearchInputDto input);

    }
}
