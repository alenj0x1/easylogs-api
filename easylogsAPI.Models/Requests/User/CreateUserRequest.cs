using System.ComponentModel.DataAnnotations;

namespace easylogsAPI.Models.Requests.User;

public class CreateUserRequest
{
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = null!;
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = null!;
    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = null!;
    [Required]
    public List<int> Permissions { get; set; } = [];
}