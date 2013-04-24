using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamsonFamily.Models.Data;
using WilliamsonFamily.Models.Data.Tests;

namespace WilliamsonFamily.Models.Data.Tests
{
    [TestClass]
    public class FamilyRepositoryTests
    {
        [TestMethod]
        public void Family_LoadbyFamilyName_GetFamilyByFamilyname()
        {
            string familyName = "williamson";
            int id = 1;
            repository.DataContext.Insert(new Family { PkID = id, FamilyName = familyName });

            var family = repository.Load(familyName);

            Assert.AreEqual(id, family.UniqueKey);
        }

        [TestMethod]
        public void Family_LoadbyFamilyName_InvalidFamilynameReturnsNull()
        {
            string familyName = "williamson";
            repository.DataContext.Insert(new Family { FamilyName = familyName });

            var family = repository.Load("noone");

            Assert.IsNull(family);
        }

        [TestMethod]
        public void Family_LoadbyFamilyName_TwoFamiliesWithSameFamilyNameThrowsException()
        {
            string familyName = "williamson";
            repository.DataContext.Insert(new Family { FamilyName = familyName });
            repository.DataContext.Insert(new Family { FamilyName = familyName });

            try
            {
                var user = repository.Load(familyName);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
        }

        FamilyRepository repository;
        [TestInitialize]
        public void Init()
        {
            var dc = new InMemoryDataContext();
            var dcf = new InMemoryDataContextFactory { DataContext = dc };
            repository = new FamilyRepository { DataContext = dc, DataContextFactory = dcf };
        }
    }
}