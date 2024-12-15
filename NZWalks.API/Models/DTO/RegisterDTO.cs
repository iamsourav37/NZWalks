using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(3)]
        public string Password { get; set; }

        public string[] Roles { get; set; } = ["Reader"];
    }
}
