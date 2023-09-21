using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Hubs
{
    public class ChatHub : Hub
    {
        public static List<string> WaitingQueueConnectionIdList { get; set; } = new List<string>();
        private readonly ILogger<ChatHub> _logger;
        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }
        public override async Task OnConnectedAsync()
        {
            if(WaitingQueueConnectionIdList.Count > 0)
            {
                await Clients.Client(WaitingQueueConnectionIdList[0]).SendAsync(method: "ConnectedChannel", Context.ConnectionId);
                await Clients.Client(Context.ConnectionId).SendAsync(method: "ConnectedChannel", WaitingQueueConnectionIdList[0]);
                WaitingQueueConnectionIdList.RemoveAt(0);
            }
            else
            {
                WaitingQueueConnectionIdList.Add(Context.ConnectionId);
            }
            Console.WriteLine($"List count: {WaitingQueueConnectionIdList.Count}");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (exception != null)
            {
                this._logger.LogError($"Error occurred and user disconnected with {Context.ConnectionId}\n{exception.Message}\n{exception}");
            }
            await Clients.All.SendAsync(method: "DisconnectedChannel", $"User disconnected with {Context.ConnectionId} id");
        }
        public async Task SendMessageServerListener(string user, string message)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveMessageClientListener", user, message);
        }
    }
}
