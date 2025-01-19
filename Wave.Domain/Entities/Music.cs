namespace Wave.Domain.Entities
{
    public class Music
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public string Genre { get; set; }
        public int ArtistId { get; set; }
        public int AlbumId { get; set; }
    }
}
