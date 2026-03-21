using Infrastructure;
using Application.Interfaces.Services;
using Infrastructure.Services;
using Application.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddInfrastructure(builder.Configuration);


// Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
//profile
builder.Services.AddScoped<IProfileService, ProfileService>();


// bật Session để lưu OTP
builder.Services.AddSession();
builder.Services.AddMemoryCache();

builder.Services.AddSignalR();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// dùng session
app.UseSession();

// Identity
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=RoomPosts}/{action=Index}/{id?}");

app.MapHub<Web.Hubs.ChatHub>("/chathub");

app.Run();

