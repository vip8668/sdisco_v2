using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Help.Dto;

namespace TepayLink.Sdisco.Help
{
    public class HelpAppService : SdiscoAppServiceBase, IHelpAppService
    {
        private readonly IRepository<HelpCategory,long> _helpCategory;

        private readonly IRepository<HelpContent, long> _helpContentRepository;

        public HelpAppService(IRepository<HelpCategory, long> helpCategory, IRepository<HelpContent, long> helpContentRepository)
        {
            _helpCategory = helpCategory;
            _helpContentRepository = helpContentRepository;
        }

        public async Task<List<HelpCategoryDto>> GetCategories(HelpTypeEnum type)
        {
            var list = _helpCategory.GetAll().Where(p => p.Type == type).ToList();
            return ObjectMapper.Map<List<HelpCategoryDto>>(list);
        }
        public async Task<PagedResultDto<HelpContentDto>> GetHelpContent(HelpContentSearchInputDto input)
        {
            var query = _helpContentRepository.GetAll()
                .WhereIf(input.CategoryId > 0, p => p.HelpCategoryId == input.CategoryId)
                .WhereIf(!string.IsNullOrEmpty(input.Keyword),
                    p => p.Question.Contains(input.Keyword) || p.Answer.Contains(input.Keyword));
            var itemCount = query.Count();
            var itemlist = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList().Select(p => new HelpContentDto
            {
                Id = p.Id,
                Answer = p.Answer,
                Question = p.Question
            }).ToList();
            return new PagedResultDto<HelpContentDto>
            {
                Items = itemlist,
                TotalCount = itemCount
            };
        }
    }
}
