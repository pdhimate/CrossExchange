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
    public class ShareServiceTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();

        private readonly ShareService _shareService;

        public ShareServiceTests()
        {
            _shareService = new ShareService(_shareRepositoryMock.Object);
        }

        [TearDown]
        public void Cleanup()
        {
            _shareRepositoryMock.Reset();
        }

        [Test]
        public async Task GetHourlyAsync_WhenExists_GetsHourlySharePrice()
        {
            // Arrange
            ArrangeRates();

            // Act
            var result = await _shareService.GetHourlyAsync("CBI", new DateTime(2018, 08, 17, 5, 10, 15));

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(4, result.Id);
        }

        [Test]
        public async Task GetBySymbolAsync_WhenExists_GetsSharePricesBySymbol()
        {
            // Arrange
            ArrangeRates();

            // Act
            var prices = await _shareService.GetBySymbolAsync("CBI");

            // Assert
            Assert.NotNull(prices);
            Assert.AreEqual(4, prices.Count);
        }

        [Test]
        public async Task GetLastPriceAsync_WhenExists_GetsLatestPricesBySymbol()
        {
            // Arrange
            ArrangeRates();

            // Act
            var lastPrice = await _shareService.GetLastPriceAsync("CBI");

            // Assert
            Assert.NotNull(lastPrice);
            Assert.AreEqual(400, lastPrice.Rate);
        }

        [Test]
        public async Task UpdateLastPriceAsync_WhenNonExistant_ReturnsNull()
        {
            // Arrange
            ArrangeRates();

            // Act
            var updatedPrice = await _shareService.UpdateLastPriceAsync("CBI-Blah!", 9999);

            // Assert
            Assert.IsNull(updatedPrice);
        }

        [Test]
        public async Task UpdateLastPriceAsync_WhenExists_UpdatesTheLatestPrice()
        {
            // Arrange
            ArrangeRates();

            // Act
            var oldPrice = await _shareService.GetLastPriceAsync("CBI");
            var newPrice = oldPrice.Rate + 1;
            var updatedPrice = await _shareService.UpdateLastPriceAsync("CBI", newPrice);

            // Assert
            Assert.IsNotNull(updatedPrice);
            Assert.AreEqual(updatedPrice.Rate, newPrice);
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
            };
            _shareRepositoryMock
                .Setup(mock => mock.Query())
                .Returns(new AsyncQueryResult<HourlyShareRate>(rates));
        }
    }
}
