// MessageRepository.cs
using Ecome2.DAL;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace Ecome2.Models
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext appDbContext;
        public MessageRepository(AppDbContext _appDbContext)
        {
           appDbContext = _appDbContext;

        }

        public async Task AddMessageAsync(Message message)
        {
            appDbContext.Messages.Add(message);
            await appDbContext.SaveChangesAsync();
        }
    }
}
