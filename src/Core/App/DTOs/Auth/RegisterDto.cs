using System.ComponentModel.DataAnnotations;
using Core.DTOs;

namespace Core.App.DTOs.Auth;

public class RegisterDto
{
    public string Username { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [StringLength(8, MinimumLength = 4)]
    public string Password { get; set; }
    
    
    [Required]
    public UserProfileDto Profile { get; set; }
}