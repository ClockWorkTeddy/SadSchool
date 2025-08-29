// <copyright file="MarksAnalyticsServiceTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Tests.Services
{
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using Moq;
    using SadSchool.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Services.ApiServices;

    /// <summary>
    /// Provides unit tests for the <see cref="MarksAnalyticsService"/> class,  verifying its behavior and functionality
    /// under various conditions.
    /// </summary>
    /// <remarks>This test class uses mocks for dependencies such as <see cref="SadSchoolContext"/>,  <see
    /// cref="MongoContext"/>, and <see cref="ICacheService"/> to isolate the  functionality of <see
    /// cref="MarksAnalyticsService"/>. It includes test methods  to validate specific scenarios, such as calculating
    /// average marks.</remarks>
    public class MarksAnalyticsServiceTest
    {
        private readonly Mock<SadSchoolContext> sadSchoolContextMock;
        private readonly Mock<MongoContext> mongoContextMock;
        private readonly Mock<ICacheService> cacheServiceMock;

        private readonly IMarksAnalyticsService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarksAnalyticsServiceTest"/> class.
        /// </summary>
        public MarksAnalyticsServiceTest()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            this.sadSchoolContextMock = new Mock<SadSchoolContext>();
            this.mongoContextMock = new Mock<MongoContext>();
            this.cacheServiceMock = new Mock<ICacheService>();

            this.sadSchoolContextMock = fixture.Create<Mock<SadSchoolContext>>();
            this.mongoContextMock = fixture.Create<Mock<MongoContext>>();
            this.cacheServiceMock = fixture.Create<Mock<ICacheService>>();
        }

        /// <summary>
        /// Tests the <see cref="IMarksAnalyticsService.GetAverageMarks(int, int)"/> method to ensure it returns a valid
        /// result.
        /// </summary>
        /// <remarks>This test verifies that the <see cref="IMarksAnalyticsService.GetAverageMarks(int, int)"/>
        /// method does not return a null value when provided with valid student and subject identifiers.</remarks>
        [Fact]
        public void GetAverageMarksTest()
        {
            // Arrange
            var studentId = 1;
            var subjectId = 1;

            // Act
            var result = this.service.GetAverageMarks(studentId, subjectId);

            // Assert
            Assert.NotNull(result);
        }
    }
}
