using System;

namespace WilliamsonFamily.Models.Communication
{
    public interface IEmail
    {
        string ToEmailAddress { get; set; }
        string FromEmailAddress { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
    }
}