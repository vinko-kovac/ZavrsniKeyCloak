using System.ComponentModel.DataAnnotations;

namespace WebApp
{
    public class User
    {
        [Required]
        public string Username { get; set; }
        //public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Button { get; set; }
    }
}
