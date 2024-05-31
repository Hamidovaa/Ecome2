using Ecome2.DAL;
using Ecome2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit;

namespace Ecome2.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ProgramUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext appDbContext;
        public AdminController(
            UserManager<ProgramUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext _appDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            appDbContext=_appDbContext;
        }

        // Admin panelinde bir kullanıcıya admin rolü eklemek için bir eylem
        [HttpPost]
        public async Task<IActionResult> AddToAdminRole(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Email != "hemidoffa55@gmail.com")
            {
                return Forbid();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Eğer admin rolü yoksa, rolü ekleyin
            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            return RedirectToAction("Index", new { id = userId });
        }

        // Admin panelinde bir kullanıcıdan admin rolünü kaldırmak için bir eylem
        [HttpPost]
        public async Task<IActionResult> RemoveFromAdminRole(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Email != "hemidoffa55@gmail.com")
            {
                return Forbid();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Eğer admin rolü varsa, rolü kaldırın
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }

            return RedirectToAction("Index", new { id = userId });
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }




        public async Task<IActionResult> ListMessages()
        {
            var messages = await appDbContext.Messages.ToListAsync();
            return View(messages);
        }

        
        public async Task<IActionResult> Reademail(int id)
        {
            var message = await appDbContext.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }
            // Mesajı okundu olarak işaretle
            message.IsRead = true;
            await appDbContext.SaveChangesAsync();
            var viewModel = new MessageVM
            {
                Id = message.Id,
                Author = message.Author,
                Email = message.Email,
                Body = message.Body,
                DateSent = message.DateSent,
                IsRead = message.IsRead
            };
            return View(message);
        }

   
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var message = await appDbContext.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            // Mesajı okundu olarak işaretle
            message.IsRead = true;
            await appDbContext.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> SendReplyEmail(string RecipientEmail, string Subject, string Body, int Id)
        {
            if (string.IsNullOrWhiteSpace(RecipientEmail) || string.IsNullOrWhiteSpace(Subject) || string.IsNullOrWhiteSpace(Body))
            {
                return BadRequest("All fields are required.");
            }

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("hemidoffa", "Hemidoffa55@gmail.com"));
                message.To.Add(new MailboxAddress("", RecipientEmail));
                message.Subject = Subject;

                message.Body = new TextPart("plain")
                {
                    Text = Body
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("Hemidoffa55@gmail.com", "ehakfkeoqhiiamhv");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return RedirectToAction("ListMessages", "Admin");
            }
            catch (Exception ex)
            {
                // Hata işlemi
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
