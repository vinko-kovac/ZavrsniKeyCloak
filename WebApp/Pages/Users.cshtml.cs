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
    public class UsersModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public List<InputUser> Users { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Users = new List<InputUser> { new InputUser() };
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("accept", "application/json");
                /*var cookies = HttpContext.Request.Cookies;
                var cookie = string.Empty;
                foreach (var key in cookies.Keys)
                {
                    cookie += key + "=" + cookies[key] + ";";
                }
                client.DefaultRequestHeaders.Add("cookie", cookie);*/

                var tok = await HttpContext.GetTokenAsync("id-token");

                var data = "username="+"&password=admin&client_id=keycloak-zavrsni&grant_type=password";
                client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded"); 
                //var res = await client.PostAsync("http://localhost:8080/realms/master/protocol/openid-connect/token?"+data);;
                //Debug.WriteLine(res);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tok);
                Debug.WriteLine(client.DefaultRequestHeaders.ToString());
                var response = await client.GetAsync("http://localhost:8080/auth/admin/realm/master/users");
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Debug.WriteLine(response.ToString());
                    return Redirect("/Index");
                }
                var content = await response.Content.ReadAsStringAsync();
                Users = JsonConvert.DeserializeObject<List<InputUser>>(content);
                return Page();
            }
        }
    }
}
