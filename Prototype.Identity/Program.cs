using Microsoft.EntityFrameworkCore;
using Prototype.Identity.Configuration;
using Prototype.Identity.Data;
using Prototype.Identity.Data.Models;

#region Services
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PrototypeIdentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

//builder.Services.AddIdentityCore<User>()
//    .AddEntityFrameworkStores<PrototypeIdentityDbContext>();

IdentityServerRegistrar.Register(builder.Services, builder.Configuration);

#endregion

#region App
var app = builder.Build();

//IdentityServerSeeder.Seed(app); // Migration & seed for IdentityServer4 (only run once). To only run migrations: Migrations/README.md
//PrototypeIdentitySeeder.Seed(app); // Seeds the database (only run once)

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// TODO: only allow clients
app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
#endregion