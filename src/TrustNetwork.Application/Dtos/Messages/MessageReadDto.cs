namespace TrustNetwork.Application.Dtos.Messages
{
    public class MessageReadDto
    {
        public string SenderLogin { get; set; } = string.Empty;
        public IEnumerable<string> MessageReceived { get; set; } = Array.Empty<string>();
    }
}
