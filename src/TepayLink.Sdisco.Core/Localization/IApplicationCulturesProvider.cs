using System.Globalization;

namespace TepayLink.Sdisco.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}