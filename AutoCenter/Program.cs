using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Infrastructure.Data.Seed;
using AutoCenter.Web.Infrastructure.Images;
using AutoCenter.Web.Models;
using AutoCenter.Web.Services;
using AutoCenter.Web.Services.Account;
using AutoCenter.Web.Services.Favourites;
using AutoCenter.Web.Services.Images;
using AutoCenter.Web.Services.Listings;
using AutoCenter.Web.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

static async Task<bool> CanConnectAsync(string connectionString)
{
    try
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();
        return true;
    }
    catch
    {
        return false;
    }
}
var dockerCs = builder.Configuration.GetConnectionString("Postgres");
var localCs = builder.Configuration.GetConnectionString("PostgresLocal")
    ?? throw new InvalidOperationException("Connection string 'PostgresLocal' not found.");

var connectionString =
    !string.IsNullOrWhiteSpace(dockerCs) && await CanConnectAsync(dockerCs)
        ? dockerCs
        : localCs;

builder.Services.AddDbContext<AutoCenterDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddRazorPages(options=>
{
    options.Conventions.AuthorizePage("/Listings/Edit");
    options.Conventions.AuthorizePage("/Listings/Delete");
    options.Conventions.AuthorizePage("/Listings/Create");
});


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options=>
    {
        options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.Password.RequiredLength = 6;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        
        options.Lockout.MaxFailedAccessAttempts=5;
        options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(15);

        //options.SignIn.RequireConfirmedEmail=true;
        options.SignIn.RequireConfirmedEmail = !builder.Environment.IsDevelopment();
        options.User.RequireUniqueEmail=true;
    })
    .AddEntityFrameworkStores<AutoCenterDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(14); //Cookie lasts for 14 days
    options.SlidingExpiration = true;
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(3);
});
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SMTP"));
builder.Services.Configure<ImageStorageOptions>(builder.Configuration.GetSection("ImageStorage"));
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IImageStorage, LocalImageStorage>();
builder.Services.AddScoped<IListingImageService, ListingImageService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddScoped<IFavouriteService,FavouriteService>();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AutoCenterDbContext>();
//    await db.Database.MigrateAsync();

//    var brandSeeder = new CarBrandSeeder(db);
//    await brandSeeder.SeedAsync();
//    var modelSeeder = new CarModelSeeder(db);
//    await modelSeeder.SeedAsync();
//}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AutoCenterDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DataSeeder");

    var seedDemoListings = builder.Configuration.GetValue<bool>("Seed:DemoListings");
    logger.LogWarning("Seed:DemoListings = {SeedDemoListings}", seedDemoListings);
    await DataSeeder.SeedAsync(db, logger, seedDemoListings);
}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
