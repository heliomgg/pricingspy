using System;
using System.Threading.Tasks;

namespace PricingSpy.Notifier
{
    public class ConsoleNotificationManager : INotificationManager
    {
        public async Task SendMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
