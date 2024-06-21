﻿using Microsoft.AspNetCore.Mvc;
using MolyCoreWeb.Models.DTOs;
using MolyCoreWeb.Services;

namespace MolyCoreWeb.Controllers
{
    public class StockController : Controller
    {
        private readonly ILineNotifyService _lineNotifyService;
        private readonly IStockService _stockService;


        public StockController(ILineNotifyService lineNotifyService, IStockService stockService)
        {
            _lineNotifyService = lineNotifyService;
            _stockService = stockService;

        }

        public IActionResult Index()
        {
            return View();
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

        [HttpPost("GetList")]
        public async Task<IActionResult> GetList([FromBody] StockGetListIn inModel)
        {

            StockGetListOut result = await _stockService.GetStockListAsync(inModel);
            if (result == null || result.GridList == null || result.GridList.Count == 0)
            {
                return NotFound(result.ErrMsg);
            }

            return Json(result);
        }

        [HttpPost("UpdateData")]
        public async Task<IActionResult> UpdateData([FromBody] StockGetListIn inModel)
        {
            Console.WriteLine("開始更新股票數據...");

            try
            {
                await _stockService.GetStockListUpdateAsync(inModel);
                return Ok(); // 成功處理請求
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); 
            }
        }


        //[HttpGet("GetRealtimePrice")]
        //public async Task<StockGetRealtimePriceOut> GetRealtimePrice([FromQuery] StockGetRealtimePriceIn inModel)
        //{
        //    return await _stockService.GetRealtimePriceAsync(inModel);
        //}

        //[HttpGet("GetDayPrice")]
        //public async Task<StockGetDayPriceOut> GetDayPrice([FromQuery] StockGetDayPriceIn inModel)
        //{
        //    return await _stockService.GetDayPriceAsync(inModel);
        //}

        //[HttpGet("GetMonthPrice")]
        //public async Task<StockGetMonthPriceOut> GetMonthPrice([FromQuery] StockGetMonthPriceIn inModel)
        //{
        //    return await _stockService.GetMonthPriceAsync(inModel);
        //}
        ////每分鐘 股票股價 現在損益 幾%
        ///

        //[HttpPost]
        //public IActionResult CalculateReasonablePrice(decimal dividend)
        //{
        //    if (dividend <= 0)
        //    {
        //        return BadRequest("預估全年總配息必須大於 0");
        //    }

        //    decimal Dividendprice5 = dividend / 0.05m;
        //    decimal Dividendprice6 = dividend / 0.06m;
        //    decimal Dividendprice7 = dividend / 0.07m;

        //    var result = new
        //    {
        //        Dividendprice7,
        //        Dividendprice6,
        //        Dividendprice5
        //    };

        //    return Ok(result);
        //}

        //[HttpPost]
        //public IActionResult TaxCount(decimal buyAmount, decimal sellAmount, decimal discount)
        //{
        //    if (buyAmount <= 0 || sellAmount <= 0 || discount <= 0)
        //    {
        //        return BadRequest("所有輸入值必須大於 0");
        //    }

        //    decimal tax = sellAmount * 0.003m;
        //    decimal buyFee = buyAmount * 0.001425m * discount;
        //    decimal buyTotal = buyAmount + buyFee;
        //    decimal fee = sellAmount * 0.001425m * discount;
        //    decimal sellFee = tax + fee;
        //    decimal sellTotal = sellAmount - sellFee;
        //    decimal profit = sellTotal - buyTotal;

        //    var result = new
        //    {
        //        BuyTotal = buyTotal,
        //        Tax = tax,
        //        Fee = fee,
        //        SellFee = sellFee,
        //        SellTotal = sellTotal,
        //        Profit = profit
        //    };

        //    return Ok(result);
        //}

        //[HttpPost]
        //public IActionResult CalculatePriceBookRatio(decimal price, decimal bookValue)
        //{
        //    if (price <= 0 || bookValue <= 0)
        //    {
        //        return BadRequest("股價和每股淨值必須大於 0");
        //    }

        //    decimal priceBookRatio = price / bookValue;

        //    return Ok(new { PriceBookRatio = priceBookRatio });
        //}

    }
}
