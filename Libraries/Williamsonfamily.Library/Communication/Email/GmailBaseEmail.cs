using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WilliamsonFamily.Models.Communication;
using System.Configuration;

namespace WilliamsonFamily.Library.Communication.Email
{
    public abstract class GmailBaseEmail
    {
        public string FromEmailAddress
        {
            get
            {
                return (ConfigurationManager.AppSettings["FromEmailAddress"] ?? "williamsonfamily.com@gmail.com").ToString();
            }
            set { }
        }
    }
}