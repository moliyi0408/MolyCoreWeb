using CsvHelper;
using CsvHelper.Configuration;
using HtmlAgilityPack;
using MolyCoreWeb.Models.DTOs;
using System.Globalization;
using System.Text;
using System.Linq;
using MolyCoreWeb.Repositorys;
using MolyCoreWeb.Models.DBEntitiy;
using System.Security.Cryptography.X509Certificates;

namespace MolyCoreWeb.Services
{
    public class StockService : IStockService
    {
       
        private readonly IUnitOfWork _unitOfWork;

        public StockService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<StockGetListOut> GetStockListAsync(StockGetListIn inModel)
        {
            StockGetListOut outModel = new();

            try
            {
                IEnumerable<StockRow> stockRows;

                string marketTypeCondition = string.Empty;
                string assetsTypeCondition = string.Empty;

                switch (inModel.Q_MARKET_TYPE)
                {
                    case "TWSE":
                        marketTypeCondition = "上市";
                        break;
                    case "OTC":
                        marketTypeCondition = "上櫃";
                        break;
                    default:
                        outModel.ErrMsg = "Invalid market type.";
                        return outModel;
                }

                switch (inModel.Q_ASSETS_TYPE)
                {
                    case "股票":
                        assetsTypeCondition = "股票";
                        break;
                    case "ETF":
                        assetsTypeCondition = "ETF";
                        break;
                }

                string sqlQuery =  string.Format(@" SELECT * FROM Stocks 
                                                    WHERE 1 = 1
                                                    AND MARKET_TYPE = '{0}'
                                                    AND ASSETS_TYPE =  '{1}' "
                        , marketTypeCondition, assetsTypeCondition);

                stockRows = await _unitOfWork.Repository<StockRow>().ExecuteSqlQueryAsync(sqlQuery);

                outModel.GridList = stockRows.ToList();
            }
            catch (Exception ex)
            {
                outModel.ErrMsg = $"An error occurred while reading the database: {ex.Message}";
            }

            return outModel;
        }

        public async Task GetStockListUpdateAsync(StockGetListIn inModel)
        {
            if (string.IsNullOrEmpty(inModel.Q_MARKET_TYPE) || string.IsNullOrEmpty(inModel.Q_ASSETS_TYPE))
            {
                Console.WriteLine("請輸入市場別和資產別");
                return;
            }

            string url = inModel.Q_MARKET_TYPE switch
            {
                "TWSE" => "https://isin.twse.com.tw/isin/C_public.jsp?strMode=2",
                "OTC" => "https://isin.twse.com.tw/isin/C_public.jsp?strMode=4",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(url))
            {
                Console.WriteLine("無效的市場別");
                return;
            }

            List<StockRow> stockRows = await CrawlStockDataAsync(url);

            if (stockRows != null && stockRows.Count > 0)
            {
                foreach (var stock in stockRows)
                {
                    var existingStock = await _unitOfWork.Repository<StockRow>().GetByCondition(s => s.STOCK_CODE == stock.STOCK_CODE);
                    if (existingStock != null)
                    {
                        existingStock.STOCK_NAME = stock.STOCK_NAME;
                        existingStock.ISIN_CODE = stock.ISIN_CODE;
                        existingStock.PUBLIC_DATE = stock.PUBLIC_DATE;
                        existingStock.MARKET_TYPE = stock.MARKET_TYPE;
                        existingStock.INDUSTRY = stock.INDUSTRY;
                        existingStock.CFI_CODE = stock.CFI_CODE;
                        existingStock.ASSETS_TYPE = stock.ASSETS_TYPE;

                        _unitOfWork.Repository<StockRow>().Update(existingStock);
                    }
                    else
                    {
                        await _unitOfWork.Repository<StockRow>().Create(stock);
                    }
                }
                await _unitOfWork.CompleteAsync(); // 保存更改
            }
            else
            {
                Console.WriteLine("No data extracted.");
            }
        }
        private static async Task<List<StockRow>> CrawlStockDataAsync(string url)
        {
            List<StockRow> stockRows = [];

            try
            {
                // 爬資料
                using HttpClient client = new();
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                // 指定編碼來讀取內容
                Encoding big5 = Encoding.GetEncoding("Big5");

                // 將回應內容讀取為 byte 陣列
                byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();

                // 使用指定的編碼將 byte 陣列轉換為 string
                string html = big5.GetString(responseBytes);

                // 解析 HTML 取得股票資料
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var table = doc.DocumentNode.SelectSingleNode("//table[2]");

                if (table != null)
                {
                    var rows = table.SelectNodes("tr");
                    string assetsType = string.Empty;

                    foreach (var row in rows)
                    {
                        var cells = row.SelectNodes("td");

                        if (cells != null && cells.Count > 0)
                        {
                            string stockCode = string.Empty;
                            string stockName = string.Empty;

                            var codeAndName = cells[0].InnerText.Trim();

                            if (codeAndName.Contains("　"))
                            {
                                var parts = codeAndName.Split('　');
                                stockCode = parts[0].Trim();
                                stockName = parts[1].Trim();
                            }
                            else if (codeAndName.Contains("有價證券代號及名稱"))
                            {
                                 var parts = codeAndName.Split(new[] { '及' }, 2); // 仅拆分成两部分
                                if (parts.Length == 2)
                                {
                                    stockCode = parts[0].Trim().Replace("有價", string.Empty);
                                    stockName = "證券" + parts[1].Trim();
                                }
                                else
                                {
                                    stockCode = codeAndName;
                                    stockName = string.Empty;
                                }
                            }
                            else
                            {
                                assetsType = codeAndName;
                                continue;
                            }
                            
                            var stockRow = new StockRow
                            {
                                STOCK_CODE = stockCode,
                                STOCK_NAME = stockName,
                                ISIN_CODE = cells[1].InnerText.Trim(),
                                PUBLIC_DATE = cells[2].InnerText.Trim(),
                                MARKET_TYPE = cells[3].InnerText.Trim(),
                                INDUSTRY = cells[4].InnerText.Trim(),
                                CFI_CODE = cells[5].InnerText.Trim(),
                                ASSETS_TYPE = assetsType
                            };

                            stockRows.Add(stockRow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return stockRows;
        }

        public Task<IEnumerable<StockRow>> GetAllAsync()
        {
            return  _unitOfWork.Repository<StockRow> ().GetAllAsync();
        }

        public Task<StockRow> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Create(StockRow stock)
        {
            var existingStock = await _unitOfWork.Repository<StockRow>().GetByCondition(s => s.STOCK_CODE == stock.STOCK_CODE && s.MARKET_TYPE == stock.MARKET_TYPE);
            if (existingStock != null)
            {
                throw new Exception("Stock already exists.");
            }
            await _unitOfWork.Repository<StockRow>().Create(stock);
            await _unitOfWork.CompleteAsync(); // 保存变更
        }

        public IQueryable<StockRow> Reads()
        {
            throw new NotImplementedException();
        }

        public async Task Update(StockRow stock)
        {
            
            _unitOfWork.Repository<StockRow>().Update(stock);
            await _unitOfWork.CompleteAsync(); // 保存变更
        }

        public void Delete(StockRow entity)
        {
            throw new NotImplementedException();
        }
    }

}
