namespace LibraryManagementSystem_API.Models
{
    public class Book
    {
        public int bookID
        { get; set; }
        public string bookTitle
        { get; set; }
        public string bookAuthor
        { get; set; }
        public bool isBorrowed
        { get; set; }
        public string? borrowedBy
        { get; set; }
        public DateTime? borrowedAt
        { get; set; }
        public DateTime? returnedAt
        { get; set; }

    }
}
