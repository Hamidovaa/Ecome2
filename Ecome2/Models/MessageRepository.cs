using System.Threading.Tasks;

namespace Ecome2.Models
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);
    }
}
