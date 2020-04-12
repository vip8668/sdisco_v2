using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface IProductSchedulesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetProductScheduleForViewDto>> GetAll(GetAllProductSchedulesInput input);

        Task<GetProductScheduleForViewDto> GetProductScheduleForView(long id);

		Task<GetProductScheduleForEditOutput> GetProductScheduleForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditProductScheduleDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetProductSchedulesToExcel(GetAllProductSchedulesForExcelInput input);

		
		Task<PagedResultDto<ProductScheduleProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}