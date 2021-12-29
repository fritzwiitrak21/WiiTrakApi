using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WiiTrakApi.Data;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Repository
{
    public class TechnicianRepository: ITechnicianRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TechnicianRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, TechnicianModel? Technician, string? ErrorMessage)> GetTechnicianByIdAsync(Guid id)
        {
            var technician = await _dbContext.Technicians
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (technician is not null)
            {
                return (true, (TechnicianModel)technician, null);
            }
            return (false, null, "No technician found");
        }

        public async Task<(bool IsSuccess, List<TechnicianModel>? Technicians, string? ErrorMessage)> GetAllTechniciansAsync()
        {
            try
            {
                var technicians = await _dbContext.Technicians
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (technicians.Any())
                {
                    return (true, technicians, null);
                }
                return (false, null, "No technicians found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<TechnicianModel>? Technicians, string? ErrorMessage)> GetTechniciansByConditionAsync(Expression<Func<TechnicianModel, bool>> expression)
        {
            try
            {
                var technicians = await _dbContext.Technicians
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (technicians.Any())
                {
                    return (true, technicians, null);
                }
                return (false, null, "No technicians found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> TechnicianExistsAsync(Guid id)
        {
            try
            {
                var exists = await _dbContext.Technicians.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateTechnicianAsync(TechnicianModel technician)
        {
            try
            {
                await _dbContext.Technicians.AddAsync(technician);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateTechnicianAsync(TechnicianModel technician)
        {
            try
            {
                _dbContext.Technicians.Update(technician);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);

            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteTechnicianAsync(Guid id)
        {
            try
            {
                var recordToDelete = await _dbContext.Technicians.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null) return (false, "Technician not found");
                _dbContext.Technicians.Remove(recordToDelete);
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
