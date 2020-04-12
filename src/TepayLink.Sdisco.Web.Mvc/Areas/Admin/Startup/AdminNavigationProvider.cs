using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using TepayLink.Sdisco.Authorization;

namespace TepayLink.Sdisco.Web.Areas.Admin.Startup
{
    public class AdminNavigationProvider : NavigationProvider
    {
        public const string MenuName = "App";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                .AddItem(new MenuItemDefinition(
                        AdminPageNames.Host.Dashboard,
                        L("Dashboard"),
                        url: "Admin/HostDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Dashboard),
                        order: 1
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.TransPortdetails,
                        L("TransPortdetails"),
                        url: "Admin/TransPortdetails",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_TransPortdetails
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Partners,
                        L("Partners"),
                        url: "Admin/Partners",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Partners
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Bookings,
                        L("Bookings"),
                        url: "Admin/Bookings",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Bookings
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Countries,
                        L("Countries"),
                        url: "Admin/Countries",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Countries
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.BlogProductRelateds,
                        L("BlogProductRelateds"),
                        url: "Admin/BlogProductRelateds",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_BlogProductRelateds
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.BankBranchs,
                        L("BankBranchs"),
                        url: "Admin/BankBranchs",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_BankBranchs
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.ProductImages,
                        L("ProductImages"),
                        url: "Admin/ProductImages",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_ProductImages
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Products,
                        L("Products"),
                        url: "Admin/Products",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Products
                    )
                ).AddItem(new MenuItemDefinition(
                    AdminPageNames.Host.Tenants,
                    L("Tenants"),
                    url: "Admin/Tenants",
                    icon: "flaticon-list-3",
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenants),
                    order: 2
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Host.Editions,
                        L("Editions"),
                        url: "Admin/Editions",
                        icon: "flaticon-app",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Editions),
                        order: 3
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Tenant.Dashboard,
                        L("Dashboard"),
                        url: "Admin/TenantDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenant_Dashboard),
                        order: 1
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Administration,
                        L("Administration"),
                        icon: "flaticon-interface-8",
                        order: 4
                    ).AddItem(new MenuItemDefinition(
                            AdminPageNames.Common.OrganizationUnits,
                            L("OrganizationUnits"),
                            url: "Admin/OrganizationUnits",
                            icon: "flaticon-map",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_OrganizationUnits),
                            order: 1
                        )
                    ).AddItem(new MenuItemDefinition(
                            AdminPageNames.Common.Roles,
                            L("Roles"),
                            url: "Admin/Roles",
                            icon: "flaticon-suitcase",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Roles),
                            order: 2
                        )
                    ).AddItem(new MenuItemDefinition(
                            AdminPageNames.Common.Users,
                            L("Users"),
                            url: "Admin/Users",
                            icon: "flaticon-users",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Users),
                            order: 3
                        )
                    ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Chatconversations,
                        L("Chatconversations"),
                        url: "Admin/Chatconversations",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_Chatconversations
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.UserDefaultCashoutMethodTypes,
                        L("UserDefaultCashoutMethodTypes"),
                        url: "Admin/UserDefaultCashoutMethodTypes",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_UserDefaultCashoutMethodTypes
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.CashoutMethodTypes,
                        L("CashoutMethodTypes"),
                        url: "Admin/CashoutMethodTypes",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_CashoutMethodTypes
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Coupons,
                        L("Coupons"),
                        url: "Admin/Coupons",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_Coupons
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.PartnerShips,
                        L("PartnerShips"),
                        url: "Admin/PartnerShips",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_PartnerShips
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Orders,
                        L("Orders"),
                        url: "Admin/Orders",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_Orders
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.ClientSettings,
                        L("ClientSettings"),
                        url: "Admin/ClientSettings",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_ClientSettings
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.BookingClaims,
                        L("BookingClaims"),
                        url: "Admin/BookingClaims",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_BookingClaims
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.ClaimReasons,
                        L("ClaimReasons"),
                        url: "Admin/ClaimReasons",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_ClaimReasons
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.BookingDetails,
                        L("BookingDetails"),
                        url: "Admin/BookingDetails",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_BookingDetails
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Currencies,
                        L("Currencies"),
                        url: "Admin/Currencies",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_Currencies
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.UserSubcribers,
                        L("UserSubcribers"),
                        url: "Admin/UserSubcribers",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_UserSubcribers
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.ProductSchedules,
                        L("ProductSchedules"),
                        url: "Admin/ProductSchedules",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_ProductSchedules
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.ProductDetailCombos,
                        L("ProductDetailCombos"),
                        url: "Admin/ProductDetailCombos",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_ProductDetailCombos
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.ProductDetails,
                        L("ProductDetails"),
                        url: "Admin/ProductDetails",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_ProductDetails
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.UserReviewDetails,
                        L("UserReviewDetails"),
                        url: "Admin/UserReviewDetails",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_UserReviewDetails
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.UserReviews,
                        L("UserReviews"),
                        url: "Admin/UserReviews",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_UserReviews
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.ProductReviewDetails,
                        L("ProductReviewDetails"),
                        url: "Admin/ProductReviewDetails",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_ProductReviewDetails
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.ProductReviews,
                        L("ProductReviews"),
                        url: "Admin/ProductReviews",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_ProductReviews
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.ProductUtilities,
                        L("ProductUtilities"),
                        url: "Admin/ProductUtilities",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_ProductUtilities
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Utilities,
                        L("Utilities"),
                        url: "Admin/Utilities",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_Utilities
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.SaveItems,
                        L("SaveItems"),
                        url: "Admin/SaveItems",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_SaveItems
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.HelpContents,
                        L("HelpContents"),
                        url: "Admin/HelpContents",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_HelpContents
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.HelpCategories,
                        L("HelpCategories"),
                        url: "Admin/HelpCategories",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_HelpCategories
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.BlogComments,
                        L("BlogComments"),
                        url: "Admin/BlogComments",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_BlogComments
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.BlogPosts,
                        L("BlogPosts"),
                        url: "Admin/BlogPosts",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_BlogPosts
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.BankAccountInfos,
                        L("BankAccountInfos"),
                        url: "Admin/BankAccountInfos",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_BankAccountInfos
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Banks,
                        L("Banks"),
                        url: "Admin/Banks",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_Banks
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Products,
                        L("Products"),
                        url: "Admin/Products",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_Products
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Categories,
                        L("Categories"),
                        url: "Admin/Categories",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_Categories
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Places,
                        L("Places"),
                        url: "Admin/Places",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_Places
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.PlaceCategories,
                        L("PlaceCategories"),
                        url: "Admin/PlaceCategories",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_PlaceCategories
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.Detinations,
                        L("Detinations"),
                        url: "Admin/Detinations",
                        icon: "flaticon-more",
                        requiredPermissionName: AppPermissions.Pages_Administration_Detinations
                    )
                ).AddItem(new MenuItemDefinition(
                            AdminPageNames.Common.Languages,
                            L("Languages"),
                            url: "Admin/Languages",
                            icon: "flaticon-tabs",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Languages),
                            order: 4
                        )
                    ).AddItem(new MenuItemDefinition(
                            AdminPageNames.Common.AuditLogs,
                            L("AuditLogs"),
                            url: "Admin/AuditLogs",
                            icon: "flaticon-folder-1",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_AuditLogs),
                            order: 5
                        )
                    ).AddItem(new MenuItemDefinition(
                            AdminPageNames.Host.Maintenance,
                            L("Maintenance"),
                            url: "Admin/Maintenance",
                            icon: "flaticon-lock",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Maintenance),
                            order: 6
                        )
                    ).AddItem(new MenuItemDefinition(
                            AdminPageNames.Tenant.SubscriptionManagement,
                            L("Subscription"),
                            url: "Admin/SubscriptionManagement",
                            icon: "flaticon-refresh",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement),
                            order: 6
                        )
                    ).AddItem(new MenuItemDefinition(
                            AdminPageNames.Common.UiCustomization,
                            L("VisualSettings"),
                            url: "Admin/UiCustomization",
                            icon: "flaticon-medical",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_UiCustomization),
                            order: 7
                        )
                    ).AddItem(new MenuItemDefinition(
                            AdminPageNames.Host.Settings,
                            L("Settings"),
                            url: "Admin/HostSettings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Settings),
                            order: 8
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AdminPageNames.Tenant.Settings,
                            L("Settings"),
                            url: "Admin/Settings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Tenant_Settings),
                            order: 8
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AdminPageNames.Common.WebhookSubscriptions,
                            L("WebhookSubscriptions"),
                            url: "Admin/WebhookSubscription",
                            icon: "flaticon2-world",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_WebhookSubscription),
                            order: 9
                        )
                    )
                ).AddItem(new MenuItemDefinition(
                        AdminPageNames.Common.DemoUiComponents,
                        L("DemoUiComponents"),
                        url: "Admin/DemoUiComponents",
                        icon: "flaticon-shapes",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_DemoUiComponents),
                        order: 5
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SdiscoConsts.LocalizationSourceName);
        }
    }
}