using WiiTrakApi.Repository.Contracts;
using Faker;
using WiiTrakApi.Models;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Enums;

namespace WiiTrakApi.Data
{
    public class DataSeeder
    {
        // ref: https://medium.com/executeautomation/seeding-data-using-ef-core-in-asp-net-core-6-0-minimal-api-d5f6ecdb350c

        // TODO repositories

        // To run: dotnet run -- seeddata

        private readonly IAssetRepository _assetRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly ITechnicianRepository _technicianRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IServiceProviderRepository _serviceProviderRepository;
        private readonly ApplicationDbContext _dbContext;

        public DataSeeder(IAssetRepository assetRepository, 
            IDriverRepository driverRepository, 
            ITechnicianRepository technicianRepository, 
            IStoreRepository storeRepository, 
            IServiceProviderRepository serviceProviderRepository)
        {
            _assetRepository = assetRepository;
            //_driverRepository = driverRepository;
            //_technicianRepository = technicianRepository;
            //_storeRepository = storeRepository;
            //_serviceProviderRepository = serviceProviderRepository;
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            //await SeedServiceProviders();
            //await SeedStores();
            await SeedAssets();
            //await SeedTrackingDevices();
            //await SeedDrivers();
            //await SeedTechnicians();
            //await SeedDriverStores();
            //await SeedTechniciansStores();
        }

        public async Task SeedServiceProviders()
        {
            if (_dbContext.ServiceProviders.Any()) return;

            var serviceProvider = new ServiceProviderModel
            {
                ServiceProviderName = "Cart Tracking Services",
                Email = "info@carttrackingservices.com",
                PhonePrimary = "555-123-4567",
                LogoPicUrl = "https://aidottstorage.blob.core.windows.net/aidottblob/logoipsum-logo-8.svg",
                CreatedAt = DateTime.UtcNow
            };

            await _serviceProviderRepository.CreateServiceProviderAsync(serviceProvider);
        }


        public async Task SeedStores()
        {
            if (_dbContext.Stores.Any()) return;

            var sp = _dbContext.ServiceProviders.FirstOrDefault();

            var stores = new List<StoreModel>();

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Kroger",
                StoreNumber = "111",
                Email = "",
                PhonePrimary = "(470) 351-4222",
                StreetAddress1 = "725 Ponce De Leon Ave NE",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30306",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.3633590557509,
                Latitude = 33.77174713602629,
                ServiceProviderId = sp.Id,
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Kroger",
                StoreNumber = "222",
                Email = "",
                PhonePrimary = "(470) 447-5040",
                StreetAddress1 = "800 Glenwood Ave SE",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30316",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.35991288581688,
                Latitude = 33.74236966213573,
                ServiceProviderId = sp.Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Kroger",
                StoreNumber = "333",
                Email = "",
                PhonePrimary = "(404) 761-7409",
                StreetAddress1 = "2685 Metropolitan Pkwy SW",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30315",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.40935984686935,
                Latitude = 33.682489645464656,
                ServiceProviderId = sp.Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Target",
                StoreNumber = "444",
                Email = "",
                PhonePrimary = "(678) 954-4265",
                StreetAddress1 = "375 18th St NW",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30363",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.39939034610244,
                Latitude = 33.79343715586747,
                ServiceProviderId = sp.Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Target",
                StoreNumber = "555",
                Email = "",
                PhonePrimary = "(404) 260-0200",
                StreetAddress1 = "1275 Caroline St NE",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30307",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.34598880192449,
                Latitude = 33.75705520028102,
                ServiceProviderId = sp.Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Target",
                StoreNumber = "666",
                Email = "",
                PhonePrimary = "(404) 237-9494",
                StreetAddress1 = "3535 Peachtree Rd NE",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30326",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.35963763075851,
                Latitude = 33.85180066254931,
                ServiceProviderId = sp.Id
            });

