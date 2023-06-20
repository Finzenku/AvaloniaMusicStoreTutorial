namespace MusicStore.Models
{
    public class Album
    {
        public required string Artist { get; set; }
        public required string AlbumTitle { get; set; }
        public string CoverUrl { get; set; } = "";
    }
}
