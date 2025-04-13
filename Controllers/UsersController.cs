using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HelpDeskSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public string ClaimType { get; private set; }

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;

        }

        // GET: UsersController
        public async Task<ActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(ApplicationUser user)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                ApplicationUser registereduser = new();
                registereduser.FirstName = user.FirstName;
                registereduser.UserName = user.UserName;
                registereduser.MiddleName = user.MiddleName;
                registereduser.LastName = user.LastName;
                registereduser.NormalizedUserName = user.UserName;
                registereduser.Email = user.Email;
                registereduser.EmailConfirmed = true;
                registereduser.City = user.City;
                registereduser.Country = user.Country;
                registereduser.PhoneNumber = user.PhoneNumber;

                var result = await _userManager.CreateAsync(registereduser, user.PasswordHash);
                if (result.Succeeded)
                {
                    //Log the Audit Trail 
                    var activity = new AuditTrail
                    {
                        Action = "Create",
                        TimeStamp = DateTime.Now,
                        IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        UserId = userId,
                        Module = "Users",
                        AffectedTable = "Users"
                    };

                    _context.Add(activity);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }


            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: UsersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
