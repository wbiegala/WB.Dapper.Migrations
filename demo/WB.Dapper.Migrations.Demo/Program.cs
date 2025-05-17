using Microsoft.Extensions.DependencyInjection;
using WB.Dapper.Migrations;
using WB.Dapper.Migrations.Contract;
using WB.Dapper.Migrations.Demo;
using WB.Dapper.Migrations.Demo.Services;
using WB.Dapper.Migrations.Sqlite;


// SERVICES REGISTRATION
var services = new ServiceCollection();

// here is migration registration
services.AddDapperMigrations(options =>
{
    options.UseSqlite(Database.ConnectionString);
}, typeof(Database).Assembly);


services.AddSingleton<IDbConnectionProvider, DbConnectionProvider>();
services.AddScoped<IAuthorService, AuthorService>();
services.AddScoped<IBookService, BookService>();


var appContainer = services.BuildServiceProvider();


// MIGRATION EXECUTION
using (var appScope = appContainer.CreateScope())
{
    var migratior = appScope.ServiceProvider.GetRequiredService<IMigrationExecutor>();
    await migratior.MigrateDatabaseAsync();
}


// BUSINESS LOGIC SIMULATION
int? bookId;

using (var appScope = appContainer.CreateScope())
{
    var authorService = appScope.ServiceProvider.GetRequiredService<IAuthorService>();
    var bookService = appScope.ServiceProvider.GetRequiredService<IBookService>();

    var authorId = await authorService.AddAuthorAsync(new AddAuthor
    {
        FirstName = "John",
        LastName = "Doe",
        Born = new DateTime(1990, 1, 1),
        Died = null
    });

    bookId = await bookService.AddBookAsync(new AddBook
    {
        Title = "Sample Book",
        ISBN = "123456789",
        ReleaseYear = 2023,
        Authors = new List<int> { authorId.Value }
    });
}

using (var appScope = appContainer.CreateScope())
{
    var bookService = appScope.ServiceProvider.GetRequiredService<IBookService>();
    var book = await bookService.GetBookByIdAsync(bookId.Value);
    Console.WriteLine($"Book: {book?.Title}, ISBN: {book?.ISBN}, Release Year: {book?.ReleaseYear}");
}