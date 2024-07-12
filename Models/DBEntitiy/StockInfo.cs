using System.ComponentModel.DataAnnotations;

namespace MolyCoreWeb.Models.DBEntitiy
{
    public class StockInfo
    {
        [Key]
        public string? StockCode { get; set; }
    }
}
