using System.ComponentModel.DataAnnotations;

namespace easylogsAPI.Models.Requests;

public class UpdateUserRequest
{
    [Required]
    public Guid UserId { get; set; }
    [MaxLength(100)]
    public string? Username { get; set; }
    [EmailAddress]
    [MaxLength(255)]
    public string? Email { get; set; }
    [MaxLength(255)]
    public string? Password { get; set; }
}