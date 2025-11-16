namespace library_mangment.library.domain.entities
{
    public class member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string role { get; set; }
        public string   Email { get; set; }
        public string passwordHas { get; set; }

        public string? ImageUrl { get; set; }
        public DateTime JoinDate { get; set; }

        public ICollection<BorrowRecord> BorrowRecords { get; set; }
    }
}
