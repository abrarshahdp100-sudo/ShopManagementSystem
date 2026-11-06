using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Connection Register
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Background Service register kiya hai
builder.Services.AddHostedService<ShopAttendanceBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- ZAROORI FIX: Owner.html ko Default Page banane ke liye ---
DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
defaultFilesOptions.DefaultFileNames.Clear();
defaultFilesOptions.DefaultFileNames.Add("owner.html");

app.UseDefaultFiles(defaultFilesOptions);
app.UseStaticFiles();

app.UseAuthorization();
app.MapControllers();

app.Run();


// --- Background Service for Hourly Shop Opening Check ---
public class ShopAttendanceBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var currentTime = DateTime.Now;

            if (currentTime.Hour >= 10 && currentTime.Minute == 0)
            {
                // Background alert logic
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}