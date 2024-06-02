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
        public async Task<IActionResult> SendReplyEmail(string Subject, string Body, int MessageId)
        {
            // Gerekli alanların doldurulup doldurulmadığını kontrol edin
            if (string.IsNullOrWhiteSpace(Subject) || string.IsNullOrWhiteSpace(Body))
            {
                return BadRequest("All fields are required.");
            }

            // Admin bilgilerini sabit olarak tanımlayın
            string adminEmail = "Hemidoffa55@gmail.com";
            string adminPhone = "123-456-7890"; // Örnek telefon numarası, gerçek numaranızı buraya koyabilirsiniz.

            try
            {
                // Orijinal mesajı veritabanından alın
                var originalMessage = await appDbContext.Messages.FindAsync(MessageId);
                if (originalMessage == null)
                {
                    return NotFound("Original message not found.");
                }

                string recipientEmail = originalMessage.Email;

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Admin", adminEmail));
                message.To.Add(new MailboxAddress("", recipientEmail));
                message.Subject = Subject;

                message.Body = new TextPart("plain")
                {
                    Text = Body
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync(adminEmail, "ehakfkeoqhiiamhv");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                // Gönderilen mesajı veritabanına kaydet
                var sentMessage = new Message
                {
                    Subject = Subject,
                    Body = Body,
                    DateSent = DateTime.Now,
                    Sender = "Admin", // Admin kullanıcısının kimliği
                    Receiver = recipientEmail,
                    IsRead = false,
                    Author = "Admin", // Admin kullanıcısının kimliği
                    Email = adminEmail, // Admin e-posta adresi
                    Phone = adminPhone // Admin telefon numarası
                };

                appDbContext.Messages.Add(sentMessage);
                await appDbContext.SaveChangesAsync();

                return RedirectToAction("ListMessages", "Admin");
            }
            catch (DbUpdateException ex)
            {
                // Inner exception ayrıntılarını yakalama
                return StatusCode(500, $"Internal server error: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                // Genel hata işlemi
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public IActionResult Sent()
        {
            var sentMessages = appDbContext.Messages.Where(m => m.Sender == "Admin").ToList();
            return View(sentMessages);
        }

        //public async Task<IActionResult> Index()
        //{
        //    var messages = await appDbContext.Messages.Where(m => m.Category == "Inbox").ToListAsync();
        //    return PartialView("_MessageListPartial", messages);
        //}

        //public async Task<IActionResult> Sent()
        //{
        //    var messages = await appDbContext.Messages.Where(m => m.Category == "Sent").ToListAsync();
        //    return PartialView("_MessageListPartial", messages);
        //}


        public async Task<IActionResult> ListMessages()
        {
            var inbox = appDbContext.Messages.Where(m => m.Sender != "Admin").ToList();
            return View(inbox);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMessage(int id, string returnUrl)
        {
            try
            {
                var messageToDelete = await appDbContext.Messages.FindAsync(id);

                if (messageToDelete == null)
                {
                    return NotFound();
                }

                appDbContext.Messages.Remove(messageToDelete);
                await appDbContext.SaveChangesAsync();

                // returnUrl boş değilse oraya yönlendir
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                // returnUrl boş ise default olarak ListMessages sayfasına yönlendir
                return RedirectToAction(nameof(ListMessages));
            }
            catch (Exception ex)
            {
                // Hata durumunda geri dönüş
                return StatusCode(500, $"An error occurred while deleting the message: {ex.Message}");
            }
        }


    }

}
