using System.Threading.Tasks;

namespace PricingSpy.Notifier
{
    public interface INotificationManager
    {
        Task SendMessage(string message);
    }
}
