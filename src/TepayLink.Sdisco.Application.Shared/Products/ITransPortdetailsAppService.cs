using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface ITransPortdetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTransPortdetailForViewDto>> GetAll(GetAllTransPortdetailsInput input);

        Task<GetTransPortdetailForViewDto> GetTransPortdetailForView(long id);

		Task<GetTransPortdetailForEditOutput> GetTransPortdetailForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditTransPortdetailDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetTransPortdetailsToExcel(GetAllTransPortdetailsForExcelInput input);

		
		Task<PagedResultDto<TransPortdetailProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}