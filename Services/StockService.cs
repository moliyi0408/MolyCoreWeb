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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace MolyCoreWeb.Services
{
    public class StockService : IStockService
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDownloadService _downloadService;

        public StockService( IUnitOfWork unitOfWork, IDownloadService downloadService)
        {
            _unitOfWork = unitOfWork;
            _downloadService = downloadService;
        }

        //獲得股票清單
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

        //股票清單更新
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

            List<StockRow> stockRows = await DownloadStockListDataAsync(url);

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

        //下載股票清單
        private static async Task<List<StockRow>> DownloadStockListDataAsync(string url)
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

        // 獲取經濟指標
        public async Task<IEnumerable<BusinessIndicator>> GetBusinessIndicatorsAsync()
        {
            try
            {        
                //var businessIndicators = await _unitOfWork.Repository<BusinessIndicator>().GetAllAsync();
                string sqlQuery = string.Format(@" 
                        SELECT *
                        FROM BusinessIndicators
                        WHERE Date >= DATE('now', '-1 year')
                                                  "
                       );
               var businessIndicator = await _unitOfWork.Repository<BusinessIndicator>().ExecuteSqlQueryAsync(sqlQuery);

                return businessIndicator;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving business indicators: {ex.Message}");
                throw;
            }
        }

        // 更新經濟指標
        public async Task GetBusinessIndicatorsUpdateAsync()
        {
            var url = "https://ws.ndc.gov.tw/Download.ashx?u=LzAwMS9hZG1pbmlzdHJhdG9yLzEwL3JlbGZpbGUvNTc4MS82MzkyL2FmYWU2OGQ1LWVjNzktNDg5NC04ODFjLTI0M2E1Nzg2ODBlZC54bHN4&n=5paw6IGe56i%2f6ZmE5Lu25pW45YiXLnhsc3g%3d&icon=.xlsx";

            List<BusinessIndicator> businessIndicators = await DownloadBusinessIndicatorsFromExcel(url);

            if (businessIndicators != null && businessIndicators.Count > 0)
            {
                foreach (var bs in businessIndicators)
                {
                    // Find existing indicator based on Date (example condition)
                    var existingIndicator = await _unitOfWork.Repository<BusinessIndicator>()
                                                          .GetByCondition(x => x.Date == bs.Date);

                    if (existingIndicator != null)
                    {
                        // Update properties of existingIndicator with data from bs
                        existingIndicator.LEI_CCI = bs.LEI_CCI;
                        existingIndicator.LEI_Ex_Trend = bs.LEI_Ex_Trend;
                        existingIndicator.CEI_CCI = bs.CEI_CCI;
                        existingIndicator.CEI_Ex_Trend = bs.CEI_Ex_Trend;
                        existingIndicator.LAG_CCI = bs.LAG_CCI;
                        existingIndicator.LAG_Ex_Trend = bs.LAG_Ex_Trend;
                        existingIndicator.BCS_Composite_Score = bs.BCS_Composite_Score;
                        existingIndicator.BCS_Signal = bs.BCS_Signal;

                        // Mark existingIndicator for update
                        _unitOfWork.Repository<BusinessIndicator>().Update(existingIndicator);
                    }
                    else
                    {
                        await _unitOfWork.Repository<BusinessIndicator>().Create(bs);

                    }
                }

                // Save changes to the database
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                Console.WriteLine("No data extracted.");
            }
        }

        // 下載經濟指標Excel
        private async Task<List<BusinessIndicator>> DownloadBusinessIndicatorsFromExcel(string url)
        {
            List<BusinessIndicator> businessIndicators = new List<BusinessIndicator>();

            try
            {
                var outputPath = Path.Combine(Path.GetTempPath(), "downloaded_file.xlsx");

                // Download Excel and process
                await _downloadService.DownloadFileAsync(url, outputPath);

                // Handle reading from Excel file
                using (var stream = new FileStream(outputPath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new XSSFWorkbook(stream);
                    ISheet sheet = workbook.GetSheetAt(0); // Assuming we read the first sheet

                    for (int row = 1; row <= sheet.LastRowNum; row++) // Start from row 1 to skip headers
                    {
                        IRow rowData = sheet.GetRow(row);
                        if (rowData != null)
                        {
                            var indicator = new BusinessIndicator
                            {
                                Date = DateTime.TryParseExact(rowData.GetCell(0)?.ToString(),
                                          "dd-M月-yyyy", // Specify the exact format of your date
                                          CultureInfo.InvariantCulture,
                                          DateTimeStyles.None,
                                          out DateTime parsedDate)
                               ? parsedDate
                               : DateTime.MinValue,
                                LEI_CCI = double.TryParse(rowData.GetCell(1)?.ToString(), out double leiCci) ? leiCci : 0.0,
                                LEI_Ex_Trend = double.TryParse(rowData.GetCell(2)?.ToString(), out double leiExTrend) ? leiExTrend : 0.0,
                                CEI_CCI = double.TryParse(rowData.GetCell(3)?.ToString(), out double ceiCci) ? ceiCci : 0.0,
                                CEI_Ex_Trend = double.TryParse(rowData.GetCell(4)?.ToString(), out double ceiExTrend) ? ceiExTrend : 0.0,
                                LAG_CCI = double.TryParse(rowData.GetCell(5)?.ToString(), out double lagCci) ? lagCci : 0.0,
                                LAG_Ex_Trend = double.TryParse(rowData.GetCell(6)?.ToString(), out double lagExTrend) ? lagExTrend : 0.0,
                                BCS_Composite_Score = double.TryParse(rowData.GetCell(7)?.ToString(), out double bcsCompositeScore) ? bcsCompositeScore : 0.0,
                                BCS_Signal = rowData.GetCell(8)?.ToString()
                            };
                            businessIndicators.Add(indicator);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading Excel file: {ex.Message}");
            }

            return businessIndicators;
        }

        public async Task<StockInfo> GetStockInfoAsync(string stockCode)
        {
            var query = _unitOfWork.Repository<StockInfo>().GetByIdAsync(int.Parse(stockCode));
            return await query;
        }

        // IService實作 CRUD
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
