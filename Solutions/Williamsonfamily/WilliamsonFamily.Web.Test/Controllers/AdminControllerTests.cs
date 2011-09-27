﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using MvcContrib.TestHelper;
using WilliamsonFamily.Web.Controllers;
using Rhino.Mocks;
using System.Web.Security;
using WilliamsonFamily.Models.Web;
using WilliamsonFamily.Web.Models.Admin;

namespace WilliamsonFamily.Tests.Web.Controllers
{
    [TestFixture]
    public class AdminControllerTests
    {
        #region Index
        [Test]
        public void AdminController_Index_ReturnsIndex()
        {
            _controller.Index()
                .AssertViewRendered()
                .ForView("Index");
        }
        #endregion

        #region Users
        [Test]
        public void AdminController_Users_Renders_Users()
        {
            _controller.Users()
                .AssertViewRendered()
                .ForView("Users");
        }

        [Test]
        public void AdminController_Users_GetAllUsers_IsCalled()
        {
            int outs = 0;
            // Act
            _controller.Users();

            // Assert
            _controller.MembershipProvider
                .AssertWasCalled(m => m.GetAllUsers(0, 100, out outs));
        }

        // NOTE: Can't test if anything gets called because it's sealed classes. Oh well.
        #endregion

        #region UserDetail
        [Test]
        public void AdminController_UserDetail_Renders_UserDetail()
        {
            _controller.UserDetail("user")
                .AssertViewRendered()
                .ForView("UserDetail");
        }

        [Test]
        public void AdminController_UserDetail_EmptyUser_RedirectsToUsers()
        {
            _controller.UserDetail("")
                .AssertActionRedirect()
                .ToAction("Users");
        }

        [Test]
        public void AdminController_UserDetail_GetUser_GetsCalled()
        {
            string user = "user";
            
            // Act
            _controller.UserDetail(user);

            // Assert
            _controller.MembershipProvider
                .AssertWasCalled(m => m.GetUser(user, false));
        }
        #endregion

        #region Create
        [Test]
        public void AdminController_CreateGet_RendersCreate()
        {
            _controller.Create()
                .AssertViewRendered()
                .ForView("Create");
        }

        [Test]
        public void AdminController_CreateGet_ViewModel_IsNewUserModel()
        {
            // Act
            _controller.Create();

            // Assert
            Assert.IsInstanceOf<NewUserModel>(_controller.ViewData.Model);
        }

        [Test]
        public void AdminController_CreatePost_Success_RedirectsToUser()
        {
            // Arrange
            var newUser = MockRepository.GenerateStub<NewUserModel>();
            newUser.UserName = "s@S.com";

            // Assert
            _controller.Create(newUser)
                .AssertActionRedirect()
                .ToAction("Users");
        }

        [Test]
        public void AdminController_CreatePost_CreateUser_GetsCalled()
        {
            // Arrange
            var newUser = MockRepository.GenerateStub<NewUserModel>();
            newUser.Email = "s@S.com";
            newUser.UserName = "user";
            newUser.Password = "pass";
            MembershipCreateStatus status = MembershipCreateStatus.Success;

            // Act
            _controller.Create(newUser);

            // Assert
            _controller.MembershipProvider
                .AssertWasCalled(m => m.CreateUser(newUser.UserName, newUser.Password, newUser.Email, "", "", true, null, out status));
        }

        [Test]
        public void AdminController_CreatePost_CreateUser_NonSuccess_RendersCreate()
        {
            // Arrange
            var newUser = MockRepository.GenerateStub<NewUserModel>();

            // Assert
            _controller.Create(newUser)
                .AssertViewRendered()
                .ForView("Create");
        }

        [Test]
        public void AdminController_CreatePost_NonSuccess_ViewModel_IsNewUserModel()
        {
             // Arrange
            var newUser = MockRepository.GenerateStub<NewUserModel>();

            // Assert
            _controller.Create(newUser);

            // Assert
            Assert.IsInstanceOf<NewUserModel>(_controller.ViewData.Model);
        }
        #endregion

        #region Delete
        [Test]
        public void AdminController_Delete_DeleteGetsCalled()
        {
            // Act
            _controller.Delete("user");
                
            // Assert
            _controller.MembershipProvider
                .AssertWasCalled(m => m.DeleteUser("user", true));
        }

        [Test]
        public void AdminController_Delete_RedirectsToUsers()
        {
            _controller.Delete("user")
                .AssertActionRedirect()
                .ToAction("Users");
        }

        [Test]
        public void AdminController_Delete_EmptyUsername_RedirectsToUsers()
        {
            _controller.Delete("")
               .AssertActionRedirect()
               .ToAction("Users");
        }
        #endregion

        #region Setup

        private AdminController _controller;
        private TestControllerBuilder _builder;

        [TestFixtureSetUp]
        public void Init()
        {
            _controller = new AdminController(false);

            _controller.MembershipProvider = MockRepository.GenerateStub<MembershipProvider>();

            _builder = new TestControllerBuilder();
            _builder.InitializeController(_controller);
        }
        #endregion
    }
}