            await _dbContext.Stores.AddRangeAsync(stores);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SeedAssets()
        {
            // if (_dbContext.Assets.Any()) return;

            var assets = new List<AssetModel>();

            var stores = new List<StoreModel>();

            //Console.WriteLine($"stores len: {result.Stores.Count()}");

            if (stores != null)
            {
                //foreach (var store in stores)
                //{
                //    for (int i = 0; i < 5; i++)
                //    {
                //        assets.Add(new AssetModel
                //        {
                //            CreatedAt = DateTime.UtcNow,
                //            StoreId = store.Id,
                //            IsProvisioned = true,
                //            Status = AssetStatus.InsideGeofence,
                //            Condition = AssetCondition.Good,
                //            OrderedFrom = AssetOrderedFrom.Manufacture
                //        });
                //    }
                //}

                Console.WriteLine($"assets len: {assets.Count()}");

                try
                {

                    var asset1 = new AssetModel
                    {
                        CreatedAt = DateTime.UtcNow,
                        StoreId = Guid.Parse("22fbb4a4-4086-4f22-288d-08d9c01a80f1"),
                        IsProvisioned = true,
                        Status = AssetStatus.InsideGeofence,
                        Condition = AssetCondition.Good,
                        OrderedFrom = AssetOrderedFrom.Manufacture
                    };

                    await _assetRepository.CreateAssetAsync(asset1);

                    //await _dbContext.Assets.AddAsync(asset1);

                    //await _dbContext.Assets.AddRangeAsync(assets);
                    //await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
               
            }

        }

    //    public async Task SeedTrackingDevices()
    //    {
    //        if (_dbContext.Assets.Any()) return;

    //        var sp = _dbContext.ServiceProviders.FirstOrDefault();
    //        var assets = _dbContext.Assets;

    //        var devices = new List<TrackingDeviceModel>();

    //        foreach (var asset in assets)
    //        {
    //            devices.Add(new TrackingDeviceModel
    //            {
    //                CreatedAt = DateTime.UtcNow,
    //                ServiceProviderId = sp.Id,
    //                AssetId = asset.Id
    //            });
    //        }

    //        await _dbContext.TrackingDevices.AddRangeAsync(devices);
    //        await _dbContext.SaveChangesAsync();
    //    }

    //    public async Task SeedDrivers()
    //    {
    //        if (_dbContext.Drivers.Any()) return;

    //        var sp = _dbContext.ServiceProviders.FirstOrDefault();

    //        var drivers = new List<DriverModel>();

    //        string firstName = Faker.Name.FirstName();
    //        string lastName = Faker.Name.LastName();
    //        drivers.Add(new DriverModel
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            FirstName = firstName,
    //            LastName = lastName,
    //            Email = $"{firstName}{lastName}@fake.com",
    //            ProfilePic = $"https://avatars.dicebear.com/api/avataaars/{firstName}.svg",
    //            ServiceProviderId = sp.Id
    //        });

    //        firstName = Faker.Name.FirstName();
    //        lastName = Faker.Name.LastName();
    //        drivers.Add(new DriverModel
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            FirstName = firstName,
    //            LastName = lastName,
    //            Email = $"{firstName}{lastName}@fake.com",
    //            ProfilePic = $"https://avatars.dicebear.com/api/avataaars/{firstName}.svg",
    //            ServiceProviderId = sp.Id
    //        });

    //        await _dbContext.Drivers.AddRangeAsync(drivers);
    //        await _dbContext.SaveChangesAsync();
    //    }

    //    public async Task SeedTechnicians()
    //    {
    //        if (_dbContext.Technicians.Any()) return;

    //        var sp = _dbContext.ServiceProviders.FirstOrDefault();

    //        var techs = new List<TechnicianModel>();

    //        string firstName = Faker.Name.FirstName();
    //        string lastName = Faker.Name.LastName();
    //        techs.Add(new TechnicianModel
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            FirstName = firstName,
    //            LastName = lastName,
    //            Email = $"{firstName}{lastName}@fake.com",
    //            ProfilePic = $"https://avatars.dicebear.com/api/avataaars/{firstName}.svg",
    //            ServiceProviderId = sp.Id
    //        });

    //        firstName = Faker.Name.FirstName();
    //        lastName = Faker.Name.LastName();
    //        techs.Add(new TechnicianModel
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            FirstName = firstName,
    //            LastName = lastName,
    //            Email = $"{firstName}{lastName}@fake.com",
    //            ProfilePic = $"https://avatars.dicebear.com/api/avataaars/{firstName}.svg",
    //            ServiceProviderId = sp.Id
    //        });

    //        await _dbContext.Technicians.AddRangeAsync(techs);
    //        await _dbContext.SaveChangesAsync();

    //    }


    //    public async Task SeedDriverStores()
    //    {
    //        if (_dbContext.DriverStores.Any()) return;

    //        var resultStores = await _storeRepository.GetAllStoresAsync();
    //        if (!resultStores.IsSuccess) return;

    //        var resultDrivers = await _driverRepository.GetAllDriversAsync();
    //        if (!resultDrivers.IsSuccess) return;

    //        var driverStores = new List<DriverStore>();

    //        driverStores.Add(new DriverStore
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            DriverId = resultDrivers.Drivers[0].Id,
    //            StoreId = resultStores.Stores[0].Id
    //        });

    //        driverStores.Add(new DriverStore
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            DriverId = resultDrivers.Drivers[0].Id,
    //            StoreId = resultStores.Stores[1].Id
    //        });

    //        driverStores.Add(new DriverStore
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            DriverId = resultDrivers.Drivers[0].Id,
    //            StoreId = resultStores.Stores[2].Id
    //        });

    //        driverStores.Add(new DriverStore
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            DriverId = resultDrivers.Drivers[1].Id,
    //            StoreId = resultStores.Stores[3].Id
    //        });

    //        driverStores.Add(new DriverStore
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            DriverId = resultDrivers.Drivers[1].Id,
    //            StoreId = resultStores.Stores[4].Id
    //        });

    //        driverStores.Add(new DriverStore
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            DriverId = resultDrivers.Drivers[1].Id,
    //            StoreId = resultStores.Stores[5].Id
    //        });

    //        await _dbContext.DriverStores.AddRangeAsync(driverStores);
    //        await _dbContext.SaveChangesAsync();

    //    }

    //    public async Task SeedTechniciansStores()
    //    {
    //        if (_dbContext.TechnicianStores.Any()) return;

    //        var resultStores = await _storeRepository.GetAllStoresAsync();
    //        if (!resultStores.IsSuccess) return;

    //        var resultTechs = await _technicianRepository.GetAllTechniciansAsync();
    //        if (!resultTechs.IsSuccess) return;

    //        var techStores = new List<TechnicianStore>();

    //        techStores.Add(new TechnicianStore
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            TechnicianId = resultTechs.Technicians[0].Id,
    //            StoreId = resultStores.Stores[0].Id
    //        });

    //        techStores.Add(new TechnicianStore
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            TechnicianId = resultTechs.Technicians[0].Id,
    //            StoreId = resultStores.Stores[1].Id
    //        });

    //        techStores.Add(new TechnicianStore
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            TechnicianId = resultTechs.Technicians[0].Id,
    //            StoreId = resultStores.Stores[2].Id
    //        });

    //        techStores.Add(new TechnicianStore
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            TechnicianId = resultTechs.Technicians[1].Id,
    //            StoreId = resultStores.Stores[3].Id
    //        });

    //        techStores.Add(new TechnicianStore
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            TechnicianId = resultTechs.Technicians[1].Id,
    //            StoreId = resultStores.Stores[4].Id
    //        });

    //        techStores.Add(new TechnicianStore
    //        {
    //            CreatedAt = DateTime.UtcNow,
    //            TechnicianId = resultTechs.Technicians[1].Id,
    //            StoreId = resultStores.Stores[5].Id
    //        });

    //        await _dbContext.TechnicianStores.AddRangeAsync(techStores);
    //        await _dbContext.SaveChangesAsync();
    //    }
    }
}
