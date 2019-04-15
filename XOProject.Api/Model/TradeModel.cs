using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XOProject.Api.ValidationAttributes;

namespace XOProject.Api.Model
{
    public class TradeModel
    {
        [Required]
        public string Symbol { get; set; }

        [Required]
        public int NoOfShares { get; set; }

        [Required]
        public int PortfolioId { get; set; }

        [StringRange(AllowedValues = new[] { "BUY", "SELL" }, ErrorMessage = "Allowed values are BUY or SELL")]
        public string Action { get; set; }
    }
}