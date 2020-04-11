using System.Threading.Tasks;

namespace TepayLink.Sdisco.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
