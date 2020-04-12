using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SDisco.Home.Dto;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Tour
{
    [RemoteService(false)]
    public interface ICommonAppService : IApplicationService
    {
        Task<PagedResultDto<ReviewDetailDto>> GetTourItemReviewDetail(GetReviewDetailInput input);
        Task<GuestPhotoDto> GetTourItemGuestPhoto(GetGuestPhotoInput input);
        Task<PagedResultDto<ReviewDetailDto>> GetTourReviewDetail(GetReviewDetailInput input);
        Task<GuestPhotoDto> GetTourGuestPhoto(GetGuestPhotoInput input);

        Task<ReviewSummaryDto> GetTourItemReviewSummary(long tourId);

        Task<Dictionary<long, List<UtilityDto>>> GetUtilities(List<long> itemIds, ItemTypeEnum type);

        Task<List<UtilityDto>> GetUtilityByIds(List<int> utilitiesId);

        Task<Dictionary<long, ReviewSummaryDto>> GetTourItemReviewSummarys(List<long> tourIds);


        Task<ReviewSummaryDto> GetTourReviewSummary(long tourId);
        Task<decimal> GetRevenueOfTour(long tourId);
        Task<Dictionary<long, ReviewSummaryDto>> GetTourReviewSummarys(List<long> tourIds);

        Task<List<AvaiableTimeDto>> GetAvaiableTimeOfTour(long tourId);

        Task<Dictionary<long, List<AvaiableTimeDto>>> GetAvaiableTimeOfTours(List<long> tourIds);


        Task<List<PhotoDto>> GetTourItemPhoto(long itemId);

        Task<List<PhotoDto>> GetTourItemThumbPhoto(long itemId);

        Task<Dictionary<long, List<PhotoDto>>> GetTourItemThumbPhotos(List<long> itemId);


        Task<List<PhotoDto>> GetTourPhoto(long itemId);
        Task<List<PhotoDto>> GetTourThumbPhoto(long itemId);


        Task<Dictionary<long, List<PhotoDto>>> GetTourThumbPhotos(List<long> itemId);


        Task<List<BasicTourDto>> GetRelateTour(long itemId);


        Task<List<SaveItemDto>> GetSaveItem(List<long> itemIds, ItemTypeEnum itemType);


        Task<bool> IsSave(long itemIds, ItemTypeEnum itemType);

        Task<List<BasicTourDto>> GetTourByTourIds(List<long> tourIds);

        Task<List<BasicTourItemDto>> GetRelateTourItem(long itemId);


        Task<List<AvaiableTimeDto>> GetAvaiableTimeOfTourItem(long itemId);

        Task<Dictionary<long, List<AvaiableTimeDto>>> GetAvaiableTimeOfTourItems(List<long> itemIds);

        Task<BasicPriceDto> GetPriceOfTour(long tourId);

        Task<Dictionary<long, BasicPriceDto>> GetPriceOfTours(List<long> tourIds);

        Task<BasicPriceDto> GetPriceOfTourItem(long tourId);
        Task<Dictionary<long, BasicPriceDto>> GetPriceOfTourItems(List<long> tourIds);
        long GetHostUserId(long userId);


        void InsertImages(List<string> photos, long itemId, ImageType imageType, ItemTypeEnum itemType);
        void DeleteImageOfTour(long itemId, ItemTypeEnum itemType, List<ImageTypeEnum> imageTypes);

        Task ProcessBonus(long tripId, RevenueTypeEnum type);

        Task<List<SearchItemOutputDto>> SearchItem(SearhTourItemInputDto input, bool checkHostUser);
        Task<List<SearchHotelOutputDto>> SearchHotel(SearhTourItemInputDto input, bool checkHostUser);
        Task<List<SearchTransportOutputDto>> SearchTransport(SearchTransportInputDto input, bool checkHostUser);
        Task<SearchHotelOutputDto> CreateHotel(CreateHotelDto input, bool isFromHost);
        Task<SearchTransportOutputDto> CreateTransport(CreateTransportDto input, bool isFromHost);
        Task<SearchItemOutputDto> SaveActivity(CreateActivityDto input, bool isFromHost);
        Task<List<BasicItemDto>> GetSearchCategory();
        Task<CustomizeTripOutputDto> GetTripForEditOrCustomize(long tourId);
        Task<decimal> GetFeeByHostUser(long userId, long price);
        Task<List<FeeConfigDto>> GetFeeConfigs(List<long> userIds);
    }
}