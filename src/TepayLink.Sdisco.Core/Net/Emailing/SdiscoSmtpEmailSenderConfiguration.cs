using Abp.Configuration;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Security;

namespace TepayLink.Sdisco.Net.Emailing
{
    public class SdiscoSmtpEmailSenderConfiguration : SmtpEmailSenderConfiguration
    {
        public SdiscoSmtpEmailSenderConfiguration(ISettingManager settingManager) : base(settingManager)
        {

        }

        public override string Password => SimpleStringCipher.Instance.Decrypt(GetNotEmptySettingValue(EmailSettingNames.Smtp.Password));
    }
}