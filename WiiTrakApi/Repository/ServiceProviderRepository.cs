using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class ServiceProviderRepository: IServiceProviderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ServiceProviderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, ServiceProviderModel? ServiceProvider, string? ErrorMessage)> GetServiceProviderByIdAsync(Guid id)
        {
            var serviceProvider = await _dbContext.ServiceProviders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (serviceProvider is not null)
            {
                return (true, (ServiceProviderModel)serviceProvider, null);
            }
            return (false, null, "No serviceProvider found");
        }

        public async Task<(bool IsSuccess, List<ServiceProviderModel>? ServiceProviders, string? ErrorMessage)> GetAllServiceProvidersAsync()
        {
            try
            {
                var serviceProviders = await _dbContext.ServiceProviders
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (serviceProviders.Any())
                {
                    return (true, serviceProviders, null);
                }
                return (false, null, "No serviceProviders found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<ServiceProviderModel>? ServiceProviders, string? ErrorMessage)> GetServiceProvidersByConditionAsync(Expression<Func<ServiceProviderModel, bool>> expression)
        {
            try
            {
                var serviceProviders = await _dbContext.ServiceProviders
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (serviceProviders.Any())
                {
                    return (true, serviceProviders, null);
                }
                return (false, null, "No serviceProviders found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> ServiceProviderExistsAsync(Guid id)
        {
            try
            {
                var exists = await _dbContext.ServiceProviders.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateServiceProviderAsync(ServiceProviderModel serviceProvider)
        {
            try
            {
                await _dbContext.ServiceProviders.AddAsync(serviceProvider);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateServiceProviderAsync(ServiceProviderModel serviceProvider)
        {
            try
            {
                _dbContext.ServiceProviders.Update(serviceProvider);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);

            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteServiceProviderAsync(Guid id)
        {
            try
            {
                var recordToDelete = await _dbContext.ServiceProviders.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null) return (false, "ServiceProvider not found");
                _dbContext.ServiceProviders.Remove(recordToDelete);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }
    }
}
