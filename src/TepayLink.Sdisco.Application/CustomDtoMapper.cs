using TepayLink.Sdisco.KOL.Dtos;
using TepayLink.Sdisco.KOL;
using TepayLink.Sdisco.Affiliate.Dtos;
using TepayLink.Sdisco.Affiliate;
using TepayLink.Sdisco.Places.Dtos;
using TepayLink.Sdisco.Places;
using TepayLink.Sdisco.Chat.Dtos;
using TepayLink.Sdisco.Cashout.Dtos;
using TepayLink.Sdisco.Cashout;
using TepayLink.Sdisco.Search.Dtos;
using TepayLink.Sdisco.Search;
using TepayLink.Sdisco.Client.Dtos;
using TepayLink.Sdisco.Client;
using TepayLink.Sdisco.Bookings.Dtos;


using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Products.Dtos;

using TepayLink.Sdisco.Help.Dtos;
using TepayLink.Sdisco.Help;
using TepayLink.Sdisco.Blog.Dtos;
using TepayLink.Sdisco.Blog;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.AdminConfig.Dtos;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Products;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using TepayLink.Sdisco.Auditing.Dto;
using TepayLink.Sdisco.Authorization.Accounts.Dto;
using TepayLink.Sdisco.Authorization.Permissions.Dto;
using TepayLink.Sdisco.Authorization.Roles;
using TepayLink.Sdisco.Authorization.Roles.Dto;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Authorization.Users.Dto;
using TepayLink.Sdisco.Authorization.Users.Importing.Dto;
using TepayLink.Sdisco.Authorization.Users.Profile.Dto;
using TepayLink.Sdisco.Chat;
using TepayLink.Sdisco.Chat.Dto;
using TepayLink.Sdisco.Editions;
using TepayLink.Sdisco.Editions.Dto;
using TepayLink.Sdisco.Friendships;
using TepayLink.Sdisco.Friendships.Cache;
using TepayLink.Sdisco.Friendships.Dto;
using TepayLink.Sdisco.Localization.Dto;
using TepayLink.Sdisco.MultiTenancy;
using TepayLink.Sdisco.MultiTenancy.Dto;
using TepayLink.Sdisco.MultiTenancy.HostDashboard.Dto;
using TepayLink.Sdisco.MultiTenancy.Payments;
using TepayLink.Sdisco.MultiTenancy.Payments.Dto;
using TepayLink.Sdisco.Notifications.Dto;
using TepayLink.Sdisco.Organizations.Dto;
using TepayLink.Sdisco.Sessions.Dto;
using TepayLink.Sdisco.WebHooks.Dto;

