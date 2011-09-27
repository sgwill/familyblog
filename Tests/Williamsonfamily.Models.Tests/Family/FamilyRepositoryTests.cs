using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamsonFamily.Models.Data;
using WilliamsonFamily.Models.Data.Tests;

namespace Williamsonfamily.Models.Tests
{
    [TestClass]
    public class FamilyRepositoryTests
    {
        #region Load Tests
        [TestMethod]
        public void Family_LoadbyFamilyName_GetFamilyByFamilyname()
        {
            string familyName = "williamson";
            int id = 1;
            var persister = GetPersister();
            persister.DataContext.Insert(new Family { PkID = id, FamilyName = familyName });

            var family = persister.Load(familyName);

            Assert.AreEqual(id, family.UniqueKey);
        }

        [TestMethod]
        public void Family_LoadbyFamilyName_InvalidFamilynameReturnsNull()
        {
            string familyName = "williamson";
            var persister = GetPersister();
            persister.DataContext.Insert(new Family { FamilyName = familyName });

            var family = persister.Load("noone");

            Assert.IsNull(family);
        }

        [TestMethod]
        public void Family_LoadbyFamilyName_TwoFamiliesWithSameFamilyNameThrowsException()
        {
            string familyName = "williamson";
            var persister = GetPersister();
            persister.DataContext.Insert(new Family { FamilyName = familyName });
            persister.DataContext.Insert(new Family { FamilyName = familyName });

            try
            {
                var user = persister.Load(familyName);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
        }
        #endregion

        #region Provider
        private FamilyRepository GetPersister()
        {
            var dc = new InMemoryDataContext();
            var dcf = new InMemoryDataContextFactory { DataContext = dc };
            return new FamilyRepository { DataContext = dc, DataContextFactory = dcf };
        }
        #endregion
    }
}