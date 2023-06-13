using Microsoft.EntityFrameworkCore;
using WebApplication1.Middleware;
using Microsoft.EntityFrameworkCore.Migrations;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using WebApplication1.Servicies;
using WebApplication1.Data;
using MySqlConnector;
using WebApplication1.Servicies.Hash;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IDateService, DateService>();
builder.Services.AddScoped<TimeService>();
builder.Services.AddTransient<DateTimeService>();


builder.Services.AddSingleton<IHashService, MD5HashService>();


builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});




String? connectionString = 
    builder.Configuration.GetConnectionString("PlanetScale");
 MySqlConnection connection = new(connectionString);
builder.Services.AddDbContext<DataContext>(
    options =>
    options.UseMySql(
        connection,
        ServerVersion.AutoDetect(connection),
        serverOptions =>
        serverOptions.MigrationsHistoryTable(
            tableName: HistoryRepository.DefaultTableName,
            schema: "asp111")
        .SchemaBehavior(
            MySqlSchemaBehavior.Translate,
            (schema, table) => $"{schema}_{table}")
        ));

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

app.UseMarker();


app.UseAuthorization();
app.UseSession();
app.UseMiddleware<LogoutMiddleware>();
app.UseAuthSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
