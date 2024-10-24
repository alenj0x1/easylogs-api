using System.ComponentModel.DataAnnotations;

namespace easylogsAPI.Models.Requests.Auth;

public class RenewAccessAuthRequest
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}