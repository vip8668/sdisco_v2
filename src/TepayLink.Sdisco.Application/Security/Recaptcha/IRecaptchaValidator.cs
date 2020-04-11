using System.Threading.Tasks;

namespace TepayLink.Sdisco.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}