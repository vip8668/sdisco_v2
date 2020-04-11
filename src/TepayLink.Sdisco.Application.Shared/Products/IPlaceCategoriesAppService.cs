using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface IPlaceCategoriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPlaceCategoryForViewDto>> GetAll(GetAllPlaceCategoriesInput input);

        Task<GetPlaceCategoryForViewDto> GetPlaceCategoryForView(int id);

		Task<GetPlaceCategoryForEditOutput> GetPlaceCategoryForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPlaceCategoryDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPlaceCategoriesToExcel(GetAllPlaceCategoriesForExcelInput input);

		
    }
}