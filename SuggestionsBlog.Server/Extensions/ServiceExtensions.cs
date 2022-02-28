
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace SuggestionsBlog.Server.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
        builder.Services.AddMemoryCache();
        builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();

        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureADB2C"));

        builder.Services.AddAuthorization(opt =>
        {
            opt.AddPolicy("Admin", policy => policy.RequireClaim("jobTitle", "Admin"));
        });

        builder.Services.AddSingleton<IDbConnection, DbConnection>();
        builder.Services.AddSingleton<ICategoryService, MongoCategoryService>();
        builder.Services.AddSingleton<IStatusService, MongoStatusService>();
        builder.Services.AddSingleton<IUserService, MongoUserService>();
        builder.Services.AddSingleton<ISuggestionService, MongoSuggestionService>();
    }
}
