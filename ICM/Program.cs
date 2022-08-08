using DataLayer;
using DataLayer.Data;
using ICM;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//Get connection string
string conection = builder.Configuration.GetConnectionString("DefaultConnection");

//Add context Db
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(conection));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<EmailService>();
builder.Services.AddAuthentication().AddGoogle();

// Add services to the DbInitializer
builder.Services.AddScoped<DbInitializer>();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

SeedDatabase();

app.Run();


void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
        dbInitializer.Initialize();
    }
}
