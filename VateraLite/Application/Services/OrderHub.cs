using Microsoft.AspNetCore.SignalR;

namespace VateraLite.Application.Services
{
    public class OrderHub : Hub
    {
        public async Task SendOrderStatus(string orderId, string status)
        {
            await Clients.All.SendAsync("ReceiveOrderStatus", orderId, status);
        }
    }
}
