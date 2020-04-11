using Microsoft.Extensions.Configuration;

namespace TepayLink.Sdisco.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
