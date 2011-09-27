using System;
using System.Web.Security;
using WilliamsonFamily.Models.Web;

namespace WilliamsonFamily.Web
{
    public class AccountMembershipService : IMembershipService
    {
        #region Injectables
        private MembershipProvider MembershipProvider { get; set; }

        private void EnsureInjectables()
        {
            if (MembershipProvider == null) MembershipProvider = System.Web.Security.Membership.Provider;
        }
        #endregion

        #region Constructor
        public AccountMembershipService()
        {
            // TODO: Remove me with the coming of spring
            EnsureInjectables();
        }
        #endregion

        #region IMembershipService Members

        public int MinPasswordLength { get { return MembershipProvider.MinRequiredPasswordLength; } }

        public bool ValidateUser(string userName, string password)
        {
            return MembershipProvider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            MembershipCreateStatus status;
            MembershipProvider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            MembershipUser currentUser = MembershipProvider.GetUser(userName, true /* userIsOnline */);
            return currentUser.ChangePassword(oldPassword, newPassword);
        }

        public bool Login(string userName, string password, bool createPersistentCookie)
        {
            bool isValid = ValidateUser(userName, password);
            if (isValid)
                FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);

            return isValid;
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        #endregion
    }
}
