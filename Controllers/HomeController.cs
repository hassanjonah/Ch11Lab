using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ch11Lab.Models;
using System.Text.Json;

namespace Ch11Lab.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {

        string? json = Request?.Cookies["user"];
        if (String.IsNullOrEmpty(json))
            return RedirectToAction("UserCheck");

        User? user = JsonSerializer.Deserialize<User>(json);
        var userTickets = new UserTickets()
        {
            User = user,
            Tickets = Data.GetTickets()
        };

        return View(userTickets);
    }

    [HttpPost]
    public IActionResult Index(UserTickets userTickets)
    {

        foreach (string ticket in userTickets.Tickets.ToArray())
        {
            if (String.IsNullOrEmpty(ticket))
                userTickets.Tickets.Remove(ticket);
        }
        ModelState.Clear();
        return View(userTickets);
    }

    [HttpGet]
    public IActionResult UserCheck()
    {
        return View(new User());
    }

    [HttpPost]
    public IActionResult UserCheck(User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        Response.Cookies.Append("user", JsonSerializer.Serialize(user), new CookieOptions()
        {
            Expires = DateTime.Now.AddMinutes(10)
        });
        return RedirectToAction("Index");

    }

    [HttpGet]
    public IActionResult ClearCookie()
    {
        Response.Cookies.Delete("user");
        return RedirectToAction("Index");
    }


    public JsonResult CheckEmail(string email)
    {
        if (Data.Users.Any(u => email.Equals(u.Email, StringComparison.InvariantCultureIgnoreCase)))
            return Json($"E-Mail Address {email} is not unique");

        return Json(true);
    }
}
