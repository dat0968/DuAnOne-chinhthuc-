
using Microsoft.EntityFrameworkCore;
using Du_An_One.Data;
using System.Net;
using System.Net.Mail;
using Du_An_One.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Du_An_OneContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Du_An_OneContext") ?? throw new InvalidOperationException("Connection string 'Du_An_OneContext' not found.")));

// Add services to the container.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.ExpireTimeSpan = TimeSpan.FromDays(365);
        options.SlidingExpiration = true; 
        options.Cookie.IsEssential = true; 
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
