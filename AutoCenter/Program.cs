using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Infrastructure.Data.Seed;
using AutoCenter.Web.Models;
using AutoCenter.Web.Services;
using AutoCenter.Web.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var dbPath = Path.Combine(builder.Environment.ContentRootPath, "autocenter.db");
var cs = $"Data Source={dbPath}";

builder.Services.AddDbContext<AutoCenterDbContext>(o => o.UseSqlite(cs));

builder.Services.AddRazorPages();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options=>
    {
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

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SMTP"));
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
