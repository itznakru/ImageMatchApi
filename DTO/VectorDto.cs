namespace MatchEngineApi.DTO
{
    public class VectorDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // my id
        public string MemberKey {get;set;} = null!; // member of system
        public string InternalKey { get; set; } // Internal key
        public byte[] Vector { get; set; }= null!;
        public string Image { get; set; }= null!;// Image in base64
        public ImageType ImageType { get; set; } //Type of image by suggest of our AI 
        public long CreateDateTime { get; set; } = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
    }
}