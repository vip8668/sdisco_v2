using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Bookings
{
    public interface IOrdersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetOrderForViewDto>> GetAll(GetAllOrdersInput input);

        Task<GetOrderForViewDto> GetOrderForView(long id);

		Task<GetOrderForEditOutput> GetOrderForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditOrderDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetOrdersToExcel(GetAllOrdersForExcelInput input);

		
    }
}