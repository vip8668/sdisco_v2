using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace TepayLink.Sdisco.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var chatMessageV2s = pages.CreateChildPermission(AppPermissions.Pages_ChatMessageV2s, L("ChatMessageV2s"));
            chatMessageV2s.CreateChildPermission(AppPermissions.Pages_ChatMessageV2s_Create, L("CreateNewChatMessageV2"));
            chatMessageV2s.CreateChildPermission(AppPermissions.Pages_ChatMessageV2s_Edit, L("EditChatMessageV2"));
            chatMessageV2s.CreateChildPermission(AppPermissions.Pages_ChatMessageV2s_Delete, L("DeleteChatMessageV2"));



        



            var transPortdetails = pages.CreateChildPermission(AppPermissions.Pages_TransPortdetails, L("TransPortdetails"));
            transPortdetails.CreateChildPermission(AppPermissions.Pages_TransPortdetails_Create, L("CreateNewTransPortdetail"));
            transPortdetails.CreateChildPermission(AppPermissions.Pages_TransPortdetails_Edit, L("EditTransPortdetail"));
            transPortdetails.CreateChildPermission(AppPermissions.Pages_TransPortdetails_Delete, L("DeleteTransPortdetail"));



            var searchHistories = pages.CreateChildPermission(AppPermissions.Pages_SearchHistories, L("SearchHistories"));
            searchHistories.CreateChildPermission(AppPermissions.Pages_SearchHistories_Create, L("CreateNewSearchHistory"));
            searchHistories.CreateChildPermission(AppPermissions.Pages_SearchHistories_Edit, L("EditSearchHistory"));
            searchHistories.CreateChildPermission(AppPermissions.Pages_SearchHistories_Delete, L("DeleteSearchHistory"));



            var wallets = pages.CreateChildPermission(AppPermissions.Pages_Wallets, L("Wallets"));
            wallets.CreateChildPermission(AppPermissions.Pages_Wallets_Create, L("CreateNewWallet"));
            wallets.CreateChildPermission(AppPermissions.Pages_Wallets_Edit, L("EditWallet"));
            wallets.CreateChildPermission(AppPermissions.Pages_Wallets_Delete, L("DeleteWallet"));



            var partners = pages.CreateChildPermission(AppPermissions.Pages_Partners, L("Partners"));
            partners.CreateChildPermission(AppPermissions.Pages_Partners_Create, L("CreateNewPartner"));
            partners.CreateChildPermission(AppPermissions.Pages_Partners_Edit, L("EditPartner"));
            partners.CreateChildPermission(AppPermissions.Pages_Partners_Delete, L("DeletePartner"));



            var bookings = pages.CreateChildPermission(AppPermissions.Pages_Bookings, L("Bookings"));
            bookings.CreateChildPermission(AppPermissions.Pages_Bookings_Create, L("CreateNewBooking"));
            bookings.CreateChildPermission(AppPermissions.Pages_Bookings_Edit, L("EditBooking"));
            bookings.CreateChildPermission(AppPermissions.Pages_Bookings_Delete, L("DeleteBooking"));



            var countries = pages.CreateChildPermission(AppPermissions.Pages_Countries, L("Countries"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Create, L("CreateNewCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Edit, L("EditCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Delete, L("DeleteCountry"));



            var blogProductRelateds = pages.CreateChildPermission(AppPermissions.Pages_BlogProductRelateds, L("BlogProductRelateds"));
            blogProductRelateds.CreateChildPermission(AppPermissions.Pages_BlogProductRelateds_Create, L("CreateNewBlogProductRelated"));
            blogProductRelateds.CreateChildPermission(AppPermissions.Pages_BlogProductRelateds_Edit, L("EditBlogProductRelated"));
            blogProductRelateds.CreateChildPermission(AppPermissions.Pages_BlogProductRelateds_Delete, L("DeleteBlogProductRelated"));



            var bankBranchs = pages.CreateChildPermission(AppPermissions.Pages_BankBranchs, L("BankBranchs"));
            bankBranchs.CreateChildPermission(AppPermissions.Pages_BankBranchs_Create, L("CreateNewBankBranch"));
            bankBranchs.CreateChildPermission(AppPermissions.Pages_BankBranchs_Edit, L("EditBankBranch"));
            bankBranchs.CreateChildPermission(AppPermissions.Pages_BankBranchs_Delete, L("DeleteBankBranch"));



            var productImages = pages.CreateChildPermission(AppPermissions.Pages_ProductImages, L("ProductImages"));
            productImages.CreateChildPermission(AppPermissions.Pages_ProductImages_Create, L("CreateNewProductImage"));
            productImages.CreateChildPermission(AppPermissions.Pages_ProductImages_Edit, L("EditProductImage"));
            productImages.CreateChildPermission(AppPermissions.Pages_ProductImages_Delete, L("DeleteProductImage"));



           
            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var revenueByMonths = administration.CreateChildPermission(AppPermissions.Pages_Administration_RevenueByMonths, L("RevenueByMonths"));
            revenueByMonths.CreateChildPermission(AppPermissions.Pages_Administration_RevenueByMonths_Create, L("CreateNewRevenueByMonth"));
            revenueByMonths.CreateChildPermission(AppPermissions.Pages_Administration_RevenueByMonths_Edit, L("EditRevenueByMonth"));
            revenueByMonths.CreateChildPermission(AppPermissions.Pages_Administration_RevenueByMonths_Delete, L("DeleteRevenueByMonth"));



            var bookingRefunds = administration.CreateChildPermission(AppPermissions.Pages_Administration_BookingRefunds, L("BookingRefunds"));
            bookingRefunds.CreateChildPermission(AppPermissions.Pages_Administration_BookingRefunds_Create, L("CreateNewBookingRefund"));
            bookingRefunds.CreateChildPermission(AppPermissions.Pages_Administration_BookingRefunds_Edit, L("EditBookingRefund"));
            bookingRefunds.CreateChildPermission(AppPermissions.Pages_Administration_BookingRefunds_Delete, L("DeleteBookingRefund"));



            var refundReasons = administration.CreateChildPermission(AppPermissions.Pages_Administration_RefundReasons, L("RefundReasons"));
            refundReasons.CreateChildPermission(AppPermissions.Pages_Administration_RefundReasons_Create, L("CreateNewRefundReason"));
            refundReasons.CreateChildPermission(AppPermissions.Pages_Administration_RefundReasons_Edit, L("EditRefundReason"));
            refundReasons.CreateChildPermission(AppPermissions.Pages_Administration_RefundReasons_Delete, L("DeleteRefundReason"));



            var transactions = administration.CreateChildPermission(AppPermissions.Pages_Administration_Transactions, L("Transactions"));
            transactions.CreateChildPermission(AppPermissions.Pages_Administration_Transactions_Create, L("CreateNewTransaction"));
            transactions.CreateChildPermission(AppPermissions.Pages_Administration_Transactions_Edit, L("EditTransaction"));
            transactions.CreateChildPermission(AppPermissions.Pages_Administration_Transactions_Delete, L("DeleteTransaction"));



            var withDrawRequests = administration.CreateChildPermission(AppPermissions.Pages_Administration_WithDrawRequests, L("WithDrawRequests"));
            withDrawRequests.CreateChildPermission(AppPermissions.Pages_Administration_WithDrawRequests_Create, L("CreateNewWithDrawRequest"));
            withDrawRequests.CreateChildPermission(AppPermissions.Pages_Administration_WithDrawRequests_Edit, L("EditWithDrawRequest"));
            withDrawRequests.CreateChildPermission(AppPermissions.Pages_Administration_WithDrawRequests_Delete, L("DeleteWithDrawRequest"));



            var shareTransactions = administration.CreateChildPermission(AppPermissions.Pages_Administration_ShareTransactions, L("ShareTransactions"));
            shareTransactions.CreateChildPermission(AppPermissions.Pages_Administration_ShareTransactions_Create, L("CreateNewShareTransaction"));
            shareTransactions.CreateChildPermission(AppPermissions.Pages_Administration_ShareTransactions_Edit, L("EditShareTransaction"));
            shareTransactions.CreateChildPermission(AppPermissions.Pages_Administration_ShareTransactions_Delete, L("DeleteShareTransaction"));



            var partnerRevenues = administration.CreateChildPermission(AppPermissions.Pages_Administration_PartnerRevenues, L("PartnerRevenues"));
            partnerRevenues.CreateChildPermission(AppPermissions.Pages_Administration_PartnerRevenues_Create, L("CreateNewPartnerRevenue"));
            partnerRevenues.CreateChildPermission(AppPermissions.Pages_Administration_PartnerRevenues_Edit, L("EditPartnerRevenue"));
            partnerRevenues.CreateChildPermission(AppPermissions.Pages_Administration_PartnerRevenues_Delete, L("DeletePartnerRevenue"));



            var shortLinks = administration.CreateChildPermission(AppPermissions.Pages_Administration_ShortLinks, L("ShortLinks"));
            shortLinks.CreateChildPermission(AppPermissions.Pages_Administration_ShortLinks_Create, L("CreateNewShortLink"));
            shortLinks.CreateChildPermission(AppPermissions.Pages_Administration_ShortLinks_Edit, L("EditShortLink"));
            shortLinks.CreateChildPermission(AppPermissions.Pages_Administration_ShortLinks_Delete, L("DeleteShortLink"));



            var nearbyPlaces = administration.CreateChildPermission(AppPermissions.Pages_Administration_NearbyPlaces, L("NearbyPlaces"));
            nearbyPlaces.CreateChildPermission(AppPermissions.Pages_Administration_NearbyPlaces_Create, L("CreateNewNearbyPlace"));
            nearbyPlaces.CreateChildPermission(AppPermissions.Pages_Administration_NearbyPlaces_Edit, L("EditNearbyPlace"));
            nearbyPlaces.CreateChildPermission(AppPermissions.Pages_Administration_NearbyPlaces_Delete, L("DeleteNearbyPlace"));



            var suggestedProducts = administration.CreateChildPermission(AppPermissions.Pages_Administration_SuggestedProducts, L("SuggestedProducts"));
            suggestedProducts.CreateChildPermission(AppPermissions.Pages_Administration_SuggestedProducts_Create, L("CreateNewSuggestedProduct"));
            suggestedProducts.CreateChildPermission(AppPermissions.Pages_Administration_SuggestedProducts_Edit, L("EditSuggestedProduct"));
            suggestedProducts.CreateChildPermission(AppPermissions.Pages_Administration_SuggestedProducts_Delete, L("DeleteSuggestedProduct"));



            var relatedProducts = administration.CreateChildPermission(AppPermissions.Pages_Administration_RelatedProducts, L("RelatedProducts"));
            relatedProducts.CreateChildPermission(AppPermissions.Pages_Administration_RelatedProducts_Create, L("CreateNewRelatedProduct"));
            relatedProducts.CreateChildPermission(AppPermissions.Pages_Administration_RelatedProducts_Edit, L("EditRelatedProduct"));
            relatedProducts.CreateChildPermission(AppPermissions.Pages_Administration_RelatedProducts_Delete, L("DeleteRelatedProduct"));



            var similarProducts = administration.CreateChildPermission(AppPermissions.Pages_Administration_SimilarProducts, L("SimilarProducts"));
            similarProducts.CreateChildPermission(AppPermissions.Pages_Administration_SimilarProducts_Create, L("CreateNewSimilarProduct"));
            similarProducts.CreateChildPermission(AppPermissions.Pages_Administration_SimilarProducts_Edit, L("EditSimilarProduct"));
            similarProducts.CreateChildPermission(AppPermissions.Pages_Administration_SimilarProducts_Delete, L("DeleteSimilarProduct"));



            var chatconversations = administration.CreateChildPermission(AppPermissions.Pages_Administration_Chatconversations, L("Chatconversations"));
            chatconversations.CreateChildPermission(AppPermissions.Pages_Administration_Chatconversations_Create, L("CreateNewChatconversation"));
            chatconversations.CreateChildPermission(AppPermissions.Pages_Administration_Chatconversations_Edit, L("EditChatconversation"));
            chatconversations.CreateChildPermission(AppPermissions.Pages_Administration_Chatconversations_Delete, L("DeleteChatconversation"));



            var userDefaultCashoutMethodTypes = administration.CreateChildPermission(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes, L("UserDefaultCashoutMethodTypes"));
            userDefaultCashoutMethodTypes.CreateChildPermission(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes_Create, L("CreateNewUserDefaultCashoutMethodType"));
            userDefaultCashoutMethodTypes.CreateChildPermission(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes_Edit, L("EditUserDefaultCashoutMethodType"));
            userDefaultCashoutMethodTypes.CreateChildPermission(AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes_Delete, L("DeleteUserDefaultCashoutMethodType"));



            var cashoutMethodTypes = administration.CreateChildPermission(AppPermissions.Pages_Administration_CashoutMethodTypes, L("CashoutMethodTypes"));
            cashoutMethodTypes.CreateChildPermission(AppPermissions.Pages_Administration_CashoutMethodTypes_Create, L("CreateNewCashoutMethodType"));
            cashoutMethodTypes.CreateChildPermission(AppPermissions.Pages_Administration_CashoutMethodTypes_Edit, L("EditCashoutMethodType"));
            cashoutMethodTypes.CreateChildPermission(AppPermissions.Pages_Administration_CashoutMethodTypes_Delete, L("DeleteCashoutMethodType"));



            var coupons = administration.CreateChildPermission(AppPermissions.Pages_Administration_Coupons, L("Coupons"));
            coupons.CreateChildPermission(AppPermissions.Pages_Administration_Coupons_Create, L("CreateNewCoupon"));
            coupons.CreateChildPermission(AppPermissions.Pages_Administration_Coupons_Edit, L("EditCoupon"));
            coupons.CreateChildPermission(AppPermissions.Pages_Administration_Coupons_Delete, L("DeleteCoupon"));



            var partnerShips = administration.CreateChildPermission(AppPermissions.Pages_Administration_PartnerShips, L("PartnerShips"));
            partnerShips.CreateChildPermission(AppPermissions.Pages_Administration_PartnerShips_Create, L("CreateNewPartnerShip"));
            partnerShips.CreateChildPermission(AppPermissions.Pages_Administration_PartnerShips_Edit, L("EditPartnerShip"));
            partnerShips.CreateChildPermission(AppPermissions.Pages_Administration_PartnerShips_Delete, L("DeletePartnerShip"));



            var orders = administration.CreateChildPermission(AppPermissions.Pages_Administration_Orders, L("Orders"));
            orders.CreateChildPermission(AppPermissions.Pages_Administration_Orders_Create, L("CreateNewOrder"));
            orders.CreateChildPermission(AppPermissions.Pages_Administration_Orders_Edit, L("EditOrder"));
            orders.CreateChildPermission(AppPermissions.Pages_Administration_Orders_Delete, L("DeleteOrder"));



            var clientSettings = administration.CreateChildPermission(AppPermissions.Pages_Administration_ClientSettings, L("ClientSettings"));
            clientSettings.CreateChildPermission(AppPermissions.Pages_Administration_ClientSettings_Create, L("CreateNewClientSetting"));
            clientSettings.CreateChildPermission(AppPermissions.Pages_Administration_ClientSettings_Edit, L("EditClientSetting"));
            clientSettings.CreateChildPermission(AppPermissions.Pages_Administration_ClientSettings_Delete, L("DeleteClientSetting"));



            var bookingClaims = administration.CreateChildPermission(AppPermissions.Pages_Administration_BookingClaims, L("BookingClaims"));
            bookingClaims.CreateChildPermission(AppPermissions.Pages_Administration_BookingClaims_Create, L("CreateNewBookingClaim"));
            bookingClaims.CreateChildPermission(AppPermissions.Pages_Administration_BookingClaims_Edit, L("EditBookingClaim"));
            bookingClaims.CreateChildPermission(AppPermissions.Pages_Administration_BookingClaims_Delete, L("DeleteBookingClaim"));



            var claimReasons = administration.CreateChildPermission(AppPermissions.Pages_Administration_ClaimReasons, L("ClaimReasons"));
            claimReasons.CreateChildPermission(AppPermissions.Pages_Administration_ClaimReasons_Create, L("CreateNewClaimReason"));
            claimReasons.CreateChildPermission(AppPermissions.Pages_Administration_ClaimReasons_Edit, L("EditClaimReason"));
            claimReasons.CreateChildPermission(AppPermissions.Pages_Administration_ClaimReasons_Delete, L("DeleteClaimReason"));



            var bookingDetails = administration.CreateChildPermission(AppPermissions.Pages_Administration_BookingDetails, L("BookingDetails"));
            bookingDetails.CreateChildPermission(AppPermissions.Pages_Administration_BookingDetails_Create, L("CreateNewBookingDetail"));
            bookingDetails.CreateChildPermission(AppPermissions.Pages_Administration_BookingDetails_Edit, L("EditBookingDetail"));
            bookingDetails.CreateChildPermission(AppPermissions.Pages_Administration_BookingDetails_Delete, L("DeleteBookingDetail"));



            var currencies = administration.CreateChildPermission(AppPermissions.Pages_Administration_Currencies, L("Currencies"));
            currencies.CreateChildPermission(AppPermissions.Pages_Administration_Currencies_Create, L("CreateNewCurrency"));
            currencies.CreateChildPermission(AppPermissions.Pages_Administration_Currencies_Edit, L("EditCurrency"));
            currencies.CreateChildPermission(AppPermissions.Pages_Administration_Currencies_Delete, L("DeleteCurrency"));



            var userSubcribers = administration.CreateChildPermission(AppPermissions.Pages_Administration_UserSubcribers, L("UserSubcribers"));
            userSubcribers.CreateChildPermission(AppPermissions.Pages_Administration_UserSubcribers_Create, L("CreateNewUserSubcriber"));
            userSubcribers.CreateChildPermission(AppPermissions.Pages_Administration_UserSubcribers_Edit, L("EditUserSubcriber"));
            userSubcribers.CreateChildPermission(AppPermissions.Pages_Administration_UserSubcribers_Delete, L("DeleteUserSubcriber"));



            var productSchedules = administration.CreateChildPermission(AppPermissions.Pages_Administration_ProductSchedules, L("ProductSchedules"));
            productSchedules.CreateChildPermission(AppPermissions.Pages_Administration_ProductSchedules_Create, L("CreateNewProductSchedule"));
            productSchedules.CreateChildPermission(AppPermissions.Pages_Administration_ProductSchedules_Edit, L("EditProductSchedule"));
            productSchedules.CreateChildPermission(AppPermissions.Pages_Administration_ProductSchedules_Delete, L("DeleteProductSchedule"));



            var productDetailCombos = administration.CreateChildPermission(AppPermissions.Pages_Administration_ProductDetailCombos, L("ProductDetailCombos"));
            productDetailCombos.CreateChildPermission(AppPermissions.Pages_Administration_ProductDetailCombos_Create, L("CreateNewProductDetailCombo"));
            productDetailCombos.CreateChildPermission(AppPermissions.Pages_Administration_ProductDetailCombos_Edit, L("EditProductDetailCombo"));
            productDetailCombos.CreateChildPermission(AppPermissions.Pages_Administration_ProductDetailCombos_Delete, L("DeleteProductDetailCombo"));



            var productDetails = administration.CreateChildPermission(AppPermissions.Pages_Administration_ProductDetails, L("ProductDetails"));
            productDetails.CreateChildPermission(AppPermissions.Pages_Administration_ProductDetails_Create, L("CreateNewProductDetail"));
            productDetails.CreateChildPermission(AppPermissions.Pages_Administration_ProductDetails_Edit, L("EditProductDetail"));
            productDetails.CreateChildPermission(AppPermissions.Pages_Administration_ProductDetails_Delete, L("DeleteProductDetail"));



            var userReviewDetails = administration.CreateChildPermission(AppPermissions.Pages_Administration_UserReviewDetails, L("UserReviewDetails"));
            userReviewDetails.CreateChildPermission(AppPermissions.Pages_Administration_UserReviewDetails_Create, L("CreateNewUserReviewDetail"));
            userReviewDetails.CreateChildPermission(AppPermissions.Pages_Administration_UserReviewDetails_Edit, L("EditUserReviewDetail"));
            userReviewDetails.CreateChildPermission(AppPermissions.Pages_Administration_UserReviewDetails_Delete, L("DeleteUserReviewDetail"));



            var userReviews = administration.CreateChildPermission(AppPermissions.Pages_Administration_UserReviews, L("UserReviews"));
            userReviews.CreateChildPermission(AppPermissions.Pages_Administration_UserReviews_Create, L("CreateNewUserReview"));
            userReviews.CreateChildPermission(AppPermissions.Pages_Administration_UserReviews_Edit, L("EditUserReview"));
            userReviews.CreateChildPermission(AppPermissions.Pages_Administration_UserReviews_Delete, L("DeleteUserReview"));



            var productReviewDetails = administration.CreateChildPermission(AppPermissions.Pages_Administration_ProductReviewDetails, L("ProductReviewDetails"));
            productReviewDetails.CreateChildPermission(AppPermissions.Pages_Administration_ProductReviewDetails_Create, L("CreateNewProductReviewDetail"));
            productReviewDetails.CreateChildPermission(AppPermissions.Pages_Administration_ProductReviewDetails_Edit, L("EditProductReviewDetail"));
            productReviewDetails.CreateChildPermission(AppPermissions.Pages_Administration_ProductReviewDetails_Delete, L("DeleteProductReviewDetail"));



            var productReviews = administration.CreateChildPermission(AppPermissions.Pages_Administration_ProductReviews, L("ProductReviews"));
            productReviews.CreateChildPermission(AppPermissions.Pages_Administration_ProductReviews_Create, L("CreateNewProductReview"));
            productReviews.CreateChildPermission(AppPermissions.Pages_Administration_ProductReviews_Edit, L("EditProductReview"));
            productReviews.CreateChildPermission(AppPermissions.Pages_Administration_ProductReviews_Delete, L("DeleteProductReview"));



            var productUtilities = administration.CreateChildPermission(AppPermissions.Pages_Administration_ProductUtilities, L("ProductUtilities"));
            productUtilities.CreateChildPermission(AppPermissions.Pages_Administration_ProductUtilities_Create, L("CreateNewProductUtility"));
            productUtilities.CreateChildPermission(AppPermissions.Pages_Administration_ProductUtilities_Edit, L("EditProductUtility"));
            productUtilities.CreateChildPermission(AppPermissions.Pages_Administration_ProductUtilities_Delete, L("DeleteProductUtility"));



            var utilities = administration.CreateChildPermission(AppPermissions.Pages_Administration_Utilities, L("Utilities"));
            utilities.CreateChildPermission(AppPermissions.Pages_Administration_Utilities_Create, L("CreateNewUtility"));
            utilities.CreateChildPermission(AppPermissions.Pages_Administration_Utilities_Edit, L("EditUtility"));
            utilities.CreateChildPermission(AppPermissions.Pages_Administration_Utilities_Delete, L("DeleteUtility"));



            var saveItems = administration.CreateChildPermission(AppPermissions.Pages_Administration_SaveItems, L("SaveItems"));
            saveItems.CreateChildPermission(AppPermissions.Pages_Administration_SaveItems_Create, L("CreateNewSaveItem"));
            saveItems.CreateChildPermission(AppPermissions.Pages_Administration_SaveItems_Edit, L("EditSaveItem"));
            saveItems.CreateChildPermission(AppPermissions.Pages_Administration_SaveItems_Delete, L("DeleteSaveItem"));



            var helpContents = administration.CreateChildPermission(AppPermissions.Pages_Administration_HelpContents, L("HelpContents"));
            helpContents.CreateChildPermission(AppPermissions.Pages_Administration_HelpContents_Create, L("CreateNewHelpContent"));
            helpContents.CreateChildPermission(AppPermissions.Pages_Administration_HelpContents_Edit, L("EditHelpContent"));
            helpContents.CreateChildPermission(AppPermissions.Pages_Administration_HelpContents_Delete, L("DeleteHelpContent"));



            var helpCategories = administration.CreateChildPermission(AppPermissions.Pages_Administration_HelpCategories, L("HelpCategories"));
            helpCategories.CreateChildPermission(AppPermissions.Pages_Administration_HelpCategories_Create, L("CreateNewHelpCategory"));
            helpCategories.CreateChildPermission(AppPermissions.Pages_Administration_HelpCategories_Edit, L("EditHelpCategory"));
            helpCategories.CreateChildPermission(AppPermissions.Pages_Administration_HelpCategories_Delete, L("DeleteHelpCategory"));



            var blogComments = administration.CreateChildPermission(AppPermissions.Pages_Administration_BlogComments, L("BlogComments"));
            blogComments.CreateChildPermission(AppPermissions.Pages_Administration_BlogComments_Create, L("CreateNewBlogComment"));
            blogComments.CreateChildPermission(AppPermissions.Pages_Administration_BlogComments_Edit, L("EditBlogComment"));
            blogComments.CreateChildPermission(AppPermissions.Pages_Administration_BlogComments_Delete, L("DeleteBlogComment"));



            var blogPosts = administration.CreateChildPermission(AppPermissions.Pages_Administration_BlogPosts, L("BlogPosts"));
            blogPosts.CreateChildPermission(AppPermissions.Pages_Administration_BlogPosts_Create, L("CreateNewBlogPost"));
            blogPosts.CreateChildPermission(AppPermissions.Pages_Administration_BlogPosts_Edit, L("EditBlogPost"));
            blogPosts.CreateChildPermission(AppPermissions.Pages_Administration_BlogPosts_Delete, L("DeleteBlogPost"));



            var bankAccountInfos = administration.CreateChildPermission(AppPermissions.Pages_Administration_BankAccountInfos, L("BankAccountInfos"));
            bankAccountInfos.CreateChildPermission(AppPermissions.Pages_Administration_BankAccountInfos_Create, L("CreateNewBankAccountInfo"));
            bankAccountInfos.CreateChildPermission(AppPermissions.Pages_Administration_BankAccountInfos_Edit, L("EditBankAccountInfo"));
            bankAccountInfos.CreateChildPermission(AppPermissions.Pages_Administration_BankAccountInfos_Delete, L("DeleteBankAccountInfo"));



            var banks = administration.CreateChildPermission(AppPermissions.Pages_Administration_Banks, L("Banks"));
            banks.CreateChildPermission(AppPermissions.Pages_Administration_Banks_Create, L("CreateNewBank"));
            banks.CreateChildPermission(AppPermissions.Pages_Administration_Banks_Edit, L("EditBank"));
            banks.CreateChildPermission(AppPermissions.Pages_Administration_Banks_Delete, L("DeleteBank"));



            var products = administration.CreateChildPermission(AppPermissions.Pages_Administration_Products, L("Products"));
            products.CreateChildPermission(AppPermissions.Pages_Administration_Products_Create, L("CreateNewProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Administration_Products_Edit, L("EditProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Administration_Products_Delete, L("DeleteProduct"));



            var categories = administration.CreateChildPermission(AppPermissions.Pages_Administration_Categories, L("Categories"));
            categories.CreateChildPermission(AppPermissions.Pages_Administration_Categories_Create, L("CreateNewCategory"));
            categories.CreateChildPermission(AppPermissions.Pages_Administration_Categories_Edit, L("EditCategory"));
            categories.CreateChildPermission(AppPermissions.Pages_Administration_Categories_Delete, L("DeleteCategory"));



            var places = administration.CreateChildPermission(AppPermissions.Pages_Administration_Places, L("Places"));
            places.CreateChildPermission(AppPermissions.Pages_Administration_Places_Create, L("CreateNewPlace"));
            places.CreateChildPermission(AppPermissions.Pages_Administration_Places_Edit, L("EditPlace"));
            places.CreateChildPermission(AppPermissions.Pages_Administration_Places_Delete, L("DeletePlace"));



            var placeCategories = administration.CreateChildPermission(AppPermissions.Pages_Administration_PlaceCategories, L("PlaceCategories"));
            placeCategories.CreateChildPermission(AppPermissions.Pages_Administration_PlaceCategories_Create, L("CreateNewPlaceCategory"));
            placeCategories.CreateChildPermission(AppPermissions.Pages_Administration_PlaceCategories_Edit, L("EditPlaceCategory"));
            placeCategories.CreateChildPermission(AppPermissions.Pages_Administration_PlaceCategories_Delete, L("DeletePlaceCategory"));



            var detinations = administration.CreateChildPermission(AppPermissions.Pages_Administration_Detinations, L("Detinations"));
            detinations.CreateChildPermission(AppPermissions.Pages_Administration_Detinations_Create, L("CreateNewDetination"));
            detinations.CreateChildPermission(AppPermissions.Pages_Administration_Detinations_Edit, L("EditDetination"));
            detinations.CreateChildPermission(AppPermissions.Pages_Administration_Detinations_Delete, L("DeleteDetination"));



            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SdiscoConsts.LocalizationSourceName);
        }
    }
}
