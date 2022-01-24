﻿using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.Repository.Contracts;
using WiiTrakApi.Repository;

namespace WiiTrakApi.Helpers
{
    public static class ServiceExtensions
    {
        public static void ConfigureCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(policy =>
            {
                policy.AddPolicy("CorsPolicy", options => options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("totalAmountPages"));
            });
        }

        public static void ConfigureAddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
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
        }
    }
}
