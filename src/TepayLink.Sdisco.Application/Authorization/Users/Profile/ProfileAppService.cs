using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Localization;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SDisco.Home.Dto;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.Authentication.TwoFactor.Google;
using TepayLink.Sdisco.Authorization.Roles;
using TepayLink.Sdisco.Authorization.Roles.Dto;
using TepayLink.Sdisco.Authorization.Users.Dto;
using TepayLink.Sdisco.Authorization.Users.Profile.Cache;
using TepayLink.Sdisco.Authorization.Users.Profile.Dto;
using TepayLink.Sdisco.Cashout;
using TepayLink.Sdisco.Friendships;
using TepayLink.Sdisco.Gdpr;
using TepayLink.Sdisco.Identity;
using TepayLink.Sdisco.Net.Sms;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Security;
using TepayLink.Sdisco.Storage;
using TepayLink.Sdisco.Timing;
using TepayLink.Sdisco.Tour;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Authorization.Users.Profile
{
    [AbpAuthorize]
    public class ProfileAppService : SdiscoAppServiceBase, IProfileAppService
    {
        private const int MaxProfilPictureBytes = 5242880; //5MB
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITimeZoneService _timeZoneService;
        private readonly IFriendshipManager _friendshipManager;
        private readonly GoogleTwoFactorAuthenticateService _googleTwoFactorAuthenticateService;
        private readonly ISmsSender _smsSender;
        private readonly ICacheManager _cacheManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<ApplicationLanguage> _languageRepository;

        private readonly IRepository<UserRole, long> _userRoleRepository;

        private readonly IRepository<UserReview, long> _userReviewRepository;

        private readonly IRepository<Product, long> _tourRepository;
        private readonly IRepository<ApplicationLanguage> _langRepository;
        private readonly IRepository<Place, long> _placeRepository;
        private readonly IRepository<Category> _tourCategoryRepository;

        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<MyUserLogin, long> _userLoginRepository;

        private readonly ICommonAppService _commonAppService;

        private readonly IRepository<CashoutMethodType> _cashoutMethodTypeRepository;
        private readonly IRepository<UserDefaultCashoutMethodType, long> _userDefaultCashoutMethodTypeRepository;
        private readonly RoleManager _roleManager;

        private readonly IRepository<UserReviewDetail, long> _userReviewDetailRepository;

        public ProfileAppService(IBinaryObjectManager binaryObjectManager, ITimeZoneService timeZoneService,
            IFriendshipManager friendshipManager, GoogleTwoFactorAuthenticateService googleTwoFactorAuthenticateService,
            ISmsSender smsSender, ICacheManager cacheManager, ITempFileCacheManager tempFileCacheManager,
            IBackgroundJobManager backgroundJobManager, IRepository<Country> countryRepository,
            IRepository<ApplicationLanguage> languageRepository, IRepository<UserRole, long> userRoleRepository,
            IRepository<UserReview, long> userReviewRepository, IRepository<Product, long> tourRepository,
            IRepository<ApplicationLanguage> langRepository, IRepository<Place, long> placeRepository,
            IRepository<Category> tourCategoryRepository, IRepository<User, long> userRepository,
            ICommonAppService commonAppService, IRepository<CashoutMethodType> cashoutMethodTypeRepository,
            IRepository<UserDefaultCashoutMethodType, long> userDefaultCashoutMethodTypeRepository,
            RoleManager roleManager, IRepository<UserReviewDetail, long> userReviewDetailRepository,
            IRepository<MyUserLogin, long> userLoginRepository)
        {
            _binaryObjectManager = binaryObjectManager;
            _timeZoneService = timeZoneService;
            _friendshipManager = friendshipManager;
            _googleTwoFactorAuthenticateService = googleTwoFactorAuthenticateService;
            _smsSender = smsSender;
            _cacheManager = cacheManager;
            _tempFileCacheManager = tempFileCacheManager;
            _backgroundJobManager = backgroundJobManager;
            _countryRepository = countryRepository;
            _languageRepository = languageRepository;
            _userRoleRepository = userRoleRepository;
            _userReviewRepository = userReviewRepository;
            _tourRepository = tourRepository;
            _langRepository = langRepository;
            _placeRepository = placeRepository;
            _tourCategoryRepository = tourCategoryRepository;
            _userRepository = userRepository;
            _commonAppService = commonAppService;
            _cashoutMethodTypeRepository = cashoutMethodTypeRepository;
            _userDefaultCashoutMethodTypeRepository = userDefaultCashoutMethodTypeRepository;
            _roleManager = roleManager;
            _userReviewDetailRepository = userReviewDetailRepository;
            _userLoginRepository = userLoginRepository;
        }


        [DisableAuditing]
        public async Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit()
        {
            var user = await GetCurrentUserAsync();
            var userProfileEditDto = ObjectMapper.Map<CurrentUserProfileEditDto>(user);

            userProfileEditDto.QrCodeSetupImageUrl = user.GoogleAuthenticatorKey != null
                ? _googleTwoFactorAuthenticateService.GenerateSetupCode("TepayLink.Sdisco",
                    user.EmailAddress, user.GoogleAuthenticatorKey, 300, 300).QrCodeSetupImageUrl
                : "";
            userProfileEditDto.IsGoogleAuthenticatorEnabled = user.GoogleAuthenticatorKey != null;

            if (Clock.SupportsMultipleTimezone)
            {
                userProfileEditDto.Timezone = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);

                var defaultTimeZoneId = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                if (userProfileEditDto.Timezone == defaultTimeZoneId)
                {
                    userProfileEditDto.Timezone = string.Empty;
                }
            }

            return userProfileEditDto;
        }

        public async Task DisableGoogleAuthenticator()
        {
            var user = await GetCurrentUserAsync();
            user.GoogleAuthenticatorKey = null;
        }

        public async Task<UpdateGoogleAuthenticatorKeyOutput> UpdateGoogleAuthenticatorKey()
        {
            var user = await GetCurrentUserAsync();
            user.GoogleAuthenticatorKey = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            CheckErrors(await UserManager.UpdateAsync(user));

            return new UpdateGoogleAuthenticatorKeyOutput
            {
                QrCodeSetupImageUrl = _googleTwoFactorAuthenticateService.GenerateSetupCode("TepayLink.Sdisco",
                    user.EmailAddress, user.GoogleAuthenticatorKey, 300, 300).QrCodeSetupImageUrl
            };
        }

        public async Task SendVerificationSms(SendVerificationSmsInputDto input)
        {
            var code = RandomHelper.GetRandom(100000, 999999).ToString();
            var cacheKey = AbpSession.ToUserIdentifier().ToString();
            var cacheItem = new SmsVerificationCodeCacheItem { Code = code };

            _cacheManager.GetSmsVerificationCodeCache().Set(
                cacheKey,
                cacheItem
            );

            await _smsSender.SendAsync(input.PhoneNumber, L("SmsVerificationMessage", code));
        }

        public async Task VerifySmsCode(VerifySmsCodeInputDto input)
        {
            var cacheKey = AbpSession.ToUserIdentifier().ToString();
            var cash = await _cacheManager.GetSmsVerificationCodeCache().GetOrDefaultAsync(cacheKey);

            if (cash == null)
            {
                throw new Exception("Phone number confirmation code is not found in cache !");
            }

            if (input.Code != cash.Code)
            {
                throw new UserFriendlyException(L("WrongSmsVerificationCode"));
            }

            var user = await UserManager.GetUserAsync(AbpSession.ToUserIdentifier());
            user.IsPhoneNumberConfirmed = true;
            user.PhoneNumber = input.PhoneNumber;
            await UserManager.UpdateAsync(user);
        }

        public async Task PrepareCollectedData()
        {
            await _backgroundJobManager.EnqueueAsync<UserCollectedDataPrepareJob, UserIdentifier>(AbpSession.ToUserIdentifier());
        }

        public async Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input)
        {
            var user = await GetCurrentUserAsync();

            if (user.PhoneNumber != input.PhoneNumber)
            {
                input.IsPhoneNumberConfirmed = false;
            }
            else if (user.IsPhoneNumberConfirmed)
            {
                input.IsPhoneNumberConfirmed = true;
            }

            ObjectMapper.Map(input, user);
            CheckErrors(await UserManager.UpdateAsync(user));

            if (Clock.SupportsMultipleTimezone)
            {
                if (input.Timezone.IsNullOrEmpty())
                {
                    var defaultValue = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), TimingSettingNames.TimeZone, defaultValue);
                }
                else
                {
                    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), TimingSettingNames.TimeZone, input.Timezone);
                }
            }
        }

        public async Task ChangePassword(ChangePasswordInput input)
        {
            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await GetCurrentUserAsync();
            if (await UserManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                CheckErrors(await UserManager.ChangePasswordAsync(user, input.NewPassword));
            }
            else
            {
                CheckErrors(IdentityResult.Failed(new IdentityError
                {
                    Description = "Incorrect password."
                }));
            }
        }

        public async Task UpdateProfilePicture(UpdateProfilePictureInput input)
        {
            byte[] byteArray;

            var imageBytes = _tempFileCacheManager.GetFile(input.FileToken);

            if (imageBytes == null)
            {
                throw new UserFriendlyException("There is no such image file with the token: " + input.FileToken);
            }

            using (var bmpImage = new Bitmap(new MemoryStream(imageBytes)))
            {
                var width = (input.Width == 0 || input.Width > bmpImage.Width) ? bmpImage.Width : input.Width;
                var height = (input.Height == 0 || input.Height > bmpImage.Height) ? bmpImage.Height : input.Height;
                var bmCrop = bmpImage.Clone(new Rectangle(input.X, input.Y, width, height), bmpImage.PixelFormat);

                using (var stream = new MemoryStream())
                {
                    bmCrop.Save(stream, bmpImage.RawFormat);
                    byteArray = stream.ToArray();
                }
            }

            if (byteArray.Length > MaxProfilPictureBytes)
            {
                throw new UserFriendlyException(L("ResizedProfilePicture_Warn_SizeLimit", AppConsts.ResizedMaxProfilPictureBytesUserFriendlyValue));
            }

            var user = await UserManager.GetUserByIdAsync(AbpSession.GetUserId());

            if (user.ProfilePictureId.HasValue)
            {
                await _binaryObjectManager.DeleteAsync(user.ProfilePictureId.Value);
            }

            var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
            await _binaryObjectManager.SaveAsync(storedFile);

            user.ProfilePictureId = storedFile.Id;
        }

        [AbpAllowAnonymous]
        public async Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting()
        {
            var passwordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit),
                RequireLowercase = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase),
                RequiredLength = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength)
            };

            return new GetPasswordComplexitySettingOutput
            {
                Setting = passwordComplexitySetting
            };
        }


        /// <summary>
        /// Lấy thông tin điểm, ranking và rating của user 
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<GetRankingAndPointDto> GetRankingAndPoint(long userId = 0)
        {
            var user = await UserManager.GetUserByIdAsync(userId == 0 ? AbpSession.GetUserId() : userId);
            return new GetRankingAndPointDto
            {
                Point = user.Point,
                Ranking = user.Ranking,
                Rating = user.Rating ?? 0
            };
        }

        [DisableAuditing]
        public async Task<GetProfilePictureOutput> GetProfilePicture()
        {
            var user = await UserManager.GetUserByIdAsync(AbpSession.GetUserId());
            if (user.ProfilePictureId == null)
            {
                return new GetProfilePictureOutput(string.Empty);
            }

            return await GetProfilePictureById(user.ProfilePictureId.Value);
        }

        public async Task<GetProfilePictureOutput> GetFriendProfilePictureById(GetFriendProfilePictureByIdInput input)
        {
            if (!input.ProfilePictureId.HasValue || await _friendshipManager.GetFriendshipOrNullAsync(AbpSession.ToUserIdentifier(), new UserIdentifier(input.TenantId, input.UserId)) == null)
            {
                return new GetProfilePictureOutput(string.Empty);
            }

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var bytes = await GetProfilePictureByIdOrNull(input.ProfilePictureId.Value);
                if (bytes == null)
                {
                    return new GetProfilePictureOutput(string.Empty);
                }

                return new GetProfilePictureOutput(Convert.ToBase64String(bytes));
            }
        }

        [AbpAllowAnonymous]
        public async Task<GetProfilePictureOutput> GetProfilePictureById(Guid profilePictureId)
        {
            return await GetProfilePictureByIdInternal(profilePictureId);
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        /// <summary>
        /// Lấy thông tin profile của 1 user  bất kỳ
        /// </summary>
        /// <param name="userId"> userId</param>
        /// <returns>
        /// code: 11: user không tồn tại
        /// </returns>
        [AbpAllowAnonymous]
        public async Task<GetProfileViewDto> GetProfileView(long userId)
        {
            var user = UserManager.Users.FirstOrDefault(p => p.Id == userId);
            if (user != null)
            {
                GetProfileViewDto profile = new GetProfileViewDto
                {
                    AboutMe = user.AboutMe,
                    FullName = user.FullName,
                    VerifyEmail = user.IsEmailConfirmed,
                    VerifyPhone = user.IsPhoneNumberConfirmed,
                    Ranking = user.Ranking,
                    Rating = user.Rating ?? 0,
                    VerifyGovermentId = false,
                    VerifySocialMedia = false,
                    Work = user.Work,
                    UserType = user.UserType,
                    Point = user.Point,

                };
                //todo add thêm language và quốc gia

                if (user.ContryId != null && user.ContryId != 0)
                {
                    var country = _countryRepository.FirstOrDefault(user.ContryId ?? 0);
                    if (country != null)
                    {
                        profile.LiveIn = country.DisplayName;
                    }
                }

                try
                {
                    var languageIds = user.LanguageSpeak.Split(',').ToList().Select(p => int.Parse(p)).ToList();
                    var languages = string.Join(",",
                        _languageRepository.GetAll().Where(p => languageIds.Contains(p.Id)).Select(p => p.DisplayName));
                    profile.Languages = languages;
                }
                catch (Exception e)
                {
                }

                return profile;
            }
            else
            {
                throw new UserFriendlyException(L("UserNotExist"));
            }
        }


        [AbpAllowAnonymous]
        public async Task<GetProfileViewDto> GetCurrentProfile(long userId = 0)
        {
            var user = await UserManager.GetUserByIdAsync(userId == 0 ? AbpSession.GetUserId() : userId);
            if (user != null)
            {
                GetProfileViewDto profile = new GetProfileViewDto
                {
                    AboutMe = user.AboutMe,
                    FullName = user.FullName,
                    VerifyEmail = user.IsEmailConfirmed,
                    VerifyPhone = user.IsPhoneNumberConfirmed,
                    Ranking = user.Ranking,
                    Rating = user.Rating ?? 0,
                    VerifyGovermentId = false,
                    VerifySocialMedia = false,
                    Work = user.Work,
                    UserType = user.UserType,
                    Avatar = user.Avatar,

                };

                if (user.ContryId != null && user.ContryId != 0)
                {
                    var country = _countryRepository.FirstOrDefault(user.ContryId ?? 0);
                    if (country != null)
                    {
                        profile.LiveIn = country.DisplayName;
                    }
                }

                try
                {
                    var languageIds = user.LanguageSpeak.Split(',').ToList().Select(p => int.Parse(p)).ToList();
                    var languages = string.Join(",",
                        _languageRepository.GetAll().Where(p => languageIds.Contains(p.Id)).Select(p => p.DisplayName));
                    profile.Languages = languages;
                }
                catch (Exception e)
                {
                }

                return profile;
            }
            else
            {
                throw new UserFriendlyException(L("UserNotExist"));
            }
        }

        /// <summary>
        /// Lấy đánh giá user
        /// </summary>
        /// <param name="userId"> User Id</param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<UserReviewDto> GetUserReviewSummary(long userId = 0)
        {
            var userreview = _userReviewRepository.FirstOrDefault(p => p.UserId == (userId == 0 ? AbpSession.UserId : userId));
            if (userreview != null)
            {
                return new UserReviewDto
                {
                    UserId = AbpSession.UserId ?? 0,
                    Food = userreview.Food,
                    GuideTour = userreview.GuideTour,
                    Itineraty = userreview.Itineraty,
                    Rating = userreview.Rating,
                    ReviewCount = userreview.ReviewCount,
                    Service = userreview.Service,
                    Transport = userreview.Transport
                };
            }
            else
            {
                return new UserReviewDto
                {
                    UserId = AbpSession.UserId ?? 0,
                    Food = 0,
                    GuideTour = 0,
                    Itineraty = 0,
                    Rating = 0,
                    ReviewCount = 0,
                    Service = 0,
                    Transport = 0
                };
            }
        }


        /// <summary>
        /// Chi tiết đánh giá User+ comment
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        // [AbpAllowAnonymous]
        public async Task<PagedResultDto<UserReviewDetailDto>> GetUserReviewDetail(GetUserReviewDetailInput input)
        {
            IQueryable<UserReviewDetail> query = _userReviewDetailRepository.GetAll()
                .Where(p => p.UserId == (input.UserId ?? AbpSession.UserId));
            long totalcount = query.Count();
            query = query.OrderByDescending(p => p.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
            var userReviewDetails = query.ToList();
            PagedResultDto<UserReviewDetailDto> pagedResultDto = new PagedResultDto<UserReviewDetailDto>();
            var userIds = userReviewDetails.Select(p => p.CreatorUserId).ToList();
            var users = UserManager.Users.Where(p => userIds.Contains(p.Id)).ToList();

            pagedResultDto.TotalCount = (int)totalcount;


            var result = (from p in userReviewDetails
                          let user = users.FirstOrDefault(q => q.Id == p.CreatorUserId)
                          select new UserReviewDetailDto
                          {
                              Avatar = user.Avatar,
                              Comment = p.Comment,
                              Food = p.Food,
                              Reviewer = user.FullName,
                              ReviewDate = p.CreationTime,
                              GuideTour = p.GuideTour,
                              Itineraty = p.Itineraty,
                              Rating = p.Rating,
                              Service = p.Service,
                              Title = p.Title,
                              Transport = p.Transport,
                              UserId = p.UserId
                          }).ToList();

            pagedResultDto.Items = result;
            return pagedResultDto;
        }



        /// <summary>
        /// Danh sách chuyến đi đã tạo của user
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<BasicTourDto>> GetTripCrated(GetTripCreated input)
        {
            var query =
                (from t in
                        _tourRepository.GetAll()
                 join l in _langRepository.GetAll() on t.LanguageId equals l.Id
                 join c in _tourCategoryRepository.GetAll() on t.CategoryId equals c.Id
                 join p in _placeRepository.GetAll() on t.PlaceId equals p.Id
                 where t.Type == ProductTypeEnum.Tour
                       && t.Status == ProductStatusEnum.Publish
                       && t.HostUserId == (input.UserId ?? AbpSession.UserId)
                 select new BasicTourDto
                 {
                     Id = t.Id,
                     CategoryId = c.Id,
                     CategoryName = c.Name,
                     PlaceId = p.Id,
                     PlaceName = p.Name,
                     OfferLanguageId = t.LanguageId??0,
                     Title = t.Name,

                     LanguageOffer = l.DisplayName,
                     SoldCount = t.BookingCount,
                     TripLength = t.TripLengh,

                     IsHotDeal = t.IsHotDeal,
                     BestSaller = t.IsBestSeller,
                     ShareCount = t.ShareCount,
                     CoppyCount = t.CoppyCount
                 });

            var total = query.Count();
            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var itemIds = list.Select((p => p.Id)).ToList();
            var listSaveItem = await _commonAppService.GetSaveItem(itemIds);

            foreach (var item in list)
            {
                var reviewItem = await _commonAppService.GetTourReviewSummary(item.Id);
                item.Review = reviewItem;
                item.ThumbImages = await _commonAppService.GetTourThumbPhoto(item.Id);
                item.IsLove = listSaveItem.FirstOrDefault(p => p.ItemId == item.Id) != null;
                item.AvaiableTimes = await _commonAppService.GetAvaiableTimeOfTour(item.Id);
            }

            return new PagedResultDto<BasicTourDto>()
            {
                Items = list,
                TotalCount = total
            };
        }


        /// <summary>
        /// Lấy Setting Notification
        /// Key Setting
        /// notification.sms : sms
        /// notification.app : app
        /// notification.email : email
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> GetNotificationSetting(GetNotifycationSettingDto input)
        {
            return true;
        }


        /// <summary>
        /// UPdate notification Setting
        /// Key Setting
        /// notification.sms : sms
        /// notification.app : app
        /// notification.email : email
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> UpdateNotificationSetting(UpdateNotificationSettingInput input)
        {
            return input.Value;
        }



        /// <summary>
        /// Danh sách role của Host
        /// </summary>
        /// <returns></returns>
        public async Task<ListResultDto<RoleListDto>> GetRoles()
        {
            var query = _roleManager.Roles.Where(p =>
                new[] { "HostAdmin", "Editor", "Accountant", "Moderator" }.Contains(p.Name));
            var roles = await query.ToListAsync();
            return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
        }

        /// <summary>
        /// Search user theo email, user name
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<BasicHostUserInfo>> GetUserByUsernameOrEmail(GetUserInput input)
        {
            var userList = _userRepository.GetAll().Where(p => p.EmailAddress != null &&
                                                               p.EmailAddress.Contains((input.UsernameOrEmail ?? "")
                                                                   .Trim()) &&
                                                               (p.HostUserId == null ||
                                                                p.HostUserId == AbpSession.UserId)).Select(p =>
                new BasicHostUserInfo
                {
                    FullName = p.FullName,
                    Avarta = p.Avatar,
                    UserId = p.Id
                }).ToList();
            return userList;
        }

        /// <summary>
        /// Danh sách user theo role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<List<BasicHostUserInfo>> GetUserByrole(string roleName)
        {
            var role = _roleManager.Roles.FirstOrDefault(p => p.Name == roleName);
            if (role == null)
            {
                return new List<BasicHostUserInfo>();
            }

            var listUser = UserManager.Users
                .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
                .Where(p => p.HostUserId == AbpSession.UserId).Select(p => new BasicHostUserInfo
                {
                    Avarta = p.Avatar,
                    FullName = p.FullName,
                    UserId = p.Id
                }).ToList();

            return listUser;
        }

        /// <summary>
        /// Thêm role cho user
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>

        [UnitOfWork]
        public async Task SetRole(List<SetRoleDto> roleDtos)
        {
            foreach (var roleDto in roleDtos)
            {


                if (!new[] { "HostAdmin", "Editor", "Accountant", "Moderator" }.Contains(roleDto.RoleName))
                    throw new UserFriendlyException("Invalid role");
                var user = UserManager.Users.FirstOrDefault(
                    p => p.Id == roleDto.UserId);
                if (user == null || (user.HostUserId != null && user.HostUserId != AbpSession.UserId))
                {
                    throw new UserFriendlyException("Không thể thêm quyền cho user này");
                }

                if (user.HostUserId == null)
                {
                    user.HostUserId = AbpSession.UserId;
                    await UserManager.UpdateAsync(user);
                }

                await UserManager.AddToRoleAsync(user, roleDto.RoleName);
            }
        }

        /// <summary>
        /// Xóa role của user
        /// </summary>
        /// <param name="roleDto"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task RemoveRole(SetRoleDto roleDto)
        {
            if (!new[] { "HostAdmin", "Editor", "Accountant", "Moderator" }.Contains(roleDto.RoleName))
                throw new UserFriendlyException("Invalid role");


            var role = _roleManager.Roles.FirstOrDefault(p => p.Name == roleDto.RoleName);
            if (role == null)
            {
                throw new UserFriendlyException("Invalid role");
            }

            var user = UserManager.Users.FirstOrDefault(
                p => p.Id == roleDto.UserId && p.HostUserId == AbpSession.UserId);
            if (user == null)
            {
                throw new UserFriendlyException("Không thể xóa quyền cho user này");
            }

            _userRoleRepository.Delete(p => p.UserId == roleDto.UserId && p.RoleId == role.Id);
        }


        public async Task<List<CashoutTypeDto>> GetCashoutType()
        {
            var list = _cashoutMethodTypeRepository.GetAll().Select(p => new CashoutTypeDto
            {
                Id = p.Id,
                Title = p.Title,
                Note = p.Note
            }).ToList();
            var exist = _userDefaultCashoutMethodTypeRepository.FirstOrDefault(p => p.UserId == AbpSession.UserId);
            if (exist != null)
            {
                var item = list.FirstOrDefault(p => p.Id == exist.CashoutMethodTypeId);
                if (item != null)
                    item.IsChecked = true;
            }
            return list;
        }

        public async Task SaveCashoutType(CashoutTypeInputDto inputDto)
        {
            var exist = _userDefaultCashoutMethodTypeRepository.FirstOrDefault(p => p.UserId == AbpSession.UserId);
            if (exist != null)
            {
                exist.CashoutMethodTypeId = inputDto.CashoutTypeId;
                _userDefaultCashoutMethodTypeRepository.Update(exist);
            }
            else
            {
                exist = new UserDefaultCashoutMethodType
                {
                    UserId = AbpSession.UserId ?? 0,
                    CashoutMethodTypeId = inputDto.CashoutTypeId

                };
                _userDefaultCashoutMethodTypeRepository.Insert(exist);
            }
        }


        /// <summary>
        /// Lấy Setting Gửi Notifycation SMS
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetNotificationSmsSetting()
        {
            return await SettingManager.GetSettingValueForUserAsync<bool>("notification.sms",
                AbpSession.ToUserIdentifier());
        }

        /// <summary>
        /// Lấy Setting Gửi Notifycation Email
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetNotificationEmailSetting()
        {
            return await SettingManager.GetSettingValueForUserAsync<bool>("notification.email",
                AbpSession.ToUserIdentifier());
        }


        /// <summary>
        /// Update Setting SMS
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateNotificationSmsSetting(bool value)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), "notification.sms",
                value.ToString());
            return value;
        }

        /// <summary>
        /// Update Setting Email
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateNotificationEmailSetting(bool value)
        {
            SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), "notification.email",
                value.ToString());
            return value;
        }

        private async Task<byte[]> GetProfilePictureByIdOrNull(Guid profilePictureId)
        {
            var file = await _binaryObjectManager.GetOrNullAsync(profilePictureId);
            if (file == null)
            {
                return null;
            }

            return file.Bytes;
        }

        private async Task<GetProfilePictureOutput> GetProfilePictureByIdInternal(Guid profilePictureId)
        {
            var bytes = await GetProfilePictureByIdOrNull(profilePictureId);
            if (bytes == null)
            {
                return new GetProfilePictureOutput(string.Empty);
            }

            return new GetProfilePictureOutput(Convert.ToBase64String(bytes));
        }
        [DisableAuditing]
        public async Task<GetProfilePictureOutput> GetProfilePicture(long userid)
        {
            var user = await UserManager.GetUserByIdAsync(userid == 0 ? AbpSession.GetUserId() : userid);
            return new GetProfilePictureOutput(user.Avatar);

        }

        // <summary>
        /// Kết nối fb/ google
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [AbpAuthorize]
        public async Task ConnectFbGG(ConnectFbDto model)
        {
            // var externalUser = await GetExternalUserInfo(model);



            var loginUser = _userLoginRepository.FirstOrDefault(p => p.ProviderKey == model.ProviderKey && p.LoginProvider == model.AuthProvider);
            if (loginUser != null)
            {
                throw new UserFriendlyException($"Tài khoản {model.AuthProvider} này đã được kết nối với một tài khoản khác. Vui lòng thử lại");
            }

            loginUser = new MyUserLogin
            {
                LoginProvider = model.AuthProvider,
                AccessCode = model.ProviderAccessCode,
                ProviderKey = model.ProviderKey,
                UserId = AbpSession.UserId ?? 0
            };

            _userLoginRepository.Insert(loginUser);

        }

    }
}
