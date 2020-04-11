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