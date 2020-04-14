using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SDisco.Home.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Tour.Dtos;
using TepayLink.Sdisco.TripPlanManager.Dto;

namespace TepayLink.Sdisco.TripplanManager
{
    public interface ITripPlanManagerAppService: IApplicationService
    {
        Task<PagedResultDto<BasicTourDto>> GetDraftTrip(PagedInputDto input);
        Task<PagedResultDto<BasicTourDto>> GetPublicTrip(PagedInputDto input);
        Task<List<BasicItemDto>> GetSearchCategory();
        Task<List<SearchItemOutputDto>> SearchItem(SearhTourItemInputDto input);

        Task<List<SearchHotelOutputDto>> SearchHotel(SearhTourItemInputDto input);

        Task<List<SearchTransportOutputDto>> SearchTransport(SearchTransportInputDto input);
        Task DeleteTripPlan(BasicItemDto item);
        Task<SearchHotelOutputDto> CreateHotel(CreateHotelDto input);

        Task<SearchTransportOutputDto> CreateTransport(CreateTransportDto input);
        Task UpdateHotel(CreateHotelDto input);
        Task UpdateTransport(CreateTransportDto input);

        Task SaveTripPlan(CreateTripPlanInputDto input);

        Task<CustomizeTripOutputDto> GetTripForEdit(long tripId);

        Task<CustomizeTripOutputDto> GetTripForCustomize(long tripId);

        Task SaveCustomizeTrip(CreateTripPlanInputDto input);
        Task CheckTourAvaiable(CheckTourItemAvaiableDto input);
        Task<List<UtilityDto>> GetUtility();
        Task<SearchItemOutputDto> SaveActivity(CreateActivityDto input);
    }
}
