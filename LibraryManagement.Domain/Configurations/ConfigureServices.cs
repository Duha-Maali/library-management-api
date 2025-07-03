using LibraryManagement.Business.Helpers;
using LibraryManagement.Business.IServices;
using LibraryManagement.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Business.Configurations;

public static class ConfigureServices
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        // Register Business services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IBorrowService, BorrowService>();
        services.AddScoped<IBorrowerService, BorrowerService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        services.AddHttpContextAccessor(); // For accessing UserId and Role in services
        return services;
    }
}
