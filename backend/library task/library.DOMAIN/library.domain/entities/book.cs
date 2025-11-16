namespace library_mangment.library.domain.entities
{
    public class book
    {
        public int Id { get; set; }
        public double Rate { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int PublishYear { get; set; }
        public bool IsAvilable { get; set; } = true;
        public string? ImageUrl { get; set; }
        public DateTime addedtime {  get; set; }
        public string? Description { get; set; }
        public ICollection<BorrowRecord> BorrowRecords { get; set; }
    }
}
