using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;
using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Repository;
using WiiTrakApi.Repository.Contracts;

namespace WiiTrakApi.Controllers
{
    [Route("api/seed")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly IAssetRepository _repository;
        private readonly ICompanyRepository _companyAccountRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly ITechnicianRepository _technicianRepository;
        private readonly IServiceProviderRepository _serviceProviderRepository;
        private readonly ApplicationDbContext _dbContext;

        public SeedController(IAssetRepository repository, 
            IStoreRepository storeRepository,
            IDriverRepository driverRepository,
            ITechnicianRepository technicianRepository,
            ApplicationDbContext dbContext, IServiceProviderRepository serviceProviderRepository, ICompanyRepository companyAccountRepository)
        {
            _repository = repository;
            _storeRepository = storeRepository;
            _driverRepository = driverRepository;
            _technicianRepository = technicianRepository;
            _dbContext = dbContext;
            _serviceProviderRepository = serviceProviderRepository;
            _companyAccountRepository = companyAccountRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //await SeedSystemOwner();
            //await SeedCompanys();
            //await SeedServiceProviders();
            //await SeedStores();
            //await SeedAssets();
            //await SeedTrackingDevices();
            //await SeedDrivers();
            //await SeedTechnicians();
            //await SeedDriverStores();
            return Ok();
        }

        public async Task SeedSystemOwner()
        {
            var sysOwner = new SystemOwnerModel
            {
                Name = "WiiTrak",
                StreetAddress1 = $"{Faker.Number.RandomNumber(111,999)} {Faker.Address.StreetName()}" ,
                City = "Atlanta",
                State = "GA",
                PostalCode = "30312",
                CountryCode = "US",
                ProfilePicUrl = "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/wiitrak_logo_200x101.png",
                Email = "info@wiitrak.com",
                PhonePrimary = Faker.Phone.GetPhoneNumber(),
                CreatedAt = DateTime.UtcNow,
            };

            await _dbContext.SystemOwners.AddAsync(sysOwner);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SeedCompanys()
        {
            var sysOwner = await _dbContext.SystemOwners.FirstOrDefaultAsync();

            var company1 = new CompanyModel
            {
                Name = "Dave's Cart Logistics, INC",
                StreetAddress1 = $"{Faker.Number.RandomNumber(111, 999)} {Faker.Address.StreetName()}",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30312",
                CountryCode = "US",
                ProfilePicUrl =
                    "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/logoipsum-logo-8.svg",
                Email = "info@davescartlogistics.com",
                PhonePrimary = Faker.Phone.GetPhoneNumber(),
                CreatedAt = DateTime.UtcNow,
                SystemOwnerId = sysOwner.Id
            };

            var company2 = new CompanyModel
            {
                Name = "ABC Cart Logistics, INC",
                StreetAddress1 = $"{Faker.Number.RandomNumber(111, 999)} {Faker.Address.StreetName()}",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30312",
                CountryCode = "US",
                ProfilePicUrl =
                    "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/logoipsum-logo-8.svg",
                Email = "info@abcscartlogistics.com",
                PhonePrimary = Faker.Phone.GetPhoneNumber(),
                CreatedAt = DateTime.UtcNow,
                SystemOwnerId = sysOwner.Id
            };

            await _companyAccountRepository.CreateCompanyAsync(company1);
            await _companyAccountRepository.CreateCompanyAsync(company2);
        }

        public async Task SeedServiceProviders()
        {
            if (_dbContext.ServiceProviders.Any()) return;

            var companies = await _dbContext.Companies.ToListAsync();

            var company1Id = companies[0].Id;
            var company2Id = companies[1].Id;

            var serviceProvider1 = new ServiceProviderModel
            {
                ServiceProviderName = "North Georgia Division",
                Email = "info@davescartlogistics.com",
                PhonePrimary = Faker.Phone.GetPhoneNumber(),
                LogoPicUrl = "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/logoipsum-logo-8.svg",
                CreatedAt = DateTime.UtcNow,
                CompanyId = company1Id
            };

            var serviceProvider2 = new ServiceProviderModel
            {
                ServiceProviderName = "Atlanta Cart Services",
                Email = "info@atlcartservices.com",
                PhonePrimary = Faker.Phone.GetPhoneNumber(),
                LogoPicUrl = "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/logoipsum-logo-8.svg",
                CreatedAt = DateTime.UtcNow,
                CompanyId = company2Id
            };

            await _serviceProviderRepository.CreateServiceProviderAsync(serviceProvider1);
            await _serviceProviderRepository.CreateServiceProviderAsync(serviceProvider2);
        }

        public async Task SeedStores()
        {
            if (_dbContext.Stores.Any()) return;

            var serviceProviders = await _dbContext.ServiceProviders.ToListAsync();
            var sp1Id = serviceProviders[0].Id;
            var sp2Id = serviceProviders[1].Id;

            var stores = new List<StoreModel>();

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Kroger",
                StoreNumber = Faker.Number.RandomNumber(111, 999).ToString(),
                Email = "info@kroger.com",
                PhonePrimary = Faker.Phone.GetShortPhoneNumber(),
                StreetAddress1 = "725 Ponce De Leon Ave NE",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30306",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.3633590557509,
                Latitude = 33.77174713602629,
                ServiceProviderId = sp1Id,
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Kroger",
                StoreNumber = Faker.Number.RandomNumber(111, 999).ToString(),
                Email = "info@kroger.com",
                PhonePrimary = Faker.Phone.GetShortPhoneNumber(),
                StreetAddress1 = "800 Glenwood Ave SE",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30316",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.35991288581688,
                Latitude = 33.74236966213573,
                ServiceProviderId = sp1Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Kroger",
                StoreNumber = Faker.Number.RandomNumber(111, 999).ToString(),
                Email = "info@kroger.com",
                PhonePrimary = Faker.Phone.GetShortPhoneNumber(),
                StreetAddress1 = "2685 Metropolitan Pkwy SW",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30315",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.40935984686935,
                Latitude = 33.682489645464656,
                ServiceProviderId = sp1Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Kroger",
                StoreNumber = Faker.Number.RandomNumber(111, 999).ToString(),
                Email = "info@kroger.com",
                PhonePrimary = Faker.Phone.GetShortPhoneNumber(),
                StreetAddress1 = "6050 Singleton Rd",
                City = "Norcross",
                State = "GA",
                PostalCode = "30093",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.19828193006146,
                Latitude = 33.91520307859206,
                ServiceProviderId = sp1Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Kroger",
                StoreNumber = Faker.Number.RandomNumber(111, 999).ToString(),
                Email = "info@kroger.com",
                PhonePrimary = Faker.Phone.GetShortPhoneNumber(),
                StreetAddress1 = "3959 Lavista Rd Suite A",
                City = "Tucker",
                State = "GA",
                PostalCode = "30084",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.25115363249193,
                Latitude = 33.85306979166759,
                ServiceProviderId = sp2Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Kroger",
                StoreNumber = Faker.Number.RandomNumber(111, 999).ToString(),
                Email = "info@kroger.com",
                PhonePrimary = Faker.Phone.GetShortPhoneNumber(),
                StreetAddress1 = "3050 Five Forks Trickum Rd SW",
                City = "Lilburn",
                State = "GA",
                PostalCode = "30047",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.06575935124228,
                Latitude = 33.8941170361337,
                ServiceProviderId = sp2Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Kroger",
                StoreNumber = Faker.Number.RandomNumber(111, 999).ToString(),
                Email = "info@kroger.com",
                PhonePrimary = Faker.Phone.GetShortPhoneNumber(),
                StreetAddress1 = "3035 Scenic Hwy S Suite 19",
                City = "Snellville",
                State = "GA",
                PostalCode = "30039",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.03623359533955,
                Latitude = 33.83824232501002,
                ServiceProviderId = sp2Id
            });

            

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Target",
                StoreNumber = Faker.Number.RandomNumber(111, 999).ToString(),
                Email = "info@target.com",
                PhonePrimary = Faker.Phone.GetShortPhoneNumber(),
                StreetAddress1 = "375 18th St NW",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30363",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.39939034610244,
                Latitude = 33.79343715586747,
                ServiceProviderId = sp1Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Target",
                StoreNumber = Faker.Number.RandomNumber(111, 999).ToString(),
                Email = "info@target.com",
                PhonePrimary = Faker.Phone.GetShortPhoneNumber(),
                StreetAddress1 = "1275 Caroline St NE",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30307",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.34598880192449,
                Latitude = 33.75705520028102,
                ServiceProviderId = sp1Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Target",
                StoreNumber = Faker.Number.RandomNumber(111, 999).ToString(),
                Email = "info@target.com",
                PhonePrimary = Faker.Phone.GetShortPhoneNumber(),
                StreetAddress1 = "3535 Peachtree Rd NE",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30326",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.35963763075851,
                Latitude = 33.85180066254931,
                ServiceProviderId = sp1Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Target",
                StoreNumber = Faker.Number.RandomNumber(111, 999).ToString(),
                Email = "info@target.com",
                PhonePrimary = Faker.Phone.GetShortPhoneNumber(),
                StreetAddress1 = "2195 GA-20 SE",
                City = "Conyers",
                State = "GA",
                PostalCode = "30013",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.01975439629872,
                Latitude = 33.644521245898076,
                ServiceProviderId = sp1Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Target",
                StoreNumber = Faker.Number.RandomNumber(111, 999).ToString(),
                Email = "info@target.com",
                PhonePrimary = Faker.Phone.GetShortPhoneNumber(),
                StreetAddress1 = "2201 Cobb Pkwy SE",
                City = "Smyrna",
                State = "GA",
                PostalCode = "30080",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.48255345393675,
                Latitude = 33.90649734182283,
                ServiceProviderId = sp2Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Target",
                StoreNumber = Faker.Number.RandomNumber(111, 999).ToString(),
                Email = "info@target.com",
                PhonePrimary = Faker.Phone.GetShortPhoneNumber(),
                StreetAddress1 = "3660 Marketplace Blvd",
                City = "East Point",
                State = "GA",
                PostalCode = "30344",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.50109288206171,
                Latitude = 33.669669123428555,
                ServiceProviderId = sp2Id
            });

            stores.Add(new StoreModel
            {
                CreatedAt = DateTime.UtcNow,
                StoreName = "Target",
                StoreNumber = Faker.Number.RandomNumber(111, 999).ToString(),
                Email = "info@target.com",
                PhonePrimary = Faker.Phone.GetShortPhoneNumber(),
                StreetAddress1 = "3200 Holcomb Bridge Rd",
                City = "Peachtree Corners",
                State = "GA",
                PostalCode = "30092",
                CountryCode = "US",
                ProfilePicUrl = "",
                Longitude = -84.23604772442332,
                Latitude = 33.96403586167325,
                ServiceProviderId = sp2Id
            });

           

            await _dbContext.Stores.AddRangeAsync(stores);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SeedAssets()
        {
            // if (_dbContext.Assets.Any()) return;

            var assets = new List<AssetModel>();

            var stores = _dbContext.Stores;

            //Console.WriteLine($"stores len: {result.Stores.Count()}");

            if (stores != null)
            {
                foreach (var store in stores)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        assets.Add(new AssetModel
                        {
                            CreatedAt = DateTime.UtcNow,
                            StoreId = store.Id,
                            IsProvisioned = true,
                            Status = AssetStatus.InsideGeofence,
                            Condition = AssetCondition.Good,
                            OrderedFrom = AssetOrderedFrom.Manufacture
                        });
                    }
                }

                Console.WriteLine($"assets len: {assets.Count()}");

                try
                {
                    await _dbContext.Assets.AddRangeAsync(assets);
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

        }

        public async Task SeedTrackingDevices()
        {
            if (_dbContext.TrackingDevices.Any()) return;

            var systemOwner = _dbContext.SystemOwners.FirstOrDefault();
            var assets = _dbContext.Assets;

            var devices = new List<TrackingDeviceModel>();

            foreach (var asset in assets)
            {
                devices.Add(new TrackingDeviceModel
                {
                    CreatedAt = DateTime.UtcNow,
                    SystemOwnerId = systemOwner.Id,
                    AssetId = asset.Id
                });
            }

            await _dbContext.TrackingDevices.AddRangeAsync(devices);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SeedDrivers()
        {
            if (_dbContext.Drivers.Any()) return;

            var companies = await _dbContext.Companies.ToListAsync();

            var company1Id = companies[0].Id;
            var company2Id = companies[1].Id;

            var drivers = new List<DriverModel>();

            string firstName = Faker.Name.FirstName();
            string lastName = Faker.Name.LastName();
            drivers.Add(new DriverModel
            {
                CreatedAt = DateTime.UtcNow,
                FirstName = firstName,
                LastName = lastName,
                Email = $"{firstName}{lastName}@fake.com",
                ProfilePic = $"https://avatars.dicebear.com/api/avataaars/{firstName}.svg",
                CompanyId = company1Id
            });

            firstName = Faker.Name.FirstName();
            lastName = Faker.Name.LastName();
            drivers.Add(new DriverModel
            {
                CreatedAt = DateTime.UtcNow,
                FirstName = firstName,
                LastName = lastName,
                Email = $"{firstName}{lastName}@fake.com",
                ProfilePic = $"https://avatars.dicebear.com/api/avataaars/{firstName}.svg",
                CompanyId = company1Id
            });

            firstName = Faker.Name.FirstName();
            lastName = Faker.Name.LastName();
            drivers.Add(new DriverModel
            {
                CreatedAt = DateTime.UtcNow,
                FirstName = firstName,
                LastName = lastName,
                Email = $"{firstName}{lastName}@fake.com",
                ProfilePic = $"https://avatars.dicebear.com/api/avataaars/{firstName}.svg",
                CompanyId = company2Id
            });

            await _dbContext.Drivers.AddRangeAsync(drivers);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SeedTechnicians()
        {
            if (_dbContext.Technicians.Any()) return;

            var sysOwner = await _dbContext.SystemOwners.FirstOrDefaultAsync();

            var techs = new List<TechnicianModel>();

            string firstName = Faker.Name.FirstName();
            string lastName = Faker.Name.LastName();
            techs.Add(new TechnicianModel
            {
                CreatedAt = DateTime.UtcNow,
                FirstName = firstName,
                LastName = lastName,
                Email = $"{firstName}{lastName}@fake.com",
                ProfilePic = $"https://avatars.dicebear.com/api/avataaars/{firstName}.svg",
                SystemOwnerId = sysOwner.Id
            });

            firstName = Faker.Name.FirstName();
            lastName = Faker.Name.LastName();
            techs.Add(new TechnicianModel
            {
                CreatedAt = DateTime.UtcNow,
                FirstName = firstName,
                LastName = lastName,
                Email = $"{firstName}{lastName}@fake.com",
                ProfilePic = $"https://avatars.dicebear.com/api/avataaars/{firstName}.svg",
                SystemOwnerId = sysOwner.Id
            });

            await _dbContext.Technicians.AddRangeAsync(techs);
            await _dbContext.SaveChangesAsync();

        }


        public async Task SeedDriverStores()
        {
            if (_dbContext.DriverStores.Any()) return;

            var companies = await _dbContext.Companies.ToListAsync();

            var company1Id = companies[0].Id;
            var company2Id = companies[1].Id;

            var resultStoresFromCompany1 = await _storeRepository
                .GetStoresByConditionAsync(x => x.ServiceProvider != null && x.ServiceProvider.CompanyId == company1Id);


            var resultStoresFromCompany2 = await _storeRepository
                .GetStoresByConditionAsync(x => x.ServiceProvider != null && x.ServiceProvider.CompanyId == company2Id);


            var resultDrivers1 =
                await _driverRepository.GetDriversByConditionAsync(x => x.CompanyId == company1Id);

            var resultDrivers2 =
                await _driverRepository.GetDriversByConditionAsync(x => x.CompanyId == company2Id);

            var driverStores = new List<DriverStore>();

            var drivers = await _dbContext.Drivers.ToListAsync();
 
            // 3 drivers seeded
            // randomly select driver 1 and 2 for first stores

            foreach (var store in resultStoresFromCompany1.Stores)
            {
                driverStores.Add(new DriverStore
                {
                    CreatedAt = DateTime.UtcNow,
                    DriverId = drivers[Faker.Number.RandomNumber(2)].Id,
                    StoreId = store.Id
                });
            }

            foreach (var store in resultStoresFromCompany2.Stores)
            {
                driverStores.Add(new DriverStore
                {
                    CreatedAt = DateTime.UtcNow,
                    DriverId = drivers[2].Id,
                    StoreId = store.Id
                });
            }



            await _dbContext.DriverStores.AddRangeAsync(driverStores);
            await _dbContext.SaveChangesAsync();

        }
    }

}
