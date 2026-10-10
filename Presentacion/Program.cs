using Microsoft.AspNetCore.Authentication.Cookies;
using Negocio;
using Presentacion.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Login con cookies: si alguien no autenticado entra a una página protegida, va a /Account/Login
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Datos de ejemplo + usuario inicial (solo se cargan si las bases están vacías)
foreach (var mensaje in DataSeeder.Sembrar(
             app.Configuration.ObtenerCadena("LiteDB"),
             app.Configuration.ObtenerCadena("MongoDB")))
{
    app.Logger.LogInformation("Seed: {Mensaje}", mensaje);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();   // siempre ANTES de UseAuthorization
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Alumno}/{action=Index}/{id?}");

app.Run();