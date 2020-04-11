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
