using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BeltExam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeltExam.Controllers;

public class HomeController : Controller
{
    public MyContext _context;
    public HomeController(MyContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        if(HttpContext.Session.GetInt32("userId") != null)
        {
            return RedirectToAction("Dashbord");
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost("Register")]
    public IActionResult Register(User FromView)
    {
        
        if(ModelState.IsValid)
        {
            if (_context.Users.Any(u => u.UserName == FromView.UserName))
            {
                // Manually add a ModelState error to the Email field, with provided
                // error message
                ModelState.AddModelError("UserName", "UserName already in use!");

                return View("Index");
                // You may consider returning to the View at this point
            }
        PasswordHasher<User> Hasher = new PasswordHasher<User>();
        FromView.Password = Hasher.HashPassword(FromView, FromView.Password);
        _context.Users.Add(FromView);
        _context.SaveChanges();
        HttpContext.Session.SetInt32("userId", FromView.UserId);
        return RedirectToAction("Dashbord");
        }
        else
        {
            return View("Index");
        }
    }
    [HttpPost("Login")]
    public IActionResult Login(LoginUser FromView)
    {
        if(ModelState.IsValid)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.UserName == FromView.UserName);
            if (userInDb == null)
            {
                // Add an error to ModelState and return to View!
                ModelState.AddModelError("UserName", "Invalid UserName/Password");
                return View("Index"); 
            }
            var hasher = new PasswordHasher<LoginUser>();

            // verify provided password against hash stored in db
            var result = hasher.VerifyHashedPassword(FromView, userInDb.Password, FromView.Password);

            // result can be compared to 0 for failure
            if (result == 0)
            {
                ModelState.AddModelError("Password", "Invalid Password");
                return View("Index");
                // handle failure (this should be similar to how "existing email" is handled)
            }
            HttpContext.Session.SetInt32("userId", userInDb.UserId);

            return RedirectToAction("Dashbord");
        }
        return View("Privacy");
    }
    [HttpGet("Logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return View("Index");
    }
    [HttpGet("Dashbord")]
    public IActionResult Dashbord()
    {
        ViewBag.Iloguari = _context.Users.First(c=>c.UserId ==(int)HttpContext.Session.GetInt32("userId") );
        ViewBag.Novice = _context.Enthusiasts.Include( e=> e.HobbyQePelqehet).ThenInclude(e =>e.Enthusiasts).Where(e => e.Type == "Novice").OrderBy(e=>e.HobbyQePelqehet.Enthusiasts.Count).ToList();
        ViewBag.Intermediate = _context.Enthusiasts.Include( e=> e.HobbyQePelqehet).ThenInclude(e =>e.Enthusiasts).Where(e => e.Type == "Intermediate").OrderBy(e=>e.HobbyQePelqehet.Enthusiasts.Count).ToList();
        ViewBag.Expert = _context.Enthusiasts.Include( e=> e.HobbyQePelqehet).ThenInclude(e =>e.Enthusiasts).Where(e => e.Type == "Expert").OrderBy(e=>e.HobbyQePelqehet.Enthusiasts.Count).ToList();
        // ViewBag.ThisHobby =  _context.Hobbies.Include(e => e.Creator).Include(e => e.Enthusiasts).ThenInclude(e=> e.UseriQePelqen).First(e => e.HobbyId== id);
        
        ViewBag.Hobbies = _context.Hobbies.Include(e => e.Enthusiasts).OrderBy( e => e.Enthusiasts.Count());
        

        ViewBag.AllHobbies = _context.Hobbies.Include(c=>c.Enthusiasts).ThenInclude(c=>c.UseriQePelqen);
        return View();
    }


    // Part 2

    // if( HttpContext.Session.GetInt32("userId") == null)
    //     {
    //         return RedirectToAction("Index");
    //     }

    [HttpGet("AddHobby")]
    public IActionResult AddHobby()
    {
        return View();
    }
    [HttpPost("AddThisHobby")]
    public IActionResult AddThisHobby(Hobby FromView)
    {
        if(ModelState.IsValid)
        {
            if (_context.Hobbies.Any(u => u.HobbyName == FromView.HobbyName))
            {
                // Manually add a ModelState error to the Email field, with provided
                // error message
                ModelState.AddModelError("HobbyName", "HobbyName Must Be Unique!");

                return View("AddHobby");
                // You may consider returning to the View at this point
            }
            FromView.UserId =  (int)HttpContext.Session.GetInt32("userId");
            _context.Hobbies.Add(FromView);
            _context.SaveChanges();
            return RedirectToAction("Dashbord");
        }
        return View("AddHobby");
    }
    [HttpGet("Hobby/{id}")]
    public IActionResult HobbyId(int id)
    {
        // ViewBag.Enthusiasts = _context.Enthusiasts
        ViewBag.Iloguari = _context.Users.First(c=>c.UserId ==(int)HttpContext.Session.GetInt32("userId") );
        ViewBag.Intermediate = _context.Enthusiasts.Include( e=> e.HobbyQePelqehet).ThenInclude(e =>e.Enthusiasts).Where(e => e.Type == "Intermediate").OrderBy(e=>e.HobbyQePelqehet.Enthusiasts.Count).ToList();
        ViewBag.Expert = _context.Enthusiasts.Include( e=> e.HobbyQePelqehet).ThenInclude(e =>e.Enthusiasts).Where(e => e.Type == "Expert").OrderBy(e=>e.HobbyQePelqehet.Enthusiasts.Count).ToList();
        ViewBag.ThisHobby =  _context.Hobbies.Include(e => e.Creator).Include(e => e.Enthusiasts).ThenInclude(e=> e.UseriQePelqen).First(e => e.HobbyId== id);
        
        ViewBag.Hobbies = _context.Hobbies.Include(e=>e.Enthusiasts).ThenInclude(e=>e.Type).OrderBy(e=>e.Enthusiasts.Count());
    
        return View();
    }
    [HttpPost("Hobby/Enthusiast/{id}")]
    public IActionResult BehuFans(int id, string type)
    {
        int idFromSession = (int)HttpContext.Session.GetInt32("userId");
        
        
        Enthusiast fansIRI = new Enthusiast()
        {
            UserId = idFromSession,
            HobbyId = id,
            Type = type
        };
        _context.Enthusiasts.Add(fansIRI);
        _context.SaveChanges();
        return RedirectToAction("Dashbord");




    }
    [HttpGet("Hobby/EditHobby/{id}")]
    public IActionResult EditHobby(int id)
    {
        ViewBag.Hobby = _context.Hobbies.First(c=>c.HobbyId == id);
        return View();
    }


    [HttpPost("Hobby/EditHobby/UpdateThisHobby/{id}")]
    public IActionResult UpdateHobby(Hobby FromView ,int id)
    {
        Hobby UpdateHobby = _context.Hobbies.First(e => e.HobbyId == id);
        int UpdateHobbyId = _context.Hobbies.First(e => e.HobbyId == id).HobbyId;
        if (ModelState.IsValid)
        {
        if (_context.Hobbies.Any(u => u.HobbyName == FromView.HobbyName))
            {
                // Manually add a ModelState error to the Email field, with provided
                // error message
                ModelState.AddModelError("HobbyName", "This Hobby already exists");

                return View("Dashbord");
                // You may consider returning to the View at this point
            }
        UpdateHobby.HobbyName = FromView.HobbyName;
        UpdateHobby.HobbyDescription = FromView.HobbyDescription;
        _context.SaveChanges();
        return RedirectToAction("Dashbord");
    }
    return View("Dashbord");
}


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
