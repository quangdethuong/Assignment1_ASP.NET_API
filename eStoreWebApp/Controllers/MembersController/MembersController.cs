using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;
using System.Net;

namespace eStoreWebApp.Controllers.MembersController
{
    public class MembersController : Controller
    {
        private readonly HttpClient client = null;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private string MemberApiUrl = "";

        public MembersController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MemberApiUrl = "https://localhost:5001/api/Member";
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public ISession session { get { return _httpContextAccessor.HttpContext.Session; } }

        public async Task<IActionResult> List()
        {
            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");
            HttpResponseMessage response = await client.GetAsync(MemberApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Member> listMembers = JsonSerializer.Deserialize<List<Member>>(strData, options);
            return View(listMembers);
        }

        public ActionResult Create()
        {
            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Member m)
        {
            string data = JsonSerializer.Serialize(m);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(MemberApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                // Handle other non-success status codes, e.g., Bad Request
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    // Use the errorMessage as needed in your client application
                    ViewBag.Error = errorMessage;
                    // Handle the BadRequest scenario, maybe log or display an error message
                    ModelState.AddModelError(string.Empty, "Please check your input.");
                }

                // Return to the view with validation errors
                return View(m);
            }
         
            //await client.PostAsJsonAsync(MemberApiUrl, m);
            //return Redirect("/Members/List");
        }

        private async Task<Member> GetMemberById(int id)
        {
            HttpResponseMessage response = await client.GetAsync(MemberApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Member>(strData, options);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");
            var member = await GetMemberById(id);
           
            if (member == null)
                return NotFound();
            return View(member);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id, [FromForm] Member member)
        {
            await client.PostAsJsonAsync(MemberApiUrl + "/" + id, member);
            return Redirect("/Members/List");
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");

            var member = await GetMemberById(id);
            if (member == null)
                return NotFound();
            return View(member);
        }

        //private async Task<Member> GetMemberById(int id)
        //{
        //    HttpResponseMessage response = await client.GetAsync(MemberApiUrl + "/" + id);
        //    if (!response.IsSuccessStatusCode)
        //        return null;
        //    string strData = await response.Content.ReadAsStringAsync();

        //    var options = new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    };
        //    return JsonSerializer.Deserialize<Member>(strData, options);
        //}

       

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await GetMemberById(id);
            HttpResponseMessage response = await client.DeleteAsync(MemberApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            return View();
        }

        public async Task<ActionResult> Details(int id)
        {
            if (session.GetString("User") == null) return RedirectToAction("Index", "Home");
            var member = await GetMemberById(id);
            if (member == null)
                return NotFound();
            return View(member);
        }

      
    }
}
