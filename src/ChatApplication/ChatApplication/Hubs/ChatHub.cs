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
            await Clients.All.SendAsync(method: "ConnectedChannel", $"User connected with {Context.ConnectionId} id");
            if(WaitingQueueConnectionIdList.Count > 0)
            {
                await Clients.Client(WaitingQueueConnectionIdList[0]).SendAsync(method: "ConnectedChannel", );
            }
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
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveMessageClientListener", user, message); //Belirtilen connection idye sahip socket haricindeki diğer bütün socketlere

            //await Clients.Others.SendAsync("ReceiveMessageClientListener", user, message); //Bu requesti atan socket haricindeki diğer bütün socketlere

            //await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessageClientListener", user, message); //Sadece belirtilen connection idye sahip sockete

            //await Clients.Caller.SendAsync("ReceiveMessageClientListener", user, message); //Sadece bu requesti atan socketin kendisine
        }
    }
}
