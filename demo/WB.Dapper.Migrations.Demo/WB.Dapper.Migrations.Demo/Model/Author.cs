namespace WB.Dapper.Migrations.Demo.Model
{
    public sealed class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Born { get; set; }
        public DateTime? Died { get; set; }
    }
}
