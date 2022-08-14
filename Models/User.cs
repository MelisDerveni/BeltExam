using System.ComponentModel.DataAnnotations;
namespace BeltExam.Models;
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations.Schema;


public class User
{
    [Key]
    public int UserId { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "First name must be 2 characters or longer!")]
    public string FirstName { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Last Name must be 2 characters or longer!")]
    public string LastName { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(15)]
    public string UserName { get; set; }
    
    [DataType(DataType.Password)]
    [Required]
    [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Hobby> CreatedHobbies { get; set; } = new List<Hobby>(); 
    public List<Enthusiast> HobbyQePelqej { get; set; } = new List<Enthusiast>(); 
    // public List<Like> Liked { get; set; } = new List<Like>(); 
    // Will not be mapped to your users table!
    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string Confirm { get; set; }
}
public class LoginUser
{
    // No other fields!
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}