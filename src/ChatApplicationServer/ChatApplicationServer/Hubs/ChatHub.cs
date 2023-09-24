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
        public async Task UserConnectedServerListener(UserConnectedServerListenerRequestModel requestBody)
        {
            var currentUserConnectionId = Context.ConnectionId;
            if (WaitingQueue.Count > 0)
            {
                ChatQueueModel firstUserOfWaitingList = WaitingQueue.First();
                var matchedChat = new ChatMatchedListModel()
                {
                    Users = new HashSet<ConnectedUserInformation>()
                    {
                        new()
                        {
                            ConnectionId = firstUserOfWaitingList.ConnectionId,
                            Username = firstUserOfWaitingList.Username,
                            City = firstUserOfWaitingList.City,
                            Age = firstUserOfWaitingList.Age,
                            Gender = firstUserOfWaitingList.Gender,
                        },
                        new()
                        {
                            ConnectionId = currentUserConnectionId,
                            Username = requestBody.Username,
                            City = requestBody.City,
                            Age = requestBody.Age,
                            Gender = requestBody.Gender,
                        }
                    }
                };
                ChatMatchedListModel.Add(matchedChat);
                await Clients.Client(firstUserOfWaitingList.ConnectionId).SendAsync("UserJoinedClientListener", matchedChat);
                await Clients.Client(currentUserConnectionId).SendAsync("UserJoinedClientListener", matchedChat);
                WaitingQueue.Remove(firstUserOfWaitingList);
            }
            else
            {
                WaitingQueue.Add(new ChatQueueModel()
                {
                    ConnectionId = currentUserConnectionId,
                    Username = requestBody.Username,
                    City = requestBody.City,
                    Age = requestBody.Age,
                    Gender = requestBody.Gender,
                });
            }
        }

        public async Task SendMessageToStrangerUserServerListener(SendMessageToStrangerRequestModel requestBody)
        {
            var currentUserConnectionId = Context.ConnectionId;
            var chatFound = ChatMatchedListModel.FirstOrDefault(chat => chat.Users.Any(user => user.ConnectionId.Contains(currentUserConnectionId))) ?? throw new ArgumentException($"There is no one in chat list with {{currentUserConnectionId}} id");
            var newMessageToAdd = new ChatMessage()
            {
                Content = requestBody.MessageContent,
                SendDate = DateTime.Now,
                SenderConnectionId = currentUserConnectionId,
                SenderUsername = requestBody.Username
            };
            chatFound.Messages.Add(newMessageToAdd);
            foreach (var currentUser in chatFound.Users)
            {
                await Clients.Client(currentUser.ConnectionId).SendAsync("ChatReceivedClientListener", newMessageToAdd);
            }
            var x1 = ChatMatchedListModel;
        }
    }
}
