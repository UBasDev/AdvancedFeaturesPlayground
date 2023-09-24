namespace ChatApplicationServer.Models.Chat
{
    public class SendMessageToStrangerRequestModel
    {
        public string MessageContent { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
    }
}
