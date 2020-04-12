using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using SDisco.Home.Dto;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Tours
{
    public class CommonAppService:ICommonAppService
    {
        public async Task<PagedResultDto<ReviewDetailDto>> GetTourItemReviewDetail(GetReviewDetailInput input)
        {
            throw new System.NotImplementedException();
        }

        public async Task<GuestPhotoDto> GetTourItemGuestPhoto(GetGuestPhotoInput input)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PagedResultDto<ReviewDetailDto>> GetTourReviewDetail(GetReviewDetailInput input)
        {
            throw new System.NotImplementedException();
        }

        public async Task<GuestPhotoDto> GetTourGuestPhoto(GetGuestPhotoInput input)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ReviewSummaryDto> GetTourItemReviewSummary(long tourId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Dictionary<long, List<UtilityDto>>> GetUtilities(List<long> itemIds, ItemTypeEnum type)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<UtilityDto>> GetUtilityByIds(List<int> utilitiesId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Dictionary<long, ReviewSummaryDto>> GetTourItemReviewSummarys(List<long> tourIds)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ReviewSummaryDto> GetTourReviewSummary(long tourId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<decimal> GetRevenueOfTour(long tourId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Dictionary<long, ReviewSummaryDto>> GetTourReviewSummarys(List<long> tourIds)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<AvaiableTimeDto>> GetAvaiableTimeOfTour(long tourId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Dictionary<long, List<AvaiableTimeDto>>> GetAvaiableTimeOfTours(List<long> tourIds)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<PhotoDto>> GetTourItemPhoto(long itemId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<PhotoDto>> GetTourItemThumbPhoto(long itemId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Dictionary<long, List<PhotoDto>>> GetTourItemThumbPhotos(List<long> itemId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<PhotoDto>> GetTourPhoto(long itemId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<PhotoDto>> GetTourThumbPhoto(long itemId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Dictionary<long, List<PhotoDto>>> GetTourThumbPhotos(List<long> itemId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<BasicTourDto>> GetRelateTour(long itemId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<SaveItemDto>> GetSaveItem(List<long> itemIds, ItemTypeEnum itemType)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> IsSave(long itemIds, ItemTypeEnum itemType)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<BasicTourDto>> GetTourByTourIds(List<long> tourIds)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<BasicTourItemDto>> GetRelateTourItem(long itemId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<AvaiableTimeDto>> GetAvaiableTimeOfTourItem(long itemId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Dictionary<long, List<AvaiableTimeDto>>> GetAvaiableTimeOfTourItems(List<long> itemIds)
        {
            throw new System.NotImplementedException();
        }

        public async Task<BasicPriceDto> GetPriceOfTour(long tourId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Dictionary<long, BasicPriceDto>> GetPriceOfTours(List<long> tourIds)
        {
            throw new System.NotImplementedException();
        }

        public async Task<BasicPriceDto> GetPriceOfTourItem(long tourId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Dictionary<long, BasicPriceDto>> GetPriceOfTourItems(List<long> tourIds)
        {
            throw new System.NotImplementedException();
        }

        public long GetHostUserId(long userId)
        {
            throw new System.NotImplementedException();
        }

        public void InsertImages(List<string> photos, long itemId, ImageType imageType, ItemTypeEnum itemType)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteImageOfTour(long itemId, ItemTypeEnum itemType, List<ImageTypeEnum> imageTypes)
        {
            throw new System.NotImplementedException();
        }

        public async Task ProcessBonus(long tripId, RevenueTypeEnum type)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<SearchItemOutputDto>> SearchItem(SearhTourItemInputDto input, bool checkHostUser)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<SearchHotelOutputDto>> SearchHotel(SearhTourItemInputDto input, bool checkHostUser)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<SearchTransportOutputDto>> SearchTransport(SearchTransportInputDto input, bool checkHostUser)
        {
            throw new System.NotImplementedException();
        }

        public async Task<SearchHotelOutputDto> CreateHotel(CreateHotelDto input, bool isFromHost)
        {
            throw new System.NotImplementedException();
        }

        public async Task<SearchTransportOutputDto> CreateTransport(CreateTransportDto input, bool isFromHost)
        {
            throw new System.NotImplementedException();
        }

        public async Task<SearchItemOutputDto> SaveActivity(CreateActivityDto input, bool isFromHost)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<BasicItemDto>> GetSearchCategory()
        {
            throw new System.NotImplementedException();
        }

        public async Task<CustomizeTripOutputDto> GetTripForEditOrCustomize(long tourId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<decimal> GetFeeByHostUser(long userId, long price)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<FeeConfigDto>> GetFeeConfigs(List<long> userIds)
        {
            throw new System.NotImplementedException();
        }
    }
}