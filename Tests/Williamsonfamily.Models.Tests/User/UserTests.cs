using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamsonFamily.Models.User;

namespace WilliamsonFamily.Models.Data.Tests
{
    [TestClass]
    public class UserTests
    {
        #region Load
       
        [TestMethod]
        public void User_LoadbyUserName_GetUserByUsername()
        {
            string userName = "sgwill";
            string id = "1";
            var persister = GetPersister();
            persister.DataContext.Insert(new User { PkID = id, Username = userName });

            var user = persister.Load(userName);

            Assert.AreEqual(id, user.UniqueKey);
        }

        [TestMethod]
        public void User_LoadbyUserName_InvalidUsernameReturnsNull()
        {
            string userName = "sgwill";
            var persister = GetPersister();
            persister.DataContext.Insert(new User { Username = userName });

            var user = persister.Load("noone");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void User_LoadbyUserName_TwoUsersWithSameUserNameThrowsException()
        {
            string userName = "sgwill";
            var persister = GetPersister();
            persister.DataContext.Insert(new User { Username = userName });
            persister.DataContext.Insert(new User { Username = userName });

            try
            {
                var user = persister.Load(userName);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void User_LoadByUserName_IgnoresCase()
        {
            string userName = "sgwill";
            var persister = GetPersister();
            persister.DataContext.Insert(new User { Username = userName });

            var user = persister.Load(userName.ToUpper());

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void User_LoadByUserName_FallsBackToUniqueKey()
        {
            Guid g = new Guid();
            string key = g.ToString();
            var persister = GetPersister();
            persister.DataContext.Insert(new User {  PkID = key, Username = "" });

            var user = persister.Load(key);

            Assert.IsNotNull(user);
        }
        #endregion

        #region Factory Tests
        [TestMethod]
        public void User_Factory_CanCreateNew()
        {
            var persister = GetPersister();

            var newUser = persister.New();

            Assert.IsInstanceOfType(newUser, typeof(User));
        }
        #endregion

        #region Save Tests
        [TestMethod]
        public void User_Save_CanSave()
        {
            string userName = "sam";
            var persister = GetPersister();

            persister.Save(new User { Username = userName });

            var user = persister.DataContext.Repository<User>().FirstOrDefault();
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void User_Save_UpdateNonExistingUserThrowsException()
        {
            string pkId = "1";
            var persister = GetPersister();

            try
            {
                persister.Save(new User { PkID = pkId });
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void User_Save_WillInsertNewUser()
        {
            string userName = "sam";
            var persister = GetPersister();

            int beforeCount = persister.DataContext.Repository<User>().Count();
            persister.Save(new User { Username = userName });

            int afterCount = persister.DataContext.Repository<User>().Count();
            Assert.AreEqual(1, afterCount - beforeCount);
        }

        [TestMethod]
        public void User_Save_WillNotInsertExistingUser()
        {
            string id = "1";
            string username = "sam";
            var persister = GetPersister();
            persister.DataContext.Insert(new User { PkID = id, Username = username });
            var user = persister.Load(username);

            int beforeCount = persister.DataContext.Repository<User>().Count();
            persister.Save(user);

            int afterCount = persister.DataContext.Repository<User>().Count();
            Assert.AreEqual(0, afterCount - beforeCount);
        }

        [TestMethod]
        public void User_Save_WillUpdateExistingUser()
        {
            string id = "1";
            string beforeName = "sam";
            string afterName = "sambo";
            var persister = GetPersister();
            persister.DataContext.Insert(new User { PkID = id, Username = beforeName, FirstName = beforeName });
            var user = persister.Load(beforeName);

            user.FirstName = afterName;
            persister.Save(user);

            user = persister.DataContext.Repository<User>().FirstOrDefault();
            Assert.AreEqual(afterName, user.FirstName);
        }
        #endregion

        #region Provider
        private UserRepository GetPersister()
        {
            var dc = new InMemoryDataContext();
            var dcf = new InMemoryDataContextFactory { DataContext = dc };
            return new UserRepository { DataContext = dc, DataContextFactory = dcf };
        }
        #endregion

        public class OtherUser : IUser
        {
            public string Username { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? Birthdate { get; set; }
            public string UniqueKey { get; set; }
            public string Email { get; set; }
        }
    }
}