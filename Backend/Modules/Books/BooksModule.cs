using BackEnd.Modules.Books.Repositories;
using BackEnd.Modules.Books.Services;

namespace BackEnd.Modules.Books;

public static class BooksModule
{
    public static IServiceCollection AddBooksModule(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookService, BookService>();
        return services;
    }
}
