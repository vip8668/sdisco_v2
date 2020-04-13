using Abp.Application.Services.Dto;
using SDisco.Home.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Tour
{
    public class PlaceAppService : SdiscoAppServiceBase, IPlaceAppService
    {
        public Task<GuestPhotoDto> GetGuestPhoto(GetGuestPhotoInput input)
        {
            throw new NotImplementedException();
        }

        public Task<PlaceDetailDto> GetPlaceDetail(long placeId)
        {
            throw new NotImplementedException();
        }

        public Task<List<BasicTourItemDto>> GetRelatePlace(long placeId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<ReviewDetailDto>> GetReviewDetail(GetReviewDetailInput input)
        {
            throw new NotImplementedException();
        }

        public Task<List<BasicTourDto>> GetSuggestTour(long activityId)
        {
            throw new NotImplementedException();
        }
    }
}
