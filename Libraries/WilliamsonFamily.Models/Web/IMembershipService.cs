using System;
using System.Web.Security;

namespace WilliamsonFamily.Models.Web
{
    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool Login(string userName, string password, bool createPersistentCookie);
        void Logout();
        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }
}
