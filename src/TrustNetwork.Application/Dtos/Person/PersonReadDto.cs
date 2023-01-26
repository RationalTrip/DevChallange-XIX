namespace TrustNetwork.Application.Dtos
{
    public class PersonReadDto
    {
        public string Id { get; set; } = string.Empty;
        public IEnumerable<string> Topics { get; set; } = null!;
    }
}
