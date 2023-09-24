using ChatApplicationServer.Models.Chat;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplicationServer.Hubs
{
    public class ChatHub : Hub
    {
        private static HashSet<ChatQueueModel> WaitingQueue = new();
        private static HashSet<ChatMatchedListModel> ChatMatchedListModel = new();
        private readonly ILogger<ChatHub> _logger;
        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }
        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"User has been connected with {Context.ConnectionId}.\n{DateTime.Now}");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            const string errorMessage = "User has been disconnected.\n{DateTime.Now}";
            if (exception != null)
            {
                _logger.LogError($"An error occurred with {Context.ConnectionId} socket id.\n{errorMessage}");
            }
            _logger.LogInformation(message: $"User has been disconnected with {Context.ConnectionId} id.\n{errorMessage}");
        }
        public async Task UserConnectedServerListener(UserConnectedServerListenerRequestModel requestModel)
        {
            if (WaitingQueue.Count > 0)
            {
                ChatQueueModel firstUserOfWaitingList = WaitingQueue.First();
                ChatMatchedListModel.Add(new ChatMatchedListModel()
                {
                    User1 = new ConnectedUserInformation()
                    {
                        ConnectionId = firstUserOfWaitingList.ConnectionId,
                        Username = firstUserOfWaitingList.Username,
                        City = firstUserOfWaitingList.City,
                        Age = firstUserOfWaitingList.Age,
                        Gender = firstUserOfWaitingList.Gender,
                    },
                    User2 = new ConnectedUserInformation()
                    {
                        ConnectionId = Context.ConnectionId,
                        Username = requestModel.Username,
                        City = requestModel.City,
                        Age = requestModel.Age,
                        Gender = requestModel.Gender,
                    }
                });
                WaitingQueue.Remove(firstUserOfWaitingList);
            }
            else
            {
                WaitingQueue.Add(new ChatQueueModel()
                {
                    ConnectionId = Context.ConnectionId,
                    Username = requestModel.Username,
                    City = requestModel.City,
                    Age = requestModel.Age,
                    Gender = requestModel.Gender,
                });
            }
        }
    }
}
