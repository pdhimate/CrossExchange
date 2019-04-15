using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XOProject.Repository.Domain;
using XOProject.Repository.Exchange;
using XOProject.Services.Domain;

namespace XOProject.Services.Exchange
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IShareRepository _shareRepository;

        public AnalyticsService(IShareRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        #region IAnalyticsService implementation

        public async Task<AnalyticsPrice> GetDailyAsync(string symbol, DateTime day)
        {
            var orderedHourlyRatesForTheDay = _shareRepository.Query()
                                                .Where(r => r.Symbol == symbol
                                                        && r.TimeStamp.Year == day.Year
                                                        && r.TimeStamp.Month == day.Month
                                                        && r.TimeStamp.Day == day.Day)
                                                .OrderBy(r => r.TimeStamp);

            AnalyticsPrice analyticsPrice = CalculateAnalyticsPrice(orderedHourlyRatesForTheDay);
            return await Task.FromResult(analyticsPrice);
        }

        public async Task<AnalyticsPrice> GetWeeklyAsync(string symbol, int year, int week)
        {
            var orderedRatesForTheWeek = _shareRepository.Query()
                                                .Where(r => r.Symbol == symbol
                                                        && r.TimeStamp.Year == year
                                                        && GetWeekNumber(r.TimeStamp) == week)
                                                .OrderBy(r => r.TimeStamp);
            AnalyticsPrice analyticsPrice = CalculateAnalyticsPrice(orderedRatesForTheWeek);
            return await Task.FromResult(analyticsPrice);
        }

        public async Task<AnalyticsPrice> GetMonthlyAsync(string symbol, int year, int month)
        {
            var orderedHourlyRatesForTheMonth = _shareRepository.Query()
                                               .Where(r => r.Symbol == symbol
                                                       && r.TimeStamp.Year == year
                                                       && r.TimeStamp.Month == month)
                                               .OrderBy(r => r.TimeStamp);

            AnalyticsPrice analyticsPrice = CalculateAnalyticsPrice(orderedHourlyRatesForTheMonth);
            return await Task.FromResult(analyticsPrice);
        } 

        #endregion

        #region Local helpers

        private int GetWeekNumber(DateTime dateTime)
        {
            int weekNum = CultureInfo.CurrentCulture.Calendar
                .GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        private static AnalyticsPrice CalculateAnalyticsPrice(IOrderedQueryable<HourlyShareRate> ratesOrderedByTimestamp)
        {
            if (!ratesOrderedByTimestamp.Any())
            {
                return null;
            }

            return new AnalyticsPrice
            {
                High = ratesOrderedByTimestamp.Max(r => r.Rate),
                Low = ratesOrderedByTimestamp.Min(r => r.Rate),
                Open = ratesOrderedByTimestamp.First().Rate,
                Close = ratesOrderedByTimestamp.Last().Rate,
            };
        }

        #endregion
    }
}