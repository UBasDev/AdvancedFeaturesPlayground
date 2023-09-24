﻿namespace ChatApplicationServer.Models.Chat
{
    public class ChatQueueModel
    {
        public string ConnectionId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public byte Age { get; set; } = byte.MinValue;
        public string City { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
    }
}
