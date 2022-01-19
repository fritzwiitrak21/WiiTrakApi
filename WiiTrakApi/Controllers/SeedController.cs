using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly ICartRepository _repository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly ITechnicianRepository _technicianRepository;
        private readonly IServiceProviderRepository _serviceProviderRepository;
        private readonly ICorporateRepository _corporateRepository;
        private readonly ApplicationDbContext _dbContext;

        public SeedController(ICartRepository repository, 
            IStoreRepository storeRepository,
            IDriverRepository driverRepository,
            ITechnicianRepository technicianRepository,
            ApplicationDbContext dbContext, IServiceProviderRepository serviceProviderRepository, ICompanyRepository companyRepository, ICorporateRepository corporateRepository)
        {
            _repository = repository;
            _storeRepository = storeRepository;
            _driverRepository = driverRepository;
            _technicianRepository = technicianRepository;
            _dbContext = dbContext;
            _serviceProviderRepository = serviceProviderRepository;
            _companyRepository = companyRepository;
            _corporateRepository = corporateRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //await SeedSystemOwner();
            //await SeedCompanys();
            //await SeedCorporates();
            //await SeedCompanyCorporates();
            //await SeedServiceProviders();
            //await SeedStores();
            //await SeedCarts();
            //await SeedTrackingDevices();
            //await SeedDrivers();
            //await SeedTechnicians();
            //await SeedDriverStores();
            //await SeedRepairIssues();
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
                Name = "VeVa, INC",
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
                SystemOwnerId = sysOwner.Id,
                ParentId = null
            };

            await _companyRepository.CreateCompanyAsync(company1);

            var result = await _companyRepository
                .GetCompaniesByConditionAsync(x => x.ParentId == null);

            var primaryCompany = result.Companies[0];

            var company2 = new CompanyModel
            {
                Name = "Dave's Cart Logistics, LLC",
                StreetAddress1 = $"{Faker.Number.RandomNumber(111, 999)} {Faker.Address.StreetName()}",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30312",
                CountryCode = "US",
                ProfilePicUrl =
                    "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/logoipsum-logo-6.svg",
                Email = "info@davescartlogistics.com",
                PhonePrimary = Faker.Phone.GetPhoneNumber(),
                CreatedAt = DateTime.UtcNow,
                ParentId = primaryCompany.Id
            };

            var company3 = new CompanyModel
            {
                Name = "Speedy Cart Services, LLC",
                StreetAddress1 = $"{Faker.Number.RandomNumber(111, 999)} {Faker.Address.StreetName()}",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30345",
                CountryCode = "US",
                ProfilePicUrl =
                    "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/logoipsum-logo-7.svg",
                Email = "info@speedycartservices.com",
                PhonePrimary = Faker.Phone.GetPhoneNumber(),
                CreatedAt = DateTime.UtcNow,
                ParentId = primaryCompany.Id
            };

            await _companyRepository.CreateCompanyAsync(company2);
            await _companyRepository.CreateCompanyAsync(company3);
        }

        public async Task SeedCorporates()
        {
            var corporate1 = new CorporateModel
            {
                Name = "Kroger, INC",
                StreetAddress1 = $"{Faker.Number.RandomNumber(111, 999)} {Faker.Address.StreetName()}",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30312",
                CountryCode = "US",
                ProfilePicUrl =
                 "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/logoipsum-logo-2.svg",
                Email = "info@kroger.com",
                PhonePrimary = Faker.Phone.GetPhoneNumber(),
                CreatedAt = DateTime.UtcNow,
            };

            var corporate2 = new CorporateModel
            {
                Name = "Target, INC",
                StreetAddress1 = $"{Faker.Number.RandomNumber(111, 999)} {Faker.Address.StreetName()}",
                City = "Atlanta",
                State = "GA",
                PostalCode = "30322",
                CountryCode = "US",
                ProfilePicUrl =
                    "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/logoipsum-logo-3.svg",
                Email = "info@target.com",
                PhonePrimary = Faker.Phone.GetPhoneNumber(),
                CreatedAt = DateTime.UtcNow,
            };

            await _corporateRepository.CreateCorporateAsync(corporate1);
            await _corporateRepository.CreateCorporateAsync(corporate2);

        }

        public async Task SeedCompanyCorporates()
        {
            if (_dbContext.CompanyCorporates.Any()) return;

            var result = await _companyRepository
                .GetCompaniesByConditionAsync(x => x.ParentId == null);

            var primaryCompany = result.Companies[0];

            var resultCorporates = await _corporateRepository.GetAllCorporatesAsync();

            foreach (var corporate in resultCorporates.Corporates)
            {
                var companyCorporate = new CompanyCorporateModel
                {
                    CompanyId = primaryCompany.Id,
                    CorporateId = corporate.Id,
                    CreatedAt = DateTime.Now
                }; 

                _dbContext.CompanyCorporates.Add(companyCorporate);
            }

            await _dbContext.SaveChangesAsync();

        }

        public async Task SeedServiceProviders()
        {
            if (_dbContext.ServiceProviders.Any()) return;

            var result = await _companyRepository.GetCompaniesByConditionAsync(x => x.ParentId != null);

            var company1Id = result.Companies[0].Id;
            var company2Id = result.Companies[1].Id;

            var serviceProvider1 = new ServiceProviderModel
            {
                ServiceProviderName = "North Georgia Division",
                Email = "info@cartlogistics.com",
                PhonePrimary = Faker.Phone.GetPhoneNumber(),
                LogoPicUrl = "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/logoipsum-logo-9.svg",
                CreatedAt = DateTime.UtcNow,
                CompanyId = company1Id
            };

            var serviceProvider2 = new ServiceProviderModel
            {
                ServiceProviderName = "Atlanta Division",
                Email = "info@atldivision.com",
                PhonePrimary = Faker.Phone.GetPhoneNumber(),
                LogoPicUrl = "https://wiitrakstorage.blob.core.windows.net/wiitrakblobcontainer/logoipsum-logo-10.svg",
                CreatedAt = DateTime.UtcNow,
                CompanyId = company2Id
            };

            await _serviceProviderRepository.CreateServiceProviderAsync(serviceProvider1);
            await _serviceProviderRepository.CreateServiceProviderAsync(serviceProvider2);
        }

        public async Task SeedStores()
        {
            if (_dbContext.Stores.Any()) return;

            // 1 to many with prime company

            var serviceProviders = await _dbContext.ServiceProviders.ToListAsync();
            var sp1Id = serviceProviders[0].Id;
            var sp2Id = serviceProviders[1].Id;

            var result = await _companyRepository
                .GetCompaniesByConditionAsync(x => x.ParentId == null);

            var primaryCompany = result.Companies[0];


            var resultCorporate = await _corporateRepository.GetAllCorporatesAsync();
            var corporate1 = resultCorporate.Corporates[0];
            var corporate2 = resultCorporate.Corporates[2];

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
                CompanyId = primaryCompany.Id,
                CorporateId = corporate1.Id
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
                ServiceProviderId = sp1Id,
                CompanyId = primaryCompany.Id,
                CorporateId = corporate1.Id
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
                ServiceProviderId = sp1Id,
                CompanyId = primaryCompany.Id,
                CorporateId = corporate1.Id
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
                ServiceProviderId = sp1Id,
                CompanyId = primaryCompany.Id,
                CorporateId = corporate1.Id
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
                ServiceProviderId = sp2Id,
                CompanyId = primaryCompany.Id,
                CorporateId = corporate1.Id
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
                ServiceProviderId = sp2Id,
                CompanyId = primaryCompany.Id,
                CorporateId = corporate1.Id
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
                ServiceProviderId = sp2Id,
                CompanyId = primaryCompany.Id,
                CorporateId = corporate2.Id
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
                ServiceProviderId = sp1Id,
                CompanyId = primaryCompany.Id,
                CorporateId = corporate2.Id
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
                ServiceProviderId = sp1Id,
                CompanyId = primaryCompany.Id,
                CorporateId = corporate2.Id
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
                ServiceProviderId = sp1Id,
                CompanyId = primaryCompany.Id,
                CorporateId = corporate2.Id
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
                ServiceProviderId = sp1Id,
                CompanyId = primaryCompany.Id,
                CorporateId = corporate2.Id
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
                ServiceProviderId = sp2Id,
                CompanyId = primaryCompany.Id,
                CorporateId = corporate2.Id
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
                ServiceProviderId = sp2Id,
                CompanyId = primaryCompany.Id,
                CorporateId = corporate2.Id
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
                ServiceProviderId = sp2Id,
                CompanyId = primaryCompany.Id,
                CorporateId = corporate2.Id
            });

           

            await _dbContext.Stores.AddRangeAsync(stores);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SeedCarts()
        {
            // if (_dbContext.Carts.Any()) return;

            var carts = new List<CartModel>();

            var stores = _dbContext.Stores;

            //Console.WriteLine($"stores len: {result.Stores.Count()}");

            if (stores != null)
            {
                foreach (var store in stores)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        carts.Add(new CartModel
                        {
                            CreatedAt = DateTime.UtcNow,
                            StoreId = store.Id,
                            IsProvisioned = true,
                            Status = CartStatus.InsideGeofence,
                            Condition = CartCondition.Good,
                            OrderedFrom = CartOrderedFrom.Manufacture
                        });
                    }
                }

                Console.WriteLine($"carts len: {carts.Count()}");

                try
                {
                    await _dbContext.Carts.AddRangeAsync(carts);
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
            var carts = _dbContext.Carts;

            var devices = new List<TrackingDeviceModel>();

            foreach (var cart in carts)
            {
                devices.Add(new TrackingDeviceModel
                {
                    CreatedAt = DateTime.UtcNow,
                    SystemOwnerId = systemOwner.Id,
                    CartId = cart.Id
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

            var result = await _companyRepository.GetCompaniesByConditionAsync(x => x.ParentId != null);

            var company1Id = result.Companies[0].Id;
            var company2Id = result.Companies[1].Id;

            var resultStoresFromCompany1 = await _storeRepository
                .GetStoresByConditionAsync(x => x.ServiceProvider != null && x.ServiceProvider.CompanyId == company1Id);


            var resultStoresFromCompany2 = await _storeRepository
                .GetStoresByConditionAsync(x => x.ServiceProvider != null && x.ServiceProvider.CompanyId == company2Id);


            var resultDrivers1 =
                await _driverRepository.GetDriversByConditionAsync(x => x.CompanyId == company1Id);

            var resultDrivers2 =
                await _driverRepository.GetDriversByConditionAsync(x => x.CompanyId == company2Id);

            var driverStores = new List<DriverStoreModel>();

            var drivers = await _dbContext.Drivers.ToListAsync();
 
            // 3 drivers seeded
            // randomly select driver 1 and 2 for first stores

            foreach (var store in resultStoresFromCompany1.Stores)
            {
                driverStores.Add(new DriverStoreModel
                {
                    CreatedAt = DateTime.UtcNow,
                    DriverId = drivers[Faker.Number.RandomNumber(2)].Id,
                    StoreId = store.Id
                });
            }

            foreach (var store in resultStoresFromCompany2.Stores)
            {
                driverStores.Add(new DriverStoreModel
                {
                    CreatedAt = DateTime.UtcNow,
                    DriverId = drivers[2].Id,
                    StoreId = store.Id
                });
            }



            await _dbContext.DriverStores.AddRangeAsync(driverStores);
            await _dbContext.SaveChangesAsync();

        }

        public async Task SeedRepairIssues()
        {
            if (_dbContext.RepairIssues.Any()) return;

            string[] repairIssues = new[]
            {
                "Broke Wheel", "Broke Flap","Damaged Child Seat","Damaged Safety Belt"
            };

            foreach (var issue in repairIssues)
            {
                _dbContext.RepairIssues.Add( new RepairIssueModel
                    {
                        CreatedAt = DateTime.Now,
                        Issue = issue
                    });
            }

            await _dbContext.SaveChangesAsync();
        }
    }

}
