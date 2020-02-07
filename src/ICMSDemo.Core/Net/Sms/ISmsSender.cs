using System.Threading.Tasks;

namespace ICMSDemo.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}