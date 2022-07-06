/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.Repository.Contracts;
using WiiTrakApi.Repository;
using WiiTrakApi.Services;
using WiiTrakApi.Services.Contracts;
using WiiTrakApi.DTOs;

namespace WiiTrakApi.Helpers
{
    public static class ServiceExtensions
    {
        public static void ConfigureCorsPolicy(this IServiceCollection services)
        {
            //services.AddCors(policy =>
            //{
            //    policy.AddPolicy("CorsPolicy", options => options
            //        .AllowAnyOrigin()
            //        .AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .WithExposedHeaders("totalAmountPages"));
            //});
            services.AddCors(policy =>
            {
                policy.AddPolicy("WiiTrakPolicy",
                 options => options
                 .SetIsOriginAllowedToAllowWildcardSubdomains()
                 //.SetIsOriginAllowed((host) => true)
                    .WithOrigins("https://black-hill-07a28510f.1.azurestaticapps.net",
                                 "https://wiitrakdemo.com",
                                 "https://localhost:7160")//.WithMethods("GET", "POST", "PUT") // Allow API calls only for these method types.
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials() // To allow the credentials for windows authentication
                    );
            });
        }

        public static void ConfigureAddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
        public static void ConfigureAddMailSetting(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettingsDto>(configuration.GetSection("MailSettings"));

        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IServiceProviderRepository, ServiceProviderRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<ITechnicianRepository, TechnicianRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ITrackingDeviceRepository, TrackingDeviceRepository>();
            services.AddScoped<IRepairIssueRepository, RepairIssueRepository>();
            services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
            services.AddScoped<ICorporateRepository, CorporateRepository>();
            services.AddScoped<IDeliveryTicketRepository, DeliveryTicketRepository>();
            services.AddScoped<ICartHistoryRepository, CartHistoryRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IDriverStoresRepository, DriverStoresRepository>();
            services.AddScoped<ISystemOwnerRepository, SystemOwnerRepository>();
            services.AddScoped<IAuthenticateRepository, AuthenticateRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<ICountyCodeRepository, CountyCodeRepository>();
            services.AddScoped<ISimCardsRepository, SimCardsRepository>();
            services.AddScoped<IDevicesRepository, DevicesRepository>();
            //services.AddScoped<ITrackSolidRepository, TrackSolidRepository>();
            services.AddScoped<IDeviceHistoryRepository, DeviceHistoryRepository>();
            services.AddScoped<ISimCardHistoryRepository, SimCardHistoryRepository>();
            services.AddScoped<IMessagesRepository, MessagesRepository>();

        }
    }
}
