namespace Shortener.Models
{
    public class LinkModel
    {
        public int Id { get; set; }
        public string OriginalLink { get; set; }
        public string ShortenedLink { get; set; }
    }
}
