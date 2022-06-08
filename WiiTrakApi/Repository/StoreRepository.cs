/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;
using WiiTrakApi.Data;
using WiiTrakApi.DTOs;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Cores;
using WiiTrakApi.Repository.Contracts;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Data;
using System.Text;
using WiiTrakApi.SPModels;
namespace WiiTrakApi.Repository
{
    public class StoreRepository : IStoreRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StoreRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool IsSuccess, StoreModel? Store, string? ErrorMessage)> GetStoreByIdAsync(Guid id)
        {
                var store = await _dbContext.Stores
                    .Include(x => x.Carts)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (store is not null)
                {
                    return (true, store, null);
                }
                return (false, null, "No store found");
        }

        public async Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetAllStoresAsync()
        {
            try
            {
                var stores = await _dbContext.Stores
                    //.Include(x => x.Carts)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (stores.Any())
                {
                    return (true, stores, null);
                }
                return (false, null, "No stores found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<SpGetDriverAssignedStores>? Stores, string? ErrorMessage)> GetStoresByDriverId(Guid DriverId)
        {
            try
            {
                const string sqlquery = "Exec SpGetDriverAssignedStores @DriverId";

                List<SqlParameter> parms;

                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@DriverId", Value = DriverId },

                };

                var Stores = await _dbContext.SpGetDriverAssignedStores.FromSqlRaw(sqlquery, parms.ToArray()).ToListAsync();

                if (Stores != null)
                {
                    return (true, Stores, null);
                }
                return (false, null, "No Users found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetStoresByConditionAsync(Expression<Func<StoreModel, bool>> expression)
        {
            try
            {
                var stores = await _dbContext.Stores
                    .Where(expression)
                    .Select(x => x)
                    .AsNoTracking()
                    .ToListAsync();

                if (stores.Any())
                {
                    return (true, stores, null);
                }
                return (false, null, "No stores found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, StoreReportDto? Report, string? ErrorMessage)> GetStoreReportById(Guid Id)
        {
            try
            {
                var report = new StoreReportDto();


                //var carts = new List<CartModel>();
                var carts = new List<CartModel>();

                var storeCarts = await _dbContext.Stores
                    .Include(x => x.Carts)
                    .FirstOrDefaultAsync(x => x.Id == Id);
                carts = storeCarts.Carts;


                //int totalStores = storeCarts.Count();
                int totalCarts = carts.Count();
                int totalCartsAtStore = carts.Count(x => x.Status == CartStatus.InsideGeofence);
                int totalCartsOutsideStore = carts.Count(x => x.Status == CartStatus.OutsideGeofence);
                int totalCartsNeedingRepair = carts.Count(x => x.Condition == CartCondition.Damage);
                int totalCartsLost = carts.Count(x => x.Status == CartStatus.Lost);

                int cartsOnVehicleToday = 0;
                int cartsDeliveredToday = 0;
                int cartsNeedingRepairToday = 0;
                int cartsLostToday = 0;


                cartsOnVehicleToday = carts.Count(x => x.UpdatedAt is not null &&
                                                    x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                    x.Status == CartStatus.PickedUp);

                cartsDeliveredToday = carts.Count(x => x.UpdatedAt is not null &&
                                                   x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                   x.Status == CartStatus.InsideGeofence);

                cartsNeedingRepairToday = carts.Count(x => x.UpdatedAt is not null &&
                                                   x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                   x.Condition == CartCondition.Damage);

                cartsLostToday = carts.Count(x => x.UpdatedAt is not null &&
                                                  x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                  x.Status == CartStatus.Lost);

                report.TotalStores = 0;
                report.TotalCarts = totalCarts;
                report.TotalCartsAtStore = totalCartsAtStore;
                report.TotalCartsLost = totalCartsLost;
                report.TotalCartsNeedingRepair = totalCartsNeedingRepair;
                report.TotalCartsOutsideStore = totalCartsOutsideStore;

                report.CartsDeliveredToday = cartsDeliveredToday;
                report.CartsLostToday = cartsLostToday;
                report.CartsNeedingRepairToday = cartsNeedingRepairToday;
                report.CartsOnVehicleToday = cartsOnVehicleToday;

                return (true, report, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, StoreReportDto? Report, string? ErrorMessage)> GetAllStoreReportByDriverId(Guid driverId)
        {
            try
            {
                var report = new StoreReportDto();


                //var carts = new List<CartModel>();
                var carts = new List<CartModel>();
                var driverStores = await _dbContext.DriverStores
                   .Include(x => x.Store)
                   .Where(x => x.DriverId == driverId)
                   .AsNoTracking()
                   .ToListAsync();

                var stores = driverStores.Select(x => x.Store).ToList();
                foreach (var store in stores)
                {
                    var storeCarts = await _dbContext.Stores
                        .Include(x => x.Carts)
                        .FirstOrDefaultAsync(x => x.Id == store.Id);
                    carts.AddRange(storeCarts.Carts);
                }


                //int totalStores = storeCarts.Count();
                int totalCarts = carts.Count();
                int totalCartsAtStore = carts.Count(x => x.Status == CartStatus.InsideGeofence);
                int totalCartsOutsideStore = carts.Count(x => x.Status == CartStatus.OutsideGeofence);
                int totalCartsNeedingRepair = carts.Count(x => x.Condition == CartCondition.Damage);
                int totalCartsLost = carts.Count(x => x.Status == CartStatus.Lost);

                int cartsOnVehicleToday = 0;
                int cartsDeliveredToday = 0;
                int cartsNeedingRepairToday = 0;
                int cartsLostToday = 0;


                cartsOnVehicleToday = carts.Count(x => x.UpdatedAt is not null &&
                                                    x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                    x.Status == CartStatus.PickedUp);

                cartsDeliveredToday = carts.Count(x => x.UpdatedAt is not null &&
                                                   x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                   x.Status == CartStatus.InsideGeofence);

                cartsNeedingRepairToday = carts.Count(x => x.UpdatedAt is not null &&
                                                   x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                   x.Condition == CartCondition.Damage);

                cartsLostToday = carts.Count(x => x.UpdatedAt is not null &&
                                                  x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                  x.Status == CartStatus.Lost);

                report.TotalStores = 0;
                report.TotalCarts = totalCarts;
                report.TotalCartsAtStore = totalCartsAtStore;
                report.TotalCartsLost = totalCartsLost;
                report.TotalCartsNeedingRepair = totalCartsNeedingRepair;
                report.TotalCartsOutsideStore = totalCartsOutsideStore;

                report.CartsDeliveredToday = cartsDeliveredToday;
                report.CartsLostToday = cartsLostToday;
                report.CartsNeedingRepairToday = cartsNeedingRepairToday;
                report.CartsOnVehicleToday = cartsOnVehicleToday;

                return (true, report, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, StoreReportDto? Report, string? ErrorMessage)> GetAllStoreReportByCorporateId(Guid corporateId)
        {
            try
            {
                var report = new StoreReportDto();

                var carts = new List<CartModel>();
                var corporate =
                    await _dbContext.Corporates
                        .Include(x => x.Stores)
                        .FirstOrDefaultAsync(x => x.Id == corporateId);

                //var stores = corporate.Stores;
                foreach (var store in corporate.Stores)
                {
                    var storeCarts = await _dbContext.Stores
                        .Include(x => x.Carts)
                        .FirstOrDefaultAsync(x => x.Id == store.Id);
                    carts.AddRange(storeCarts.Carts);
                }


                //int totalStores = storeCarts.Count();
                int totalCarts = carts.Count();
                int totalCartsAtStore = carts.Count(x => x.Status == CartStatus.InsideGeofence);
                int totalCartsOutsideStore = carts.Count(x => x.Status == CartStatus.OutsideGeofence);
                int totalCartsNeedingRepair = carts.Count(x => x.Condition == CartCondition.Damage);
                int totalCartsLost = carts.Count(x => x.Status == CartStatus.Lost);

                int cartsOnVehicleToday = 0;
                int cartsDeliveredToday = 0;
                int cartsNeedingRepairToday = 0;
                int cartsLostToday = 0;


                cartsOnVehicleToday = carts.Count(x => x.UpdatedAt is not null &&
                                                    x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                    x.Status == CartStatus.PickedUp);

                cartsDeliveredToday = carts.Count(x => x.UpdatedAt is not null &&
                                                   x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                   x.Status == CartStatus.InsideGeofence);

                cartsNeedingRepairToday = carts.Count(x => x.UpdatedAt is not null &&
                                                   x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                   x.Condition == CartCondition.Damage);

                cartsLostToday = carts.Count(x => x.UpdatedAt is not null &&
                                                  x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                  x.Status == CartStatus.Lost);

                report.TotalStores = 0;
                report.TotalCarts = totalCarts;
                report.TotalCartsAtStore = totalCartsAtStore;
                report.TotalCartsLost = totalCartsLost;
                report.TotalCartsNeedingRepair = totalCartsNeedingRepair;
                report.TotalCartsOutsideStore = totalCartsOutsideStore;

                report.CartsDeliveredToday = cartsDeliveredToday;
                report.CartsLostToday = cartsLostToday;
                report.CartsNeedingRepairToday = cartsNeedingRepairToday;
                report.CartsOnVehicleToday = cartsOnVehicleToday;

                return (true, report, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, StoreReportDto? Report, string? ErrorMessage)> GetAllStoreReportByCompanyId(Guid companyId)
        {
            try
            {
                var report = new StoreReportDto();

                var carts = new List<CartModel>();
                var company =
                    await _dbContext.Companies
                        .Include(x => x.Stores)
                        .FirstOrDefaultAsync(x => x.Id == companyId);

                //var stores = corporate.Stores;
                foreach (var store in company.Stores)
                {
                    var storeCarts = await _dbContext.Stores
                        .Include(x => x.Carts)
                        .FirstOrDefaultAsync(x => x.Id == store.Id);
                    carts.AddRange(storeCarts.Carts);
                }


                //int totalStores = storeCarts.Count();
                int totalCarts = carts.Count();
                int totalCartsAtStore = carts.Count(x => x.Status == CartStatus.InsideGeofence);
                int totalCartsOutsideStore = carts.Count(x => x.Status == CartStatus.OutsideGeofence);
                int totalCartsNeedingRepair = carts.Count(x => x.Condition == CartCondition.Damage);
                int totalCartsLost = carts.Count(x => x.Status == CartStatus.Lost);

                int cartsOnVehicleToday = 0;
                int cartsDeliveredToday = 0;
                int cartsNeedingRepairToday = 0;
                int cartsLostToday = 0;


                cartsOnVehicleToday = carts.Count(x => x.UpdatedAt is not null &&
                                                    x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                    x.Status == CartStatus.PickedUp);

                cartsDeliveredToday = carts.Count(x => x.UpdatedAt is not null &&
                                                   x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                   x.Status == CartStatus.InsideGeofence);

                cartsNeedingRepairToday = carts.Count(x => x.UpdatedAt is not null &&
                                                   x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                   x.Condition == CartCondition.Damage);

                cartsLostToday = carts.Count(x => x.UpdatedAt is not null &&
                                                  x.UpdatedAt.Value.Date == DateTime.Now.Date &&
                                                  x.Status == CartStatus.Lost);

                report.TotalStores = 0;
                report.TotalCarts = totalCarts;
                report.TotalCartsAtStore = totalCartsAtStore;
                report.TotalCartsLost = totalCartsLost;
                report.TotalCartsNeedingRepair = totalCartsNeedingRepair;
                report.TotalCartsOutsideStore = totalCartsOutsideStore;

                report.CartsDeliveredToday = cartsDeliveredToday;
                report.CartsLostToday = cartsLostToday;
                report.CartsNeedingRepairToday = cartsNeedingRepairToday;
                report.CartsOnVehicleToday = cartsOnVehicleToday;

                return (true, report, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetStoresByCorporateId(Guid corporateId)
        {
            try
            {
                var corporate =
                    await _dbContext.Corporates
                        .Include(x => x.Stores)
                        .FirstOrDefaultAsync(x => x.Id == corporateId);

                if (corporate is not null && corporate.Stores.Any())
                {
                    return (true, corporate.Stores, null);
                }
                return (false, null, "No stores found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, List<StoreModel>? Stores, string? ErrorMessage)> GetStoresByCompanyId(Guid companyId)
        {
            try
            {
                var company =
                    await _dbContext.Companies
                        .Include(x => x.Stores)
                        .FirstOrDefaultAsync(x => x.Id == companyId);

                if (company is not null && company.Stores.Any())
                {
                    return (true, company.Stores, null);
                }
                return (false, null, "No stores found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool IsSuccess, List<SPGetStoresBySystemOwnerId>? Stores, string? ErrorMessage)> GetStoresBySystemOwnerId(Guid SystemownerId)
        {
            try
            {
                const string sqlquery = "Exec SPGetStoresBySystemOwnerId @SystemownerId";

                List<SqlParameter> parms;

                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@SystemownerId", Value = SystemownerId },

                };

                var Stores = await _dbContext.SPGetStoresBySystemOwnerId.FromSqlRaw(sqlquery, parms.ToArray()).ToListAsync();

                if (Stores != null)
                {
                    return (true, Stores, null);
                }

                return (false, null, "No stores found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, bool Exists, string? ErrorMessage)> StoreExistsAsync(Guid id)
        {
            try
            {
                var exists = await _dbContext.Stores.AnyAsync(x => x.Id.Equals(id));
                return (true, exists, null);
            }
            catch (Exception ex)
            {
                return (false, false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> CreateStoreAsync(StoreModel store)
        {
            try
            {
                #region Get Lat Long from Address
                string Address = store.StreetAddress1 + " " + store.StreetAddress2 + "," + store.City + "," + store.State + " " + store.PostalCode;
                LatitudeLongitude LatLong = GetLatLong(Address);
                if (LatLong != null)
                {
                    store.Latitude = LatLong.Latitude;
                    store.Longitude = LatLong.Longitude;
                    store.TimezoneDiff = LatLong.TimezoneDiff;
                    store.TimezoneName= LatLong.TimezoneName;
                }
                #endregion
                await _dbContext.Stores.AddAsync(store);

                #region Adding Store details to users table
                UsersModel user = new UsersModel();
                user.Id = store.Id;
                user.FirstName = store.StoreName;
                user.Password = Core.CreatePassword();
                user.Email = store.Email;
                user.AssignedRole = (int)Role.Store;
                user.CreatedAt =
                user.PasswordLastUpdatedAt = DateTime.UtcNow;
                user.IsActive = true;
                user.IsFirstLogin = true;

                await _dbContext.Users.AddAsync(user);
                #endregion


                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateStoreAsync(StoreModel store)
        {
            try
            {
                #region Update store details to users table
                const string sqlquery = "Exec SpUpdateUserDetails @Id,@FirstName,@LastName,@IsActive,@Email";

                List<SqlParameter> parms;

                parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@Id", Value = store.Id},
                     new SqlParameter { ParameterName = "@FirstName", Value = store.StoreName },
                     new SqlParameter { ParameterName = "@LastName", Value = store.StoreNumber },
                     new SqlParameter { ParameterName = "@IsActive", Value = store.IsActive },
                     new SqlParameter { ParameterName = "@Email", Value = store.Email }
                };
                var Result = await _dbContext.Database.ExecuteSqlRawAsync(sqlquery, parms.ToArray());
                #endregion

                #region Get Lat Long from Address
                string Address = store.StreetAddress1 + " " + store.StreetAddress2 + "," + store.City + "," + store.State + " " + store.PostalCode;
                LatitudeLongitude LatLong = GetLatLong(Address);
                if (LatLong != null)
                {
                    store.Latitude = LatLong.Latitude;
                    store.Longitude = LatLong.Longitude;
                    store.TimezoneDiff = LatLong.TimezoneDiff;
                    store.TimezoneName = LatLong.TimezoneName;
                }
                #endregion

                _dbContext.Stores.Update(store);
                await _dbContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteStoreAsync(Guid id)
        {
            try
            {
                var recordToDelete = await _dbContext.Stores.FirstOrDefaultAsync(x => x.Id == id);
                if (recordToDelete is null)
                {
                    return (false, "Store not found");
                }
                _dbContext.Stores.Remove(recordToDelete);
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
        public LatitudeLongitude GetLatLong(string Address)
        {
            string APIKey = "AIzaSyAUc0IKnyHlqoltF0zEzVAIAz6NUCQdeDE";
            string url = "https://maps.googleapis.com/maps/api/geocode/xml?address=" + Address + "&key=" + APIKey + "";
            WebRequest request = WebRequest.Create(url);
            LatitudeLongitude LatLong = new();
            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                try
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))

                    {

                        DataSet dsResult = new DataSet();

                        dsResult.ReadXml(reader);

                        DataTable dtCoordinates = new DataTable();

                        dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),

                                        new DataColumn("Address", typeof(string)),

                                        new DataColumn("Latitude",typeof(string)),

                                        new DataColumn("Longitude",typeof(string)) });

                        foreach (DataRow row in dsResult.Tables["result"].Rows)

                        {

                            string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"])[0]["geometry_id"].ToString();

                            DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];

                            dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);

                            LatLong.Latitude = Convert.ToDouble(dtCoordinates.Rows[0]["latitude"].ToString());
                            LatLong.Longitude = Convert.ToDouble(dtCoordinates.Rows[0]["longitude"].ToString());

                        }
                        string Timestamp = DateTime.UtcNow.ToString("HHmmssffff");
                        string url1 = "https://maps.googleapis.com/maps/api/timezone/xml?location=" + LatLong.Latitude + "," + LatLong.Longitude + "&timestamp=" + Timestamp + "&key=" + APIKey + "";
                        WebRequest requestzone = WebRequest.Create(url1);
                        using (WebResponse responsezone = (HttpWebResponse)requestzone.GetResponse())
                        {
                            using (StreamReader readerzone = new StreamReader(responsezone.GetResponseStream(), Encoding.UTF8))

                            {
                                DataSet dszoneResult = new DataSet();
                                dszoneResult.ReadXml(readerzone);
                                foreach (DataRow row in dszoneResult.Tables["TimeZoneResponse"].Rows)
                                {
                                    var raw_offset = row["raw_offset"].ToString();
                                    var dst_offset = row["dst_offset"].ToString();
                                    LatLong.TimezoneDiff =(Convert.ToDouble(raw_offset) + Convert.ToDouble(dst_offset)).ToString();
                                    LatLong.TimezoneName = row["time_zone_name"].ToString();
                                }
                            }
                        }
                    }
                    return LatLong;
                }
                catch (Exception ex)
                {
                    return null;

                }
            }
        }
    }
    public class LatitudeLongitude
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string TimezoneDiff { get; set; }
        public string TimezoneName { get; set; }
    }
}
