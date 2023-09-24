namespace BookService.Application.Domain.Entity
{
    public class Book
    {
        public long Id { get; set; }
        public string BookName { get; set; } = "";
        public int Price { get; set; } = 0;
        public long AuthorId { get; set; }
    }
}
