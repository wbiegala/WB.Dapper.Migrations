namespace WB.Dapper.Migrations.Demo.Model
{
    public sealed class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public IEnumerable<Author> Authors { get; set; } = new List<Author>();
    }
}
