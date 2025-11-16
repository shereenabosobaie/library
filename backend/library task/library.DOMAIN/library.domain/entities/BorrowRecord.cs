namespace library_mangment.library.domain.entities
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int bookId { get; set; }
        public int memberId { get; set; }
        public book book { get; set; }
        public member member { get; set; }


    }
}
