using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace WebApp.Pages
{
    public class RegisterModel : PageModel
    {

        [BindProperty]
        public User Input { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Input = new User { };
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            Console.WriteLine();
            Console.WriteLine("usao");
            Console.WriteLine();
            if (Input.Button != "register")
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    var token = await HttpContext.GetTokenAsync("access-token");
                    var data = new Dictionary<string, object>();
                    data.Add("username", Input.Username);
                    data.Add("firstName", Input.FirstName);
                    data.Add("lastName", Input.LastName);
                    data.Add("email", Input.Email);
                    data.Add("enabled", "true");
                    data.Add("emailVerified", "");

                    var credentials = new Dictionary<string, object>();
                    credentials.Add("type", "password");
                    credentials.Add("value", Input.Password);
                    credentials.Add("temporary", "false");
                    var credArray = new List<Dictionary<string, object>>();
                    var jsonArray = JsonConvert.SerializeObject(credArray);
                    data.Add("credentials", jsonArray);

                    var json = JsonConvert.SerializeObject(data);

                    var rep = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Add("accept", "application/json");

                    /*var cookies = HttpContext.Request.Cookies;
                    var cookie = string.Empty;
                    foreach (var key in cookies.Keys)
                    {
                        cookie += key + "=" + cookies[key] + ";";
                    }
                    client.DefaultRequestHeaders.Add("cookie", cookie);*/

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = await client.PostAsync("http://localhost:8080/realms/myrealm/users", rep);

                    if (response.StatusCode != HttpStatusCode.Created)
                    {
                        Debug.WriteLine(response.StatusCode.ToString());
                        Debug.WriteLine(response.ToString());
                        return RedirectToAction("Register");
                    }
                    return RedirectToAction("Index");
                   
                }
            } else
            {
                throw new Exception("Model not valid.");
                return RedirectToAction("Register");
            }
        }


    }
}
