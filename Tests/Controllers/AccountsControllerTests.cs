// <copyright file="AccountsControllerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Tests.Controllers
{
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using SadSchool.ViewModels;

    /// <summary>
    /// Tests for the <see cref="SadSchool.Controllers.AccountController"/> class.
    /// </summary>
    public class AccountsControllerTests
    {
        private readonly Mock<SignInManager<IdentityUser>> signInManagerMock;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountsControllerTests"/> class.
        /// </summary>
        public AccountsControllerTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            // Get mock with all constructor dependencies automatically injected
            this.signInManagerMock = fixture.Create<Mock<SignInManager<IdentityUser>>>();
        }

        /// <summary>
        /// Tests the Login method of the AccountController with a valid model.
        /// </summary>
        /// <returns>Task (void).</returns>
        [Fact]
        public async Task Login_ValidModel_InvalidPassword_ReturnsViewWithError()
        {
            // Arrange
            var model = new LoginViewModel
            {
                UserName = "testuser",
                Password = "wrongpassword",
            };

            this.signInManagerMock
                .Setup(s => s.PasswordSignInAsync(model.UserName, model.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var controller = new SadSchool.Controllers.AccountController(this.signInManagerMock.Object);

            // Act
            var result = await controller.Login(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
            Assert.False(controller.ModelState.IsValid); // Optional: Check ModelState
        }
    }
}
