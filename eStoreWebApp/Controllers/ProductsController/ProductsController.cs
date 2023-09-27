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
using System.Net.Http.Json;
using System.Text;

namespace eStoreWebApp.Controllers.ProductsController

{
    public class ProductsController : Controller
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";
        private string CategoryApiUrl = "";

        private readonly IHttpContextAccessor _httpContextAccessor;


        public ProductsController(IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:5001/api/Product";
            CategoryApiUrl = "https://localhost:5001/api/Category";
            _httpContextAccessor = httpContextAccessor;

        }

        public ISession session { get { return _httpContextAccessor.HttpContext.Session; } }
        // GET: ProductsController
       

        public async Task<IActionResult> List(string searchString)
        {

            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");
            HttpResponseMessage response;
            if (string.IsNullOrEmpty(searchString))
            {
                response = await client.GetAsync(ProductApiUrl);
            }
            else
            {
                response = await client.GetAsync(ProductApiUrl + "/Search?search=" + searchString.ToLower());
            }
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Product> listProducts = JsonSerializer.Deserialize<List<Product>>(strData, options);

            return View(listProducts);
        }



        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Product p)
        {

            await client.PostAsJsonAsync(ProductApiUrl, p);
            return Redirect("/Products/Index");
        }


        // GET: ProductsController/Create

        public async Task<IActionResult> Create()
        {
            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");

            // Category
            HttpResponseMessage response = await client.GetAsync(CategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Category> listCategories = JsonSerializer.Deserialize<List<Category>>(strData, options);
            ViewBag.listCategories = listCategories;
            return View();
        }

        private async Task<Product> GetProductById(int id)
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Product>(strData, options);
        }

        // GET: ProductsController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");

            var p = await GetProductById(id);
            if (p == null)
                return NotFound();
            return View(p);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");
            var product = await GetProductById(id);
            HttpResponseMessage response = await client.GetAsync(CategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Category> listCategories = JsonSerializer.Deserialize<List<Category>>(strData, options);
            ViewBag.listCategories = listCategories;
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost]
       
        public async Task<ActionResult> Edit(int id, [FromForm] Product product)
        {
            await client.PostAsJsonAsync(ProductApiUrl + "/" + id, product);
            return Redirect("/Products/List");
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");
            var product = await GetProductById(id);
            if (product == null)
                return NotFound();
            await SetViewData();
            return View(product);
        }

        public async Task SetViewData()
        {
            var listCategory = await GetCategory();
            ViewData["Categories"] = new SelectList(listCategory, "CategoryId", "CategoryName");
        }

       

        public async Task<IEnumerable<Category>> GetCategory()
        {
            HttpResponseMessage response = await client.GetAsync(CategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Category> listCategory = JsonSerializer.Deserialize<List<Category>>(strData, options);
            return listCategory;
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await GetProductById(id);

            HttpResponseMessage response = await client.DeleteAsync(ProductApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            return View();
        }
    }
}
