using Ecome2.DAL;
using Microsoft.AspNetCore.Mvc;
using Ecome2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Ecome2.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly AppDbContext appDbContext;
        public MessagesController(IMessageRepository messageRepository, AppDbContext _appDbContext)
        {
            _messageRepository = messageRepository;
            appDbContext = _appDbContext;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Message message)
        {
            if (ModelState.IsValid)
            {
                await _messageRepository.AddMessageAsync(message);
                // Optionally, you can send an email notification to admin here

                // Redirect to a thank you page or your home page
                return RedirectToAction("Index", "Home");
            }
            return View(message);
        }

        [Authorize(Roles = "Admin")] // Sadece admin rolüne sahip kullanıcılar erişebilir
        public async Task<IActionResult> ListMessages()
        {
            var messages = await appDbContext.Messages.ToListAsync();
            return View(messages);
        }

        [Authorize(Roles = "Admin")] // Sadece admin rolüne sahip kullanıcılar erişebilir
        public async Task<IActionResult> Reademail(int id)
        {
            var message = await appDbContext.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }
    }
}
