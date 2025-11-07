using Microsoft.AspNetCore.Authentication.Cookies;
using OvulaeDashboard.Services;
using OvulaeShared.Services.APIs.Affiliates;
using OvulaeShared.Services.APIs.Authentication;
using OvulaeShared.Services.APIs.Doctors;
using OvulaeShared.Services.APIs.Messaging;
using OvulaeShared.Services.APIs.ModuleServices;
using OvulaeShared.Services.APIs.Transactions;
using OvulaeShared.Services.APIs.Users;
using OvulaeShared.Services.Email;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddHttpContextAccessor();

// Services
builder.Services.AddScoped<IAuthenticationApi, AuthenticationApi>();
builder.Services.AddScoped<IMessagingApi, MessagingApi>();
builder.Services.AddScoped<IUsersApi, UsersApi>();
builder.Services.AddScoped<IOvulaeEmailService, OvulaeEmailService>();
builder.Services.AddScoped<IDoctorsApi, DoctorsApi>();
builder.Services.AddScoped<IAffiliatesApi, AffiliatesApi>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ITransactionsApi, TransactionsApi>();
builder.Services.AddScoped<IModuleLogsApi, ModuleLogsApi>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Authentication/Login"; 
    options.LogoutPath = "/Authentication/Logout";
    options.AccessDeniedPath = "/Authentication/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(2);
    options.SlidingExpiration = true;
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(2); 
    options.SlidingExpiration = true; 
    options.LoginPath = "/Authentication/Login"; 
    options.AccessDeniedPath = "/Authentication/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Login}/{id?}");

app.Run();
