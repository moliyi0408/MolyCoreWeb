using Microsoft.AspNetCore.Mvc;
using MolyCoreWeb.Models.DBEntitiy;
using MolyCoreWeb.Models.DTOs;

namespace MolyCoreWeb.Services
{
    public interface IStockService :  IService<StockRow>
    {
        Task<StockGetListOut> GetStockListAsync(StockGetListIn inModel);
        Task GetStockListUpdateAsync(StockGetListIn inModel); // 添加此方法的定義
        Task<StockInfo> GetStockInfoAsync(string stockCode);
        Task GetBusinessIndicatorsUpdateAsync();
        Task<IEnumerable<BusinessIndicator>> GetBusinessIndicatorsAsync();
        //Task<StockGetRealtimePriceOut> GetRealtimePriceAsync(StockGetRealtimePriceIn inModel);
        //Task<StockGetDayPriceOut> GetDayPriceAsync(StockGetDayPriceIn inModel);
        //Task<StockGetMonthPriceOut> GetMonthPriceAsync(StockGetMonthPriceIn inModel);  
    }
}
