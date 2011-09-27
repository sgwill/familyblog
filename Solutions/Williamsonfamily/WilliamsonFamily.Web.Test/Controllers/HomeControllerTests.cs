using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using MvcContrib.TestHelper;
using WilliamsonFamily.Web.Controllers;

namespace WilliamsonFamily.Tests.Web.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {

        [Test]
        public void HomeController_Home_CanGetThere()
        {
            _builder.InitializeController(_controller);
            _controller.Index().AssertViewRendered().ForView("Index");
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            _controller = new HomeController();
            _builder = new TestControllerBuilder();
            _builder.InitializeController(_controller);
        }

        private HomeController _controller;
        private TestControllerBuilder _builder;
    }
}
