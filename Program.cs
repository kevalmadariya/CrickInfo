using crickinfo_mvc_ef_core;
using crickinfo_mvc_ef_core.Models;
using crickinfo_mvc_ef_core.Models.Interfaces;
using crickinfo_mvc_ef_core.Models.Repository;
using crickinfo_mvc_ef_core.Services;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register HttpClientFactory
builder.Services.AddHttpClient();

// ✅ Register ScrapeService for Hangfire
builder.Services.AddScoped<IScrapeService, ScrapeService>();

// Add Hangfire services
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer();

// Dependency Injection for Repositories
builder.Services.AddScoped<IUserRepo, UserRepository>();
builder.Services.AddScoped<ITournamentRepo, TournamentRepository>();
builder.Services.AddScoped<ITeamRepo, TeamRepository>();
builder.Services.AddScoped<IMatchRepo, MatchRepository>();
builder.Services.AddScoped<IPointsTableRepo, PointstableRepository>();

// Database Connection
builder.Services.AddDbContext<CrickDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ✅ Add Hangfire Dashboard
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() } // Optional: Add authorization
});

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ✅ Register Hangfire Recurring Job using the Service
using (var scope = app.Services.CreateScope())
{
    var recurringJobs = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    // Schedule daily job at 12:00 PM
    recurringJobs.AddOrUpdate<IScrapeService>(
        "daily-scrape-job",
        service => service.FetchAndUpdateDataAsync(),
        "0 12 * * *" // Cron expression: every day at 12:00 PM
    );
}

app.Run();

// ✅ Optional: Hangfire Dashboard Authorization Filter
public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // For development, allow all access
        // In production, implement proper authorization
        return true;
    }
}