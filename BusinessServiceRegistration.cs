using Microsoft.AspNetCore.Authentication.Cookies;
using SimoshStore;
using SimoshStoreAPI;

public static class BusinessServiceRegistration
{
    public static IServiceCollection AddBusinessService(this IServiceCollection services)
    {

        services.AddHttpContextAccessor();

        
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IDataRepository, DataRepository>();
        services.AddTransient<IProductImageService, ProductImageService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<ICommentService, CommentService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IContactFormService, ContactFormService>();
        services.AddTransient<IBlogService, BlogService>();

        return services;
    }
}
