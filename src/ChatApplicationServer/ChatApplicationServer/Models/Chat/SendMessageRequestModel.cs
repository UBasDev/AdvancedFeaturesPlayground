namespace ChatApplicationServer.Models.Chat
{
    public class SendMessageRequestModel
    {
        public string SenderConnectionId { get; set; } = String.Empty;
        public DateTime SendDate { get; set; }
        public string Content { get; set; } = String.Empty;
    }
}
