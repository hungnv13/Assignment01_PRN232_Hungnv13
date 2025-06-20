using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace eStoreClient.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient client = null;
        private string OrderApiUrl = "";

        public OrderController()
        {
            client = new HttpClient();
            OrderApiUrl = "https://localhost:7115/api/OrderAPI";
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<List<Order>>(strData);
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{OrderApiUrl}/{id}");
            string strData = await response.Content.ReadAsStringAsync();
            var order = JsonConvert.DeserializeObject<Order>(strData);
            return View(order);
        }
    }
}
