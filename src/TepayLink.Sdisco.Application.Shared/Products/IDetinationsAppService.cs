using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products
{
    public interface IDetinationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDetinationForViewDto>> GetAll(GetAllDetinationsInput input);

        Task<GetDetinationForViewDto> GetDetinationForView(long id);

		Task<GetDetinationForEditOutput> GetDetinationForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditDetinationDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetDetinationsToExcel(GetAllDetinationsForExcelInput input);

		
    }
}