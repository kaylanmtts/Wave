namespace Wave.Domain.Entities
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int TotalDuration { get; set; }
        public int TotalMusics { get; set; }
        public int ArtistId { get; set; }
    }
}
