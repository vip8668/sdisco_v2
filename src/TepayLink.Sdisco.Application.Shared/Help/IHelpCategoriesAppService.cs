using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Help.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Help
{
    public interface IHelpCategoriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetHelpCategoryForViewDto>> GetAll(GetAllHelpCategoriesInput input);

        Task<GetHelpCategoryForViewDto> GetHelpCategoryForView(long id);

		Task<GetHelpCategoryForEditOutput> GetHelpCategoryForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditHelpCategoryDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetHelpCategoriesToExcel(GetAllHelpCategoriesForExcelInput input);

		
    }
}