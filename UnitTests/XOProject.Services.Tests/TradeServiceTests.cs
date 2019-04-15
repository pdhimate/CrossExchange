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
    public class TradeServiceTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();
        private readonly TradeService _tradeService;

        public TradeServiceTests()
        {
            var shareService = new ShareService(_shareRepositoryMock.Object);
            _tradeService = new TradeService(_tradeRepositoryMock.Object, shareService);
        }

        [TearDown]
        public void Cleanup()
        {
            _tradeRepositoryMock.Reset();
            _shareRepositoryMock.Reset();
        }

        [Test]
        public async Task GetByPortfolioId_WhenExists_GetsTrades()
        {
            // Arrange
            ArrangeRates();
            ArrangeTrades();

            // Act
            var trades = await _tradeService.GetByPortfolioId(1);

            // Assert
            Assert.NotNull(trades);
            Assert.AreEqual(2, trades.Count);
        }

        [Test]
        public async Task BuyOrSell_ForNonExistantSymbol_ReturnNull()
        {
            // Arrange
            ArrangeRates();
            ArrangeTrades();

            // Act
            var trade = await _tradeService.BuyOrSell(1, "CBI-blah!", 1, "BUY");

            // Assert
            Assert.IsNull(trade);
        }

        [Test]
        public async Task BuyOrSell_ForExistantSymbol_InsertsNewTrade()
        {
            // Arrange
            ArrangeRates();
            ArrangeTrades();

            // Act
            var portfolioId = 1;
            var existingTrades = await _tradeService.GetByPortfolioId(portfolioId);
            var trade = await _tradeService.BuyOrSell(portfolioId, "CBI", 1, "BUY");
            var newTrades = await _tradeService.GetByPortfolioId(portfolioId);

            // Assert
            Assert.IsNotNull(trade);
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

        private void ArrangeTrades()
        {
            var trades = new[]
            {
                new Trade
                {
                    Id = 1,
                    Symbol = "CBI",
                    Action="BUY",
                    ContractPrice= 310.0M,
                    NoOfShares = 5,
                    PortfolioId = 1,
                },
                new Trade
                {
                    Id = 2,
                    Symbol = "REL",
                    Action="BUY",
                    ContractPrice= 230.55M,
                    NoOfShares = 50,
                    PortfolioId = 1,
                },
                new Trade
                {
                    Id = 3,
                    Symbol = "CBI",
                    Action="SELL",
                    ContractPrice= 77.15M,
                    NoOfShares = 65,
                    PortfolioId = 2,
                },
                new Trade
                {
                    Id = 4,
                    Symbol = "CBI",
                    Action="SELL",
                    ContractPrice= 100.15M,
                    NoOfShares = 12,
                    PortfolioId = 3,
                },
                new Trade
                {
                    Id = 5,
                    Symbol = "CBI",
                    Action="SELL",
                    ContractPrice= 104.15M,
                    NoOfShares = 129,
                    PortfolioId = 3,
                },
            };
            var entities = new AsyncQueryResult<Trade>(trades);
            _tradeRepositoryMock
                .Setup(mock => mock.Query())
                .Returns(entities);
        }

    }
}
