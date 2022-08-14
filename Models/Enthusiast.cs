using System.ComponentModel.DataAnnotations;
namespace BeltExam.Models;
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations.Schema;


public class Enthusiast{
    [Key]
    public int EnthusiastId {get;set;}
    public int UserId {get;set;}
    public int HobbyId { get; set; }
    public string Type{get;set;}
    public User? UseriQePelqen {get;set;}
    public Hobby? HobbyQePelqehet {get;set;}
    
}