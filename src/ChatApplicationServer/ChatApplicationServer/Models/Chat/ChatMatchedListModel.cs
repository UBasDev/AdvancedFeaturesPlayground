namespace ChatApplicationServer.Models.Chat
{
    public class ChatMatchedListModel
    {
        public HashSet<ConnectedUserInformation> Users { get; set; } = new HashSet<ConnectedUserInformation>();
        public HashSet<ChatMessage> Messages { get; set; } = new HashSet<ChatMessage>();

    }
    public class ConnectedUserInformation
    {
        public string ConnectionId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public byte Age { get; set; } = byte.MinValue;
        public string City { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
    }
    public class ChatMessage
    {
        public string SenderConnectionId { get; set; } = String.Empty;
        public DateTime SendDate { get; set; }
        public string Content { get; set; } = String.Empty;
    }
}
