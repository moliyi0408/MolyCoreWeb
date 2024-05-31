using Microsoft.AspNetCore.Mvc;
using MolyCoreWeb.Services;

namespace MolyCoreWeb.Controllers
{
    public class StockController : Controller
    {
        private readonly ILineNotifyService _lineNotifyService;

        public IActionResult Index()
        {
            return View();
        }

        public StockController(ILineNotifyService lineNotifyService)
        {
            _lineNotifyService = lineNotifyService;
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("Message", "Message cannot be empty.");
                return View("Index");
            }

            await _lineNotifyService.SendMessageAsync(message);
            ViewBag.NotificationResult = "Notification sent successfully!";
            return View("Index");
        }
    

        [HttpPost]
        public IActionResult CalculateReasonablePrice(decimal dividend)
        {
            if (dividend <= 0)
            {
                return BadRequest("預估全年總配息必須大於 0");
            }

            decimal Dividendprice5 = dividend / 0.05m;
            decimal Dividendprice6 = dividend / 0.06m;
            decimal Dividendprice7 = dividend / 0.07m;

            var result = new
            {
                Dividendprice7,
                Dividendprice6,
                Dividendprice5
            };

            return Ok(result);
        }

        [HttpPost]
        public IActionResult TaxCount(decimal buyAmount, decimal sellAmount, decimal discount)
        {
            if (buyAmount <= 0 || sellAmount <= 0 || discount <= 0)
            {
                return BadRequest("所有輸入值必須大於 0");
            }

            decimal tax = sellAmount * 0.003m;
            decimal buyFee = buyAmount * 0.001425m * discount;
            decimal buyTotal = buyAmount + buyFee;
            decimal fee = sellAmount * 0.001425m * discount;
            decimal sellFee = tax + fee;
            decimal sellTotal = sellAmount - sellFee;
            decimal profit = sellTotal - buyTotal;

            var result = new
            {
                BuyTotal = buyTotal,
                Tax = tax,
                Fee = fee,
                SellFee = sellFee,
                SellTotal = sellTotal,
                Profit = profit
            };

            return Ok(result);
        }

        [HttpPost]
        public IActionResult CalculatePriceBookRatio(decimal price, decimal bookValue)
        {
            if (price <= 0 || bookValue <= 0)
            {
                return BadRequest("股價和每股淨值必須大於 0");
            }

            decimal priceBookRatio = price / bookValue;

            return Ok(new { PriceBookRatio = priceBookRatio });
        }

        [HttpPost]
        public IActionResult Calculate(int count, int choice)
        {
            if (count <= 0)
            {
                return BadRequest("計算次數必須大於 0");
            }

            var results = new List<object>();

            for (int i = 0; i < count; i++)
            {
                switch (choice)
                {
                    case 1:
                        // You can add logic to call CalculateReasonablePrice here
                        results.Add("功能1計算結果");
                        break;
                    case 2:
                        // You can add logic to call TaxCount here
                        results.Add("功能2計算結果");
                        break;
                    case 3:
                        // You can add logic to call CalculatePriceBookRatio here
                        results.Add("功能3計算結果");
                        break;
                    default:
                        return BadRequest("輸入錯誤，請重新輸入。");
                }
            }

            return Ok(results);
        }
    }
}
