using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using XOProject.Api.Model.Analytics;
using XOProject.Services.Domain;
using XOProject.Services.Exchange;

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace XOProject.Api.Controller
{
    [Route("api")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("daily/{symbol}/{year}/{month}/{day}")]
        public async Task<IActionResult> Daily([FromRoute] string symbol,
            [FromRoute] int year, [FromRoute][Range(1, 12)] int month, [FromRoute][Range(1, 31)] int day)
        {
            if (!IsMonthValid(month) || !IsDayValid(day))
            {
                return BadRequest();
            }

            var dateTime = new DateTime(year, month, day);
            var price = await _analyticsService.GetDailyAsync(symbol, dateTime);
            if (price == null)
            {
                return NotFound();
            }

            var result = new DailyModel()
            {
                Symbol = symbol,
                Day = dateTime,
                Price = Map(price)
            };
            return Ok(result);
        }

        [HttpGet("weekly/{symbol}/{year}/{week}")]
        public async Task<IActionResult> Weekly([FromRoute] string symbol,
            [FromRoute] int year, [FromRoute][Range(1, 54)] int week)
        {
            if (!IsWeekOfYearValid(week))
            {
                return BadRequest();
            }

            var price = await _analyticsService.GetWeeklyAsync(symbol, year, week);
            if (price == null)
            {
                return NotFound();
            }

            var result = new WeeklyModel()
            {
                Symbol = symbol,
                Year = year,
                Week = week,
                Price = Map(price)
            };
            return Ok(result);
        }

        [HttpGet("monthly/{symbol}/{year}/{month}")]
        public async Task<IActionResult> Monthly([FromRoute] string symbol,
            [FromRoute] int year, [FromRoute, Range(1, 12)] int month)
        {
            if (!IsMonthValid(month))
            {
                return BadRequest();
            }

            var price = await _analyticsService.GetMonthlyAsync(symbol, year, month);
            if (price == null)
            {
                return NotFound();
            }

            var result = new MonthlyModel()
            {
                Symbol = symbol,
                Year = year,
                Month = month,
                Price = Map(price)
            };
            return Ok(result);
        }

        #region Validations

        private static bool IsDayValid(int day)
        {
            if (day < 1 || day > 31)
            {
                return false;
            }
            return true;
        }


        private static bool IsMonthValid(int month)
        {
            if (month < 1 || month > 12)
            {
                return false;
            }
            return true;
        }

        private static bool IsWeekOfYearValid(int week)
        {
            if (week < 1 || week > 54)
            {
                return false;
            }
            return true;
        }

        #endregion

        private PriceModel Map(AnalyticsPrice price)
        {
            return new PriceModel()
            {
                Open = price.Open,
                Close = price.Close,
                High = price.High,
                Low = price.Low
            };
        }
    }
}