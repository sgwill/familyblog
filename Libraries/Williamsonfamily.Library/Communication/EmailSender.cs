using System;
using System.Net.Mail;
using WilliamsonFamily.Models.Communication;

namespace WilliamsonFamily.Library.Communication
{
    public class EmailSender : IEmailSender
    {
        #region IEmailSender Members

        public void Send(IEmail email)
        {
            var smtp = new SmtpClient();
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage message = new MailMessage(new MailAddress(email.FromEmailAddress, "Williamson Family Website"), new MailAddress(email.ToEmailAddress))
                {
                    Subject = email.Subject,
                    Body = email.Body,
                    IsBodyHtml = true
                };

            smtp.Send(message);
        }

        #endregion
    }
}