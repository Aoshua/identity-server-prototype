using Microsoft.EntityFrameworkCore;
using Prototype.Identity.Configuration;
using Prototype.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Prototype.Identity.Data.Models;

#region Services
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PrototypeIdentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<PrototypeIdentityDbContext>()
    .AddDefaultTokenProviders();

IdentityServerRegistrar.Register(builder.Services, builder.Configuration);

#endregion

#region App
var app = builder.Build();

if (Convert.ToBoolean(app.Configuration.GetSection("MigrateAndSeed").Value))
    PrototypeIdentitySeeder.Seed(app);

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