using Abp.MailKit;
using Abp.Net.Mail.Smtp;
using MailKit.Net.Smtp;

namespace TepayLink.Sdisco.Net.Emailing
{
    public class SdiscoMailKitSmtpBuilder : DefaultMailKitSmtpBuilder
    {
        public SdiscoMailKitSmtpBuilder(
            ISmtpEmailSenderConfiguration smtpEmailSenderConfiguration,
            IAbpMailKitConfiguration abpMailKitConfiguration) : base(smtpEmailSenderConfiguration, abpMailKitConfiguration)
        {

        }

        protected override void ConfigureClient(SmtpClient client)
        {
            client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            base.ConfigureClient(client);
        }
    }
}
