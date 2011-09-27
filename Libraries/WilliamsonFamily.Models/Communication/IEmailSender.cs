using System;

namespace WilliamsonFamily.Models.Communication
{
    public interface IEmailSender
    {
        void Send(IEmail email);
    }
}