using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Infrastructure.Data.Seed;
using AutoCenter.Web.Infrastructure.Images;
using AutoCenter.Web.Models;
using AutoCenter.Web.Services;
using AutoCenter.Web.Services.Images;
using AutoCenter.Web.Services.Listings;
using AutoCenter.Web.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var dbPath = Path.Combine(builder.Environment.ContentRootPath, "autocenter.db");
var cs = $"Data Source={dbPath}";

builder.Services.AddDbContext<AutoCenterDbContext>(o => o.UseSqlite(cs));
builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options=>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(14); //Cookie lasts for 14 days
    options.SlidingExpiration = true;
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

        options.SignIn.RequireConfirmedEmail=true;
        options.User.RequireUniqueEmail=true;
    })
    .AddEntityFrameworkStores<AutoCenterDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Error";
});
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(3);
});
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SMTP"));
builder.Services.Configure<ImageStorageOptions>(builder.Configuration.GetSection("ImageStorage"));
builder.Services.AddScoped<IImageStorage, LocalImageStorage>();
builder.Services.AddScoped<IListingImageService, ListingImageService>();
builder.Services.AddSingleton<IEmailService, EmailService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AutoCenterDbContext>();
    await db.Database.MigrateAsync();

    var brandSeeder = new CarBrandSeeder(db);
    await brandSeeder.SeedAsync();
    var modelSeeder = new CarModelSeeder(db);
    await modelSeeder.SeedAsync();
}
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AutoCenterDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DataSeeder");
        await DataSeeder.SeedAsync(db, logger);
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
