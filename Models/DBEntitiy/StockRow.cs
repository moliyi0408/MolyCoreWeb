using System.ComponentModel.DataAnnotations;

namespace MolyCoreWeb.Models.DBEntitiy
{
    public class StockRow
    {
        [Key]
        public string? STOCK_CODE { get; set; }
        public string? STOCK_NAME { get; set; }
        public string? ISIN_CODE { get; set; }
        public string? PUBLIC_DATE { get; set; }
        public string? MARKET_TYPE { get; set; }
        public string? INDUSTRY { get; set; }
        public string? CFI_CODE { get; set; }
        public string? ASSETS_TYPE { get; set; }

    }

}
