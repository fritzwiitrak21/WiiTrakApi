using Microsoft.AspNetCore.OData;
using WiiTrakApi.Data;
using WiiTrakApi.Helpers;
using WiiTrakApi.Repository;
using WiiTrakApi.Repository.Contracts;
//using Hangfire;
//using Hangfire.SqlServer;
using System.Configuration;
using WiiTrakApi.Services;
using WiiTrakApi.Services.Contracts;
using WiiTrakApi.DTOs;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddOData(options =>
    options.Select().Filter().OrderBy());


builder.Services.ConfigureCorsPolicy();

builder.Services.ConfigureAddDbContext(builder.Configuration);

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddRepositories();

builder.Services.AddScoped<IBackgroundJobService, BackgroundJobService>();
builder.Services.AddScoped<IUploadService, UploadService>();

builder.Services.ConfigureAddMailSetting(builder.Configuration);







// Add Hangfire services.
//builder.Services.AddHangfire(configuration => configuration
//    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
//    .UseSimpleAssemblyNameTypeSerializer()
//    .UseRecommendedSerializerSettings()
//    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
//    {
//        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
//        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
//        QueuePollInterval = TimeSpan.Zero,
//        UseRecommendedIsolationLevel = true,
//        DisableGlobalLocks = true
//    }));

//builder.Services.AddHangfireServer();


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

//app.UseHangfireDashboard();

app.Run();
