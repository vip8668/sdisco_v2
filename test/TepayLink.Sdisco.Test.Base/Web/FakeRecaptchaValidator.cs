using System.Threading.Tasks;
using TepayLink.Sdisco.Security.Recaptcha;

namespace TepayLink.Sdisco.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
