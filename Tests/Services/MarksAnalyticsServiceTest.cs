// <copyright file="MarksAnalyticsServiceTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Tests.Services
{
    using System.Collections.Generic;
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using Moq;
    using SadSchool.Contracts;
    using SadSchool.DbContexts;
    using SadSchool.Models.SqlServer;
    using SadSchool.Services.ApiServices;

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

            var service = new MarksAnalyticsService(
                this.mongoContextMock.Object,
                this.cacheServiceMock.Object,
                this.sadSchoolContextMock.Object);
        }

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
