using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using BusinessObject;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;
using System;

namespace eStoreWebApp.Controllers.AuthenticationController
{
    public class AuthenticationController : Controller
    {

        private readonly HttpClient client = null;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private string MemberApiUrl = "";
    


        public AuthenticationController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MemberApiUrl = "https://localhost:5001/api/Authen";
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public ISession session { get { return _httpContextAccessor.HttpContext.Session; } }



        public async Task<ActionResult> Login()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        [ValidateAntiForgeryToken]
        public ActionResult LoginUser(Member m, int id)
        {
            var getAdminEmail = _configuration.GetValue<string>("Account:email");
            var getAdminPassword = _configuration.GetValue<string>("Account:password");
            if (getAdminEmail.Equals(m.Email) && getAdminPassword.Equals(m.Password))
            {
                session.SetString("Role", "Admin");
                session.SetString("User", m.Email);
                return RedirectToAction("Index", "Home");
            }

            string data = JsonSerializer.Serialize(m);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response =  client.PostAsync(MemberApiUrl + "/Login", content).Result;

            if (response.IsSuccessStatusCode)
            {
                session.SetString("Role", "User");
                session.SetString("User", m.Email);
                return RedirectToAction("Index", "Home");
            }
            if (m.Password != null || m.Email != null)
            {
                ViewBag.error = "Incorrect username or password. Try again!!!";
            }
            return View();
        }


        public ActionResult SignUp()
        {
           // if (session.GetString("User") != null) return RedirectToAction("Index", "Home");

            return View();
        }

        //[HttpPost, ActionName("SignUp")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SignUp(Member m)
        //{
        //    string data = JsonSerializer.Serialize(m);
        //    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
        //    HttpResponseMessage response =  client.PostAsync(MemberApiUrl + "/SignUp", content).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("List");
        //    }
        //    return View();

        //}

        [HttpPost, ActionName("SignUp")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(Member m)
        {
            try
            {
                // Serialize the Member object to JSON
                string data = JsonSerializer.Serialize(m);

                // Create a StringContent object with JSON data and content type
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                // Send the POST request to the API
                HttpResponseMessage response = await client.PostAsync(MemberApiUrl + "/SignUp", content);

                // Check if the response indicates success
                if (response.IsSuccessStatusCode)
                {
                    session.SetString("Role", "User");
                    session.SetString("User", m.Email);
                    // Redirect to the "List" action
                    return RedirectToAction("Index", "Home");
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
            }
            catch (Exception ex)
            {
                // Handle any unexpected exceptions that might occur during the request
                ModelState.AddModelError(string.Empty, "An error occurred: " + ex.Message);

                // Return to the view with an error message
                return View(m);
            }
        }


        public ActionResult Logout()
        {
            session.Remove("Role");
            session.Remove("User");
            return RedirectToAction("Index", "Home");
        }
        //// GET: AuthenticationController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: AuthenticationController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: AuthenticationController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AuthenticationController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: AuthenticationController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: AuthenticationController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: AuthenticationController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: AuthenticationController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
