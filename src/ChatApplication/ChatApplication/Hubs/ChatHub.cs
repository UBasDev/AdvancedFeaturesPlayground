using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync(method: "ConnectedChannel", $"User connected with {Context.ConnectionId} id");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (exception != null)
            {
                this._logger.LogError($"Error occurred and user disconnected with {Context.ConnectionId}\n{exception.Message}\n{exception}");
            }
            await Clients.All.SendAsync(method: "DisconnectedChannel", $"User disconnected with {Context.ConnectionId} id");
        }
    }
}
