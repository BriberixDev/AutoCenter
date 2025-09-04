using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Services;
using AutoCenter.Web.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "autocenter.db");
var cs = $"Data Source={dbPath}";

builder.Services.AddDbContext<AutoCenterDbContext>(o => o.UseSqlite(cs));

builder.Services.AddRazorPages();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options=>
    {
        options.Password.RequiredLength = 8;
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
