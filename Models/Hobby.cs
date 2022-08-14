using System.ComponentModel.DataAnnotations;
namespace BeltExam.Models;
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations.Schema;

public class Hobby
{
    [Key]
    public int HobbyId { get; set; }
    [Required]

    public string HobbyName { get; set; }
    
    [Required]
    public string HobbyDescription { get; set; }


    // One to Many Connection in DB
    
    [Required]
    public int UserId { get; set; }
    // Navigation property for related User object
    public User? Creator { get; set; }

    // Many to Many connection in DB    

    public List<Enthusiast> Enthusiasts { get; set; } = new List<Enthusiast>(); 

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;


}