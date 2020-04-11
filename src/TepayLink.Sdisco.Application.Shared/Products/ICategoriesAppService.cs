using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface ICategoriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCategoryForViewDto>> GetAll(GetAllCategoriesInput input);

        Task<GetCategoryForViewDto> GetCategoryForView(int id);

		Task<GetCategoryForEditOutput> GetCategoryForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCategoryDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetCategoriesToExcel(GetAllCategoriesForExcelInput input);

		
    }
}