using Microsoft.AspNetCore.OData;
using WiiTrakApi.Data;
using WiiTrakApi.Helpers;
using WiiTrakApi.Repository;
using WiiTrakApi.Repository.Contracts;
using Hangfire;
using Hangfire.SqlServer;
using System.Configuration;
using WiiTrakApi.Services;
using WiiTrakApi.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddOData(options =>
    options.Select().Filter().OrderBy());


builder.Services.ConfigureCorsPolicy();

builder.Services.ConfigureAddDbContext(builder.Configuration);

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Add repositories
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IServiceProviderRepository, ServiceProviderRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<ITechnicianRepository, TechnicianRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ITrackingDeviceRepository, TrackingDeviceRepository>();
builder.Services.AddScoped<IRepairIssueRepository, RepairIssueRepository>();
builder.Services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
builder.Services.AddScoped<ICorporateRepository, CorporateRepository>();

builder.Services.AddScoped<IBackgroundJobService, BackgroundJobService>();


// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();

app.Run();
