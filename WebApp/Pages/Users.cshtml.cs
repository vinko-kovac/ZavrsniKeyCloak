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


                var tok = await HttpContext.GetTokenAsync("access_token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tok);
                var response = await client.GetAsync("http://localhost:8080/auth/admin/realms/master/users");
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
