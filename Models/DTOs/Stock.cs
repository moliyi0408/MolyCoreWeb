using MolyCoreWeb.Models.DBEntitiy;

namespace MolyCoreWeb.Models.DTOs
{

    public class StockGetListIn
    {
        public string? Q_MARKET_TYPE { get; set; }
        public string? Q_ASSETS_TYPE { get; set; }
    }

    public class StockGetListOut
    {
        public string? ErrMsg { get; set; }
        public List<StockRow>? GridList { get; set; }
       // public string? CName { get; set; }
        public int RowCnt => GridList?.Count ?? 0; // Ensure RowCnt reflects the number of items in GridList
    }

}
