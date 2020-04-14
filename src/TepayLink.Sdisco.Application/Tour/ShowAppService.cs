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
    public class ShowAppService : SdiscoAppServiceBase, IShowAppService
    {
       
        private readonly IRepository<Product, long> _tourItemRepository;
        private readonly IRepository<ProductImage, long> _imageRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ApplicationLanguage> _langRepository;
        private readonly IRepository<ProductSchedule,long> _itemScheduleRepository;
        private readonly IRepository<Place, long> _placeRepository;
        private readonly IRepository<Category> _tourItemCategoryRepository;
        private readonly ICommonAppService _commonAppService;

        public ShowAppService(IRepository<Product, long> tourItemRepository, IRepository<ProductImage, long> imageRepository, IRepository<User, long> userRepository, IRepository<ApplicationLanguage> langRepository, IRepository<ProductSchedule, long> itemScheduleRepository, IRepository<Place, long> placeRepository, IRepository<Category> tourItemCategoryRepository, ICommonAppService commonAppService)
        {
            _tourItemRepository = tourItemRepository;
            _imageRepository = imageRepository;
            _userRepository = userRepository;
            _langRepository = langRepository;
            _itemScheduleRepository = itemScheduleRepository;
            _placeRepository = placeRepository;
            _tourItemCategoryRepository = tourItemCategoryRepository;
            _commonAppService = commonAppService;
        }

        public async Task<ShowDetailDetailDto> GetShowDetail(long showId)
        {
            var itemSchedule = _itemScheduleRepository.GetAll().Where(p => p.ProductId == showId).OrderBy(p => p.Price)
                .FirstOrDefault();
            var tourItem = (from p in _tourItemRepository.GetAll()
                            join l in _langRepository.GetAll() on p.LanguageId equals l.Id
                            join place in _placeRepository.GetAll() on p.PlaceId equals place.Id
                            join user in _userRepository.GetAll() on p.HostUserId equals user.Id
                            where p.Id == showId
                            select new ShowDetailDetailDto
                            {
                                Id = p.Id,

                                Overview = p.Description,
                                Name = p.Name,
                                Language = l.DisplayName,
                                Location = new BasicLocationDto
                                {
                                    Id = place.Id,
                                    Name = place.Name,
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

                                WhatExpect = p.ExtraData,
                                InstantBook = p.InstantBook
                            }).FirstOrDefault();
            if (tourItem == null)
                return null;

            tourItem.Price =
                new BasicPriceDto
                {
                    Price = itemSchedule != null ? itemSchedule.Price : 0,
                    ServiceFee =0, //itemSchedule != null ? itemSchedule.ServiceFee : 0,
                    OldPrice =0,// itemSchedule != null ? itemSchedule.OldPrice : 0,
                };

            tourItem.Images = await _commonAppService.GetTourItemPhoto(showId);
            tourItem.IsLove = await _commonAppService.IsSave(showId);
            tourItem.Review = await _commonAppService.GetTourItemReviewSummary(showId);
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


        public async Task<List<BasicTourItemDto>> GetPopularShow(long showId)
        {
            return await _commonAppService.GetRelateTourItem(showId);

        }

        public async Task<List<BasicTourDto>> GetRelateTour(long showId)
        {
            return await _commonAppService.GetRelateTour(showId);
        }
    }
}
