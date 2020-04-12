using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface IUtilitiesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetUtilityForViewDto>> GetAll(GetAllUtilitiesInput input);

        Task<GetUtilityForViewDto> GetUtilityForView(int id);

		Task<GetUtilityForEditOutput> GetUtilityForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditUtilityDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetUtilitiesToExcel(GetAllUtilitiesForExcelInput input);

		
    }
}