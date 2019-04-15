using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using XOProject.Repository.Domain;
using XOProject.Repository.Exchange;
using XOProject.Services.Exchange;
using XOProject.Services.Tests.Helpers;

namespace XOProject.Services.Tests
{
    public class AnalyticsServiceTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();

        private readonly AnalyticsService _analyticsService;

        public AnalyticsServiceTests()
        {
            _analyticsService = new AnalyticsService(_shareRepositoryMock.Object);
        }

        [TearDown]
        public void Cleanup()
        {
            _shareRepositoryMock.Reset();
        }

        [Test]
        public async Task GetDailyAsync_WhenExists_GetsAnayticsPriceForTheDay()
        {
            // Arrange
            ArrangeRates();

            // Act
            var price = await _analyticsService.GetDailyAsync("CBI", new DateTime(2018, 08, 17, 5, 10, 15));

            // Assert
            Assert.NotNull(price);
            Assert.AreEqual(300, price.Open);
            Assert.AreEqual(400, price.Close);
            Assert.AreEqual(400, price.High);
            Assert.AreEqual(300, price.Low);
        }

        [Test]
        public async Task GetWeeklyAsync_WhenExists_GetsAnayticsPriceForTheWeek()
        {
            // Arrange
            ArrangeRates();

            // Act
            var price = await _analyticsService.GetWeeklyAsync("CBI", 2019, 1);

            // Assert
            Assert.NotNull(price);
            Assert.AreEqual(999, price.Open);
            Assert.AreEqual(1999, price.Close);
            Assert.AreEqual(1999, price.High);
            Assert.AreEqual(999, price.Low);
        }


        [Test]
        public async Task GetMonthlyAsync_WhenExists_GetsAnayticsPriceForTheMonth()
        {
            // Arrange
            ArrangeRates();

            // Act
            var price = await _analyticsService.GetMonthlyAsync("CBI", 2019, 1);

            // Assert
            Assert.NotNull(price);
            Assert.AreEqual(999, price.Open);
            Assert.AreEqual(2010, price.Close);
            Assert.AreEqual(2010, price.High);
            Assert.AreEqual(201, price.Low);
        }

        private void ArrangeRates()
        {
            var rates = new[]
            {
                new HourlyShareRate
                {
                    Id = 1,
                    Symbol = "CBI",
                    Rate = 310.0M,
                    TimeStamp = new DateTime(2017, 08, 17, 5, 0, 0)
                },
                new HourlyShareRate
                {
                    Id = 2,
                    Symbol = "CBI",
                    Rate = 320.0M,
                    TimeStamp = new DateTime(2018, 08, 16, 5, 0, 0)
                },
                new HourlyShareRate
                {
                    Id = 3,
                    Symbol = "REL",
                    Rate = 300.0M,
                    TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
                },
                new HourlyShareRate
                {
                    Id = 4,
                    Symbol = "CBI",
                    Rate = 300.0M,
                    TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
                },
                new HourlyShareRate
                {
                    Id = 5,
                    Symbol = "CBI",
                    Rate = 400.0M,
                    TimeStamp = new DateTime(2018, 08, 17, 6, 0, 0)
                },
                new HourlyShareRate
                {
                    Id = 6,
                    Symbol = "IBM",
                    Rate = 300.0M,
                    TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
                },
                 new HourlyShareRate
                {
                    Id = 7,
                    Symbol = "CBI",
                    Rate = 999.0M,
                    TimeStamp = new DateTime(2019, 01, 01, 6, 0, 0)
                },
                  new HourlyShareRate
                {
                    Id = 8,
                    Symbol = "CBI",
                    Rate = 1999.0M,
                    TimeStamp = new DateTime(2019, 01, 02, 3, 0, 0)
                },

                      new HourlyShareRate
                {
                    Id = 9,
                    Symbol = "CBI",
                    Rate = 201,
                    TimeStamp = new DateTime(2019, 01, 20, 1, 0, 0)
                },
                          new HourlyShareRate
                {
                    Id = 10,
                    Symbol = "CBI",
                    Rate = 2010,
                    TimeStamp = new DateTime(2019, 01, 30, 2, 0, 0)
                },
            };
            _shareRepositoryMock
                .Setup(mock => mock.Query())
                .Returns(new AsyncQueryResult<HourlyShareRate>(rates));
        }
    }
}
