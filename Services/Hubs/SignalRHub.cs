using Microsoft.AspNetCore.SignalR;

namespace OvulaeDashboard.Services.Hubs
{
    public class SignalRHub : Hub
    {
        public async Task BroadcastUserCount(int count)
        {
            await Clients.All.SendAsync("AdminDashboardRefresh", count);
        }

        public async Task BroadcastLinkClick(string linkId)
        {
            await Clients.All.SendAsync("ReceiveLinkClick", linkId);
        }
    }
}
