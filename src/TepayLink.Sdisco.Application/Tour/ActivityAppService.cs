using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Localization;
using SDisco.Home.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Tour
{
    public class ActivityAppService : SdiscoAppServiceBase, IActivityAppService
    {
      
        private readonly IRepository<Product, long> _tourRepository;
        private readonly IRepository<User, long> _userRepository;

        private readonly IRepository<ApplicationLanguage> _langRepository;
  
        private readonly IRepository<Place, long> _placeRepository;
        private readonly IRepository<ProductSchedule, long> _productScheduleRepository;
        private readonly ICommonAppService _commonAppService;
        private readonly IRepository<Category> _tourCategoryRepository;

        public ActivityAppService(IRepository<Product, long> tourRepository, IRepository<User, long> userRepository, IRepository<ApplicationLanguage> langRepository, IRepository<Place, long> placeRepository, IRepository<ProductSchedule, long> productScheduleRepository, ICommonAppService commonAppService, IRepository<Category> tourCategoryRepository)
        {
            _tourRepository = tourRepository;
            _userRepository = userRepository;
            _langRepository = langRepository;
            _placeRepository = placeRepository;
            _productScheduleRepository = productScheduleRepository;
            _commonAppService = commonAppService;
            _tourCategoryRepository = tourCategoryRepository;
        }

        public async Task<ActivityDetailDto> GetActivityDetail(long activityId)
        {
            var itemSchedule = _productScheduleRepository.GetAll().Where(p => p.ProductId == activityId).OrderBy(p => p.Price)
                .FirstOrDefault();

            var tourItem = (from p in _tourRepository.GetAll()
                            join l in _langRepository.GetAll() on p.LanguageId equals l.Id
                            join place in _placeRepository.GetAll() on p.PlaceId equals place.Id
                            join user in _userRepository.GetAll() on p.HostUserId equals user.Id
                            where p.Id == activityId
                            select new ActivityDetailDto
                            {
                                Id = p.Id,

                                Name = p.Name,
                                BookCount = p.BookingCount,

                                Language = l.DisplayName,
                                Location = new BasicLocationDto
                                {
                                    Id = place.Id,
                                    Name = p.Name,
                                    Addess = place.DisplayAddress,
                                    Lat = place.Lat,
                                    Long = place.Long
                                },
                                HostUser = new BasicHostUserInfo
                                {
                                    Ranking = user.Ranking,
                                    Ratting = user.Rating ?? 0,
                                    FullName = user.FullName,
                                    Avarta = user.Avatar,
                                    UserId = user.Id
                                },
                                WhatElseShouldKnow = p.ExtraData,
                                WhatWeDo = p.WhatWeDo,
                                Policies = p.Policies,
                                InstantBook = p.InstantBook
                            }).FirstOrDefault();
            if (tourItem == null)
                return null;
            tourItem.Price =
                new BasicPriceDto
                {
                    Price = itemSchedule != null ? itemSchedule.Price : 0,
                    ServiceFee =0,// itemSchedule != null ? itemSchedule.ServiceFee : 0,
                    OldPrice =0,// itemSchedule != null ? itemSchedule.OldPrice : 0,
                };

            tourItem.Images = await _commonAppService.GetTourItemPhoto(activityId);
            tourItem.Review = await _commonAppService.GetTourItemReviewSummary(activityId);
            tourItem.IsLove = await _commonAppService.IsSave(activityId);
            return tourItem;
        }

        public async Task<PagedResultDto<ReviewDetailDto>> GetReviewDetail(GetReviewDetailInput input)
        {
            return await _commonAppService.GetTourItemReviewDetail(input);
        }

        public async Task<GuestPhotoDto> GetGuestPhoto(GetGuestPhotoInput input)
        {
            return await _commonAppService.GetTourItemGuestPhoto(input);
        }


        public async Task<List<BasicTourItemDto>> GetRelateActivity(long activityId)
        {
            return await _commonAppService.GetRelateTourItem(activityId);
        }

        public async Task<List<BasicTourDto>> GetSuggestTour(long activityId)
        {
            return await _commonAppService.GetRelateTour(activityId);
        }

        public async Task<LastBookedDto> GetLastBooked(long activityId)
        {
            var item = _tourRepository.GetAll().Where(p => p.Id == activityId).Select(p => new LastBookedDto
            {
                TotalBook = p.BookingCount,
                LastBook = p.LastBookTime
            }).FirstOrDefault();
            return item;
        }


    }
}
