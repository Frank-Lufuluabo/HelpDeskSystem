﻿using HelpDeskSystem.ClaimsManagement;
using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using HelpDeskSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Permissions;

namespace HelpDeskSystem.Controllers
{

    [Authorize]
    public class RolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> rolemanager, SignInManager<ApplicationUser> signInManager)
        {
            _rolemanager = rolemanager;

            _signInManager = signInManager;

            _userManager = userManager;

            _context = context;
        }

        [Permission("ROLES:VIEW")]
        public async Task<ActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }



        [Permission("ROLES:CREATE")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            return View();
        }


        [Permission("ROLES:CREATE")]
        [HttpPost]
        public async Task<ActionResult> Create(RolesViewModel vm)
        {
            IdentityRole role = new();
            role.Name = vm.RoleName;

            var result = await _rolemanager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(vm);
            }
        }
    }
}
