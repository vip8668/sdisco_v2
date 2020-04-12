using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.AdminConfig.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.AdminConfig
{
    public interface IPartnerShipsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPartnerShipForViewDto>> GetAll(GetAllPartnerShipsInput input);

        Task<GetPartnerShipForViewDto> GetPartnerShipForView(int id);

		Task<GetPartnerShipForEditOutput> GetPartnerShipForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPartnerShipDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPartnerShipsToExcel(GetAllPartnerShipsForExcelInput input);

		
    }
}