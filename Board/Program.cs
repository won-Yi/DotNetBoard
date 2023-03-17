using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Board.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BoardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'BoardContext' not found.")));
// Add services to the container
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "Home",
    pattern: "{controller=Notices}/{action=Index}/{id?}");
    
app.Run();
