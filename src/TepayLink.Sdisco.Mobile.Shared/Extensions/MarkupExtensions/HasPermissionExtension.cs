using System;
using TepayLink.Sdisco.Core;
using TepayLink.Sdisco.Core.Dependency;
using TepayLink.Sdisco.Services.Permission;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TepayLink.Sdisco.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class HasPermissionExtension : IMarkupExtension
    {
        public string Text { get; set; }
        
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || Text == null)
            {
                return false;
            }

            var permissionService = DependencyResolver.Resolve<IPermissionService>();
            return permissionService.HasPermission(Text);
        }
    }
}