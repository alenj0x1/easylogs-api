using System.ComponentModel.DataAnnotations;

namespace easylogsAPI.Models.Requests.Auth;

public class LoginAuthRequest
{
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = null!;
    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = null!;
}