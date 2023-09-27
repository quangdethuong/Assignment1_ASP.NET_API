using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace eStoreWebApp.Controllers.OrderController
{
    public class OrdersController : Controller
    {
        private readonly HttpClient client = null;
        private string OrderApiUrl = "";
        private string MemberApiUrl = "";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrdersController(IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApiUrl = "https://localhost:5001/api/Order";
            MemberApiUrl = "https://localhost:5001/api/Member";
            _httpContextAccessor = httpContextAccessor;
        }

        public ISession session { get { return _httpContextAccessor.HttpContext.Session; } }

        public async Task<IActionResult> List()
        {
            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Order> listOrders = JsonSerializer.Deserialize<List<Order>>(strData, options);
            return View(listOrders);
        }

        public async Task<ActionResult> Create()
        {
            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");
            HttpResponseMessage responeCategory = await client.GetAsync(MemberApiUrl);
            string strData = await responeCategory.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Member> listMember = JsonSerializer.Deserialize<List<Member>>(strData, options);
            ViewData["Members"] = new SelectList(listMember, "MemberId", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order o)
        {
            string data = JsonSerializer.Serialize(o);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(OrderApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            HttpResponseMessage responeMember = await client.GetAsync(MemberApiUrl);
            string strData = await responeMember.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Member> listMember = JsonSerializer.Deserialize<List<Member>>(strData, options);
            ViewData["Categories"] = new SelectList(listMember, "MemberId", "Email");
            return View();
        }

        public async Task<ActionResult> Details(int id)
        {
            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");
            var product = await GetOrderById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        private async Task<Order> GetOrderById(int id)
        {
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Order>(strData, options);
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");
            var order = await GetOrderById(id);
            if (order == null)
                return NotFound();
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await GetOrderById(id);

            HttpResponseMessage response = await client.DeleteAsync(OrderApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Order order = JsonSerializer.Deserialize<Order>(strData, options);

            HttpResponseMessage responeCategory = await client.GetAsync(MemberApiUrl);
            string str = await responeCategory.Content.ReadAsStringAsync();

            List<Member> listMember = JsonSerializer.Deserialize<List<Member>>(str, options);
            ViewData["Members"] = new SelectList(listMember, "MemberId", "Email");
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Order order)
        {
            string data = JsonSerializer.Serialize(order);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(OrderApiUrl + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            HttpResponseMessage responeMember = await client.GetAsync(MemberApiUrl);
            string strData = await responeMember.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Member> listMember = JsonSerializer.Deserialize<List<Member>>(strData, options);
            ViewData["Members"] = new SelectList(listMember, "MemberId", "Email");
            return View();
        }
    }
}