namespace TepayLink.Sdisco
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditTransactionDto, Transaction>().ReverseMap();
            configuration.CreateMap<TransactionDto, Transaction>().ReverseMap();
            configuration.CreateMap<CreateOrEditWithDrawRequestDto, WithDrawRequest>().ReverseMap();
            configuration.CreateMap<WithDrawRequestDto, WithDrawRequest>().ReverseMap();
            configuration.CreateMap<CreateOrEditShareTransactionDto, ShareTransaction>().ReverseMap();
            configuration.CreateMap<ShareTransactionDto, ShareTransaction>().ReverseMap();
            configuration.CreateMap<CreateOrEditPartnerRevenueDto, PartnerRevenue>().ReverseMap();
            configuration.CreateMap<PartnerRevenueDto, PartnerRevenue>().ReverseMap();
            configuration.CreateMap<CreateOrEditShortLinkDto, ShortLink>().ReverseMap();
            configuration.CreateMap<ShortLinkDto, ShortLink>().ReverseMap();
            configuration.CreateMap<CreateOrEditChatMessageV2Dto, ChatMessageV2>().ReverseMap();
            configuration.CreateMap<ChatMessageV2Dto, ChatMessageV2>().ReverseMap();
            configuration.CreateMap<CreateOrEditNearbyPlaceDto, NearbyPlace>().ReverseMap();
            configuration.CreateMap<NearbyPlaceDto, NearbyPlace>().ReverseMap();
            configuration.CreateMap<CreateOrEditSuggestedProductDto, SuggestedProduct>().ReverseMap();
            configuration.CreateMap<SuggestedProductDto, SuggestedProduct>().ReverseMap();
            configuration.CreateMap<CreateOrEditRelatedProductDto, RelatedProduct>().ReverseMap();
            configuration.CreateMap<RelatedProductDto, RelatedProduct>().ReverseMap();
            configuration.CreateMap<CreateOrEditSimilarProductDto, SimilarProduct>().ReverseMap();
            configuration.CreateMap<SimilarProductDto, SimilarProduct>().ReverseMap();
            configuration.CreateMap<CreateOrEditChatconversationDto, Chatconversation>().ReverseMap();
            configuration.CreateMap<ChatconversationDto, Chatconversation>().ReverseMap();
            configuration.CreateMap<CreateOrEditUserDefaultCashoutMethodTypeDto, UserDefaultCashoutMethodType>().ReverseMap();
            configuration.CreateMap<UserDefaultCashoutMethodTypeDto, UserDefaultCashoutMethodType>().ReverseMap();
            configuration.CreateMap<CreateOrEditCashoutMethodTypeDto, CashoutMethodType>().ReverseMap();
            configuration.CreateMap<CashoutMethodTypeDto, CashoutMethodType>().ReverseMap();
            configuration.CreateMap<CreateOrEditTransPortdetailDto, TransPortdetail>().ReverseMap();
            configuration.CreateMap<TransPortdetailDto, TransPortdetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditCouponDto, Coupon>().ReverseMap();
            configuration.CreateMap<CouponDto, Coupon>().ReverseMap();
            configuration.CreateMap<CreateOrEditSearchHistoryDto, SearchHistory>().ReverseMap();
            configuration.CreateMap<SearchHistoryDto, SearchHistory>().ReverseMap();
            configuration.CreateMap<CreateOrEditWalletDto, Wallet>().ReverseMap();
            configuration.CreateMap<WalletDto, Wallet>().ReverseMap();
            configuration.CreateMap<CreateOrEditPartnerShipDto, PartnerShip>().ReverseMap();
            //  configuration.CreateMap<PartnerShipDto, PartnerShip>().ReverseMap();
            configuration.CreateMap<CreateOrEditPartnerDto, Partner>().ReverseMap();
            configuration.CreateMap<PartnerDto, Partner>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderDto, Order>().ReverseMap();
            configuration.CreateMap<OrderDto, Order>().ReverseMap();
            configuration.CreateMap<CreateOrEditClientSettingDto, ClientSetting>().ReverseMap();
            configuration.CreateMap<ClientSettingDto, ClientSetting>().ReverseMap();
            configuration.CreateMap<CreateOrEditBookingClaimDto, BookingClaim>().ReverseMap();
            configuration.CreateMap<BookingClaimDto, BookingClaim>().ReverseMap();
            configuration.CreateMap<CreateOrEditClaimReasonDto, ClaimReason>().ReverseMap();
            configuration.CreateMap<ClaimReasonDto, ClaimReason>().ReverseMap();
            configuration.CreateMap<CreateOrEditBookingDetailDto, BookingDetail>().ReverseMap();
            configuration.CreateMap<BookingDetailDto, BookingDetail>().ReverseMap();
            // configuration.CreateMap<CreateOrEditBookingDto, Booking>().ReverseMap();
            // configuration.CreateMap<BookingDto, Booking>().ReverseMap();
            configuration.CreateMap<CreateOrEditCountryDto, Country>().ReverseMap();
            configuration.CreateMap<CountryDto, Country>().ReverseMap();
            configuration.CreateMap<CreateOrEditCurrencyDto, Currency>().ReverseMap();
            configuration.CreateMap<CurrencyDto, Currency>().ReverseMap();
            configuration.CreateMap<CreateOrEditUserSubcriberDto, UserSubcriber>().ReverseMap();
            configuration.CreateMap<UserSubcriberDto, UserSubcriber>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductScheduleDto, ProductSchedule>().ReverseMap();
            configuration.CreateMap<ProductScheduleDto, ProductSchedule>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductDetailComboDto, ProductDetailCombo>().ReverseMap();
            configuration.CreateMap<ProductDetailComboDto, ProductDetailCombo>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductDetailDto, ProductDetail>().ReverseMap();
            configuration.CreateMap<ProductDetailDto, ProductDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditUserReviewDetailDto, UserReviewDetail>().ReverseMap();
            configuration.CreateMap<UserReviewDetailDto, UserReviewDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditUserReviewDto, UserReview>().ReverseMap();
            configuration.CreateMap<UserReviewDto, UserReview>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductReviewDetailDto, ProductReviewDetail>().ReverseMap();
            configuration.CreateMap<ProductReviewDetailDto, ProductReviewDetail>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductReviewDto, ProductReview>().ReverseMap();
            configuration.CreateMap<ProductReviewDto, ProductReview>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductUtilityDto, ProductUtility>().ReverseMap();
            configuration.CreateMap<ProductUtilityDto, ProductUtility>().ReverseMap();
            configuration.CreateMap<CreateOrEditUtilityDto, Utility>().ReverseMap();
            configuration.CreateMap<UtilityDto, Utility>().ReverseMap();
            configuration.CreateMap<CreateOrEditSaveItemDto, SaveItem>().ReverseMap();
            configuration.CreateMap<SaveItemDto, SaveItem>().ReverseMap();
            configuration.CreateMap<CreateOrEditHelpContentDto, HelpContent>().ReverseMap();
            configuration.CreateMap<HelpContentDto, HelpContent>().ReverseMap();
            configuration.CreateMap<CreateOrEditHelpCategoryDto, HelpCategory>().ReverseMap();
            configuration.CreateMap<HelpCategoryDto, HelpCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditBlogProductRelatedDto, BlogProductRelated>().ReverseMap();
            configuration.CreateMap<BlogProductRelatedDto, BlogProductRelated>().ReverseMap();
            configuration.CreateMap<CreateOrEditBlogCommentDto, BlogComment>().ReverseMap();
            configuration.CreateMap<BlogCommentDto, BlogComment>().ReverseMap();
            configuration.CreateMap<CreateOrEditBlogPostDto, BlogPost>().ReverseMap();
            configuration.CreateMap<BlogPostDto, BlogPost>().ReverseMap();
            configuration.CreateMap<CreateOrEditBankAccountInfoDto, BankAccountInfo>().ReverseMap();
            configuration.CreateMap<BankAccountInfoDto, BankAccountInfo>().ReverseMap();
            configuration.CreateMap<CreateOrEditBankBranchDto, BankBranch>().ReverseMap();
            configuration.CreateMap<BankBranchDto, BankBranch>().ReverseMap();
            configuration.CreateMap<CreateOrEditBankDto, Bank>().ReverseMap();
            configuration.CreateMap<BankDto, Bank>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductImageDto, ProductImage>().ReverseMap();
            configuration.CreateMap<ProductImageDto, ProductImage>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductDto, Product>().ReverseMap();
            configuration.CreateMap<ProductDto, Product>().ReverseMap();
            configuration.CreateMap<CreateOrEditCategoryDto, Category>().ReverseMap();
            configuration.CreateMap<CategoryDto, Category>().ReverseMap();
            configuration.CreateMap<CreateOrEditPlaceDto, Place>().ReverseMap();
            configuration.CreateMap<PlaceDto, Place>().ReverseMap();
            configuration.CreateMap<CreateOrEditPlaceCategoryDto, PlaceCategory>().ReverseMap();
            configuration.CreateMap<PlaceCategoryDto, PlaceCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditDetinationDto, Detination>().ReverseMap();
            configuration.CreateMap<DetinationDto, Detination>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<HelpCategory, TepayLink.Sdisco.Help.Dto.HelpCategoryDto>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}
