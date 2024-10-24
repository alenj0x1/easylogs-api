using System.ComponentModel.DataAnnotations;

namespace easylogsAPI.Models.Requests.User;

public class UpdateUserRequest
{
    public string? Username { get; set; }
    [EmailAddress]
    [MaxLength(255)]
    public string? Email { get; set; }
    [MaxLength(255)]
    public string? Password { get; set; }
}