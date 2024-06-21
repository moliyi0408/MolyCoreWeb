using MolyCoreWeb.Models.DBEntitiy;

namespace MolyCoreWeb.Models.DTOs
{
    public class Stock
    {
    }

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



    //public class StockPriceRow
    //{
    //    public string? symbolCode { get; set; }
    //    public string? symbolName { get; set; }
    //    public string? date { get; set; }
    //    public string? open { get; set; }
    //    public string? high { get; set; }
    //    public string? low { get; set; }
    //    public string? close { get; set; }
    //    public string? volume { get; set; }
    //}




    //public class StockGetDayPriceIn
    //{
    //    public string Sample2_Date { get; set; }
    //}

    //public class StockGetDayPriceOut
    //{
    //    public string ErrMsg { get; set; }
    //    public List<StockPriceRow> GridList { get; set; }
    //}

    //public class StockGetMonthPriceIn
    //{
    //    public string Sample3_Symbol { get; set; }
    //    public string Sample3_Date { get; set; }
    //}

    //public class StockGetMonthPriceOut
    //{
    //    public string ErrMsg { get; set; }
    //    public List<StockPriceRow> GridList { get; set; }
    //}

    //public class StockGetRealtimePriceIn
    //{
    //    public string Sample1_Symbol { get; set; }
    //}

    //public class StockGetRealtimePriceOut
    //{
    //    public string ErrMsg { get; set; }
    //    public string realPrice { get; set; }
    //}

    //public class TwsePriceSchema
    //{
    //    public QueryTime queryTime { get; set; }
    //    public string referer { get; set; }
    //    public string rtmessage { get; set; }
    //    public string exKey { get; set; }
    //    public IList<MsgArray> msgArray { get; set; }
    //    public int userDelay { get; set; }
    //    public string rtcode { get; set; }
    //    public int cachedAlive { get; set; }
    //}

    //public class QueryTime
    //{
    //    public int? stockInfoItem { get; set; }
    //    public string? sessionKey { get; set; }
    //    public string? sessionStr { get; set; }
    //    public string? sysDate { get; set; }
    //    public int? sessionFromTime { get; set; }
    //    public int? stockInfo { get; set; }
    //    public bool? showChart { get; set; }
    //    public int? sessionLatestTime { get; set; }
    //    public string? sysTime { get; set; }
    //}

    //public class MsgArray
    //{
    //    public string? n { get; set; }
    //    public string? g { get; set; }
    //    public string? u { get; set; }
    //    public string? mt { get; set; }
    //    public string? o { get; set; }
    //    public string? ps { get; set; }
    //    public string? tk0 { get; set; }
    //    public string? a { get; set; }
    //    public string? tlong { get; set; }
    //    public string? t { get; set; }
    //    public string? it { get; set; }
    //    public string? ch { get; set; }
    //    public string? b { get; set; }
    //    public string? f { get; set; }
    //    public string? w { get; set; }
    //    public string? pz { get; set; }
    //    public string? l { get; set; }
    //    public string? c { get; set; }
    //    public string? v { get; set; }
    //    public string? d { get; set; }
    //    public string? tv { get; set; }
    //    public string? tk1 { get; set; }
    //    public string? ts { get; set; }
    //    public string? nf { get; set; }
    //    public string? y { get; set; }
    //    public string? p { get; set; }
    //    public string? i { get; set; }
    //    public string? ip { get; set; }
    //    public string? z { get; set; }
    //    public string? s { get; set; }
    //    public string? h { get; set; }
    //    public string? ex { get; set; }
    //}
}
