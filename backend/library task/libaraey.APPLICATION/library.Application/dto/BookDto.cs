namespace library_mangment.library.Application.dto
{
    public class BookDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int PublishYear { get; set; }
        public bool IsAvilable { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public double Rate { get; set; }
        public DateTime addedtime { get; set; }
    }
}
