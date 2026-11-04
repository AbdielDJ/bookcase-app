using BackEnd.Modules.Authors.Repositories;
using BackEnd.Modules.Authors.Services;

namespace BackEnd.Modules.Authors;

public static class AuthorsModule
{
    public static IServiceCollection AddAuthorsModule(this IServiceCollection services)
    {
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IAuthorService, AuthorService>();
        return services;
    }
}
