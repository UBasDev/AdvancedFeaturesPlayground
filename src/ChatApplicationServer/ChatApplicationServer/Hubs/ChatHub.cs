using ChatApplicationServer.Models.Chat;
using Microsoft.AspNetCore.SignalR;
using System.Globalization;

namespace ChatApplicationServer.Hubs
{
    public class ChatHub : Hub
    {
        private static HashSet<ChatQueueModel> WaitingQueue = new();
        private static HashSet<ChatMatchedListModel> ChatMatchedList = new();
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
            var currentUserConnectionId = Context.ConnectionId;
            const string errorMessage = "User has been disconnected.\n{DateTime.Now}";
            if (exception != null)
            {
                _logger.LogError($"An error occurred with {Context.ConnectionId} socket id.\n{errorMessage}");
            }
            _logger.LogInformation(message: $"User has been disconnected with {Context.ConnectionId} id.\n{errorMessage}");
            foreach (var currentQueue in WaitingQueue)
            {
                if (currentQueue.ConnectionId == currentUserConnectionId)
                {
                    WaitingQueue.Remove(currentQueue);
                    _logger.LogInformation(message: $"User has been removed from waiting queue with {Context.ConnectionId} id");
                    break;
                }
            }
            /* OPTIONAL FEATURE
            foreach (var currentChatMatched in ChatMatchedListModel)
            {
                if (currentChatMatched.Users.Any(u => u.ConnectionId == currentUserConnectionId)){
                    ChatMatchedListModel.Remove(currentChatMatched);
                    break;
                }
            }
            */
        }
        public async Task UserConnectedServerListener(UserConnectedServerListenerRequestModel requestBody)
        {
            var currentUserConnectionId = Context.ConnectionId;
            if (WaitingQueue.Count > 0)
            {
                var isAlreadyChatExists = false;
                ChatQueueModel firstUserOfWaitingList = WaitingQueue.First();

                foreach (var currentChat in ChatMatchedList)
                {
                    if (currentChat.JoinedUsersConnectionIdList.Any(list => list.Contains(currentUserConnectionId) && list.Contains(firstUserOfWaitingList.ConnectionId)))
                    {
                        await Clients.Client(firstUserOfWaitingList.ConnectionId).SendAsync("UserJoinedClientListener", currentChat);
                        await Clients.Client(currentUserConnectionId).SendAsync("UserJoinedClientListener", currentChat);
                        WaitingQueue.Remove(firstUserOfWaitingList);
                        isAlreadyChatExists = true;
                        return;
                    }
                }
                if (isAlreadyChatExists) return;

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
                    },
                    JoinedUsersConnectionIdList = new HashSet<string>() { currentUserConnectionId, firstUserOfWaitingList.ConnectionId }
                };
                ChatMatchedList.Add(matchedChat);
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
            var chatFound = ChatMatchedList.FirstOrDefault(chat => chat.Users.Any(user => user.ConnectionId.Contains(currentUserConnectionId))) ?? throw new ArgumentException($"There is no one in chat list with {{currentUserConnectionId}} id");
            var newMessageToAdd = new ChatMessage()
            {
                Content = requestBody.MessageContent,
                SendDate = DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), "MM/dd/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture),
                SenderConnectionId = currentUserConnectionId,
                SenderUsername = requestBody.Username
            };
            chatFound.Messages.Add(newMessageToAdd);
            foreach (var currentUser in chatFound.Users)
            {
                await Clients.Client(currentUser.ConnectionId).SendAsync("ChatReceivedClientListener", newMessageToAdd);
            }
        }
    }
}
