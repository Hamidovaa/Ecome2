﻿using Ecome2.DAL;
using Ecome2.EXtentions;
using Ecome2.Models;
using Ecome2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecome2.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<ProgramUser> _userManager;
        private readonly SignInManager<ProgramUser> _signInManager;
        private readonly IEmailService _emailService;
        public AccountController(AppDbContext _appDbContext, UserManager<ProgramUser> userManager, SignInManager<ProgramUser> signInManager, IEmailService emailService)
        {
            appDbContext = _appDbContext;
            _userManager = userManager;
            _signInManager =signInManager;
            _emailService=emailService;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterVM model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            ProgramUser programUser = new ProgramUser
            {
                Email = model.Email,
                UserName=model.Email
            };
            var result=await _userManager.CreateAsync(programUser, model.Password);
            if(result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(programUser);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = programUser.Id, token = token }, Request.Scheme);
                await _emailService.SendEmailAsync(model.Email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

                return RedirectToAction("Index", "Home");
            }
            foreach(var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            return View();
            
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Login(LoginVM model)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong");  
            }
            var user =await _userManager.FindByEmailAsync(model.Email);
            if (user!=null)
            { 
               var result= await _signInManager.PasswordSignInAsync(user, model.Password, model.IsRemember, false );
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Password or email incorrect");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}