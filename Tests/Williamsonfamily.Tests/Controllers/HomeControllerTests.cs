using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using WilliamsonFamily.Web.Controllers;

namespace WilliamsonFamily.Web.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {

        [TestMethod]
        public void Home_CanGetThere()
        {
            _builder.InitializeController(_controller);
            _controller.Index().AssertViewRendered().ForView("Index");
        }

        [TestInitialize]
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
