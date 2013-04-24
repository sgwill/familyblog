using System;
using System.Linq;
using WilliamsonFamily.Models.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamsonFamily.Models.Data;

namespace WilliamsonFamily.Models.Data.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestClass]
        public class Load
        {
            [TestMethod]
            public void User_LoadbyUserName_GetUserByUsername()
            {
                string userName = "sgwill";
                string id = "1";
                repository.DataContext.Insert(new User { PkID = id, Username = userName });

                var user = repository.Load(userName);

                Assert.AreEqual(id, user.UniqueKey);
            }

            [TestMethod]
            public void User_LoadbyUserName_InvalidUsernameReturnsNull()
            {
                string userName = "sgwill";
                repository.DataContext.Insert(new User { Username = userName });

                var user = repository.Load("noone");

                Assert.IsNull(user);
            }

            [TestMethod]
            public void User_LoadbyUserName_TwoUsersWithSameUserNameThrowsException()
            {
                string userName = "sgwill";
                repository.DataContext.Insert(new User { Username = userName });
                repository.DataContext.Insert(new User { Username = userName });

                try
                {
                    var user = repository.Load(userName);
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
                repository.DataContext.Insert(new User { Username = userName });

                var user = repository.Load(userName.ToUpper());

                Assert.IsNotNull(user);
            }

            [TestMethod]
            public void User_LoadByUserName_FallsBackToUniqueKey()
            {
                Guid g = new Guid();
                string key = g.ToString();
                repository.DataContext.Insert(new User { PkID = key, Username = "" });

                var user = repository.Load(key);

                Assert.IsNotNull(user);
            }

            UserRepository repository;
            [TestInitialize]
            public void Init()
            {
                var dc = new InMemoryDataContext();
                var dcf = new InMemoryDataContextFactory { DataContext = dc };
                repository = new UserRepository { DataContext = dc, DataContextFactory = dcf };
            }
        }

        [TestClass]
        public class Factory
        {
            [TestMethod]
            public void User_Factory_CanCreateNew()
            {
                var repository = new UserRepository();

                var newUser = repository.New();

                Assert.IsInstanceOfType(newUser, typeof(User));
            }
        }

        [TestClass]
        public class Save
        {
            [TestMethod]
            public void User_Save_CanSave()
            {
                string userName = "sam";

                repository.Save(new User { Username = userName });

                var user = repository.DataContext.Repository<User>().FirstOrDefault();
                Assert.IsNotNull(user);
            }

            [TestMethod]
            public void User_Save_UpdateNonExistingUserThrowsException()
            {
                string pkId = "1";

                try
                {
                    repository.Save(new User { PkID = pkId });
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

                int beforeCount = repository.DataContext.Repository<User>().Count();
                repository.Save(new User { Username = userName });

                int afterCount = repository.DataContext.Repository<User>().Count();
                Assert.AreEqual(1, afterCount - beforeCount);
            }

            [TestMethod]
            public void User_Save_WillNotInsertExistingUser()
            {
                string id = "1";
                string username = "sam";
                repository.DataContext.Insert(new User { PkID = id, Username = username });
                var user = repository.Load(username);

                int beforeCount = repository.DataContext.Repository<User>().Count();
                repository.Save(user);

                int afterCount = repository.DataContext.Repository<User>().Count();
                Assert.AreEqual(0, afterCount - beforeCount);
            }

            [TestMethod]
            public void User_Save_WillUpdateExistingUser()
            {
                string id = "1";
                string beforeName = "sam";
                string afterName = "sambo";
                repository.DataContext.Insert(new User { PkID = id, Username = beforeName, FirstName = beforeName });
                var user = repository.Load(beforeName);

                user.FirstName = afterName;
                repository.Save(user);

                user = repository.DataContext.Repository<User>().FirstOrDefault();
                Assert.AreEqual(afterName, user.FirstName);
            }

            UserRepository repository;
            [TestInitialize]
            public void Init()
            {
                var dc = new InMemoryDataContext();
                var dcf = new InMemoryDataContextFactory { DataContext = dc };
                repository = new UserRepository { DataContext = dc, DataContextFactory = dcf };
            }
        }

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