using System;
using WilliamsonFamily.Models.Communication;

namespace WilliamsonFamily.Library.Communication.Email
{
    public class GmailEmail : GmailBaseEmail, IEmail
    {
        #region IEmail Members

        public string ToEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        #endregion
    }
}
