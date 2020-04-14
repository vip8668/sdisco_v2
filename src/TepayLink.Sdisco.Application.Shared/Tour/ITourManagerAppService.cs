using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Tour.Dtos;
using TepayLink.Sdisco.TripPlanManager.Dto;

namespace TepayLink.Sdisco.Tour
{
    public interface ITourManagerAppService : IApplicationService
    {
        //  Task<PagedResultDto<BasicTourDto>> GetDraftTrip(PagedInputDto input);
        // Task<PagedResultDto<BasicTourDto>> GetPublicTrip(PagedInputDto input);
        Task<List<BasicItemDto>> GetSearchCategory();
        Task<List<SearchItemOutputDto>> SearchItem(SearhTourItemInputDto input);

        Task<List<SearchHotelOutputDto>> SearchHotel(SearhTourItemInputDto input);

        Task<List<SearchTransportOutputDto>> SearchTransport(SearchTransportInputDto input);
        Task DeleteTour(long tourId);
        Task<SearchHotelOutputDto> CreateHotel(CreateHotelDto input);
        Task<SearchTransportOutputDto> CreateTransport(CreateTransportDto input);

        Task Save(CreateTripPlanInputDto input);

        Task<List<UtilityDto>> GetUtility();


        Task<SearchItemOutputDto> SaveActivity(CreateActivityDto input);

        Task UpdateHotel(CreateHotelDto input);
        Task UpdateTransport(CreateTransportDto input);
        Task UpdateActivity(CreateActivityDto inpput);

        Task<CustomizeTripOutputDto> GetTourForEdit(long tripId);

        //  Task<CustomizeTripOutputDto> GetTripForEditOrCustomize(long tripId);
    }
}
