using WiiTrakApi.Enums;
using WiiTrakApi.Models;
using WiiTrakApi.Repository.Contracts;
using WiiTrakApi.Services.Contracts;

namespace WiiTrakApi.Services
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ITrackingDeviceRepository _trackingDeviceRepository;

        public BackgroundJobService(ICartRepository cartRepository, 
            IStoreRepository storeRepository, 
            ITrackingDeviceRepository trackingDeviceRepository)
        {
            _cartRepository = cartRepository;
            _storeRepository = storeRepository;
            _trackingDeviceRepository = trackingDeviceRepository;
        }

        public async Task ResetCartData()
        {
            var result = await _storeRepository.GetAllStoresAsync();

            
            if (!result.IsSuccess || result.Stores is null) return;
            foreach (var store in result.Stores)
            {
                await ResetCartDataHelper(store);
            }
        }

        private async Task ResetCartDataHelper(StoreModel store)
        {
            GeoLocationPoint[] locations;

            switch (store.Longitude)
            {
                case -84.3633590557509:
                    locations = new[]
                    {
                        new GeoLocationPoint
                        {
                            Latitude = 33.772790531571935,
                            Longitude = -84.36262954109523
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.77260324645141,
                            Longitude = -84.364710935225
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.77062335013737,
                            Longitude = -84.36335910192423
                        },
                    };
                    await SetStatusAndGeolocation(store, locations);
                    break;
                case -84.359912885816883:
                    locations = new[]
                    {
                        new GeoLocationPoint
                        {
                            Latitude = 33.74286928294373,
                            Longitude = -84.36196210954728
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.741994977098976,
                            Longitude = -84.35851815328101
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.74127232982676,
                            Longitude = -84.36142566776125
                        },
                    };
                    await SetStatusAndGeolocation(store, locations);
                    break;
                case -84.40935984686935:
                    locations = new[]
                    {
                        new GeoLocationPoint
                        {
                            Latitude = 33.68408762739265,
                            Longitude = -84.40988551487608
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.68148074265096,
                            Longitude = -84.40909158103277
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.68281097796149,
                            Longitude = -84.40710674642446
                        },
                    };
                    await SetStatusAndGeolocation(store, locations);
                    break;
                case -84.198281930061455:
                    locations = new[]
                    {
                        new GeoLocationPoint
                        {
                            Latitude = 33.91685020805501,
                            Longitude = -84.19798149452153
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.913876452182215,
                            Longitude = -84.19846429212895
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.91488255485646,
                            Longitude = -84.19640435567061
                        },
                    };
                    await SetStatusAndGeolocation(store, locations);
                    break;
                case -84.251153632491935:
                    locations = new[]
                    {
                        new GeoLocationPoint
                        {
                            Latitude = 33.85331034935219,
                            Longitude = -84.25209773946618
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.855591263244285,
                            Longitude = -84.2500163453364
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.85268665135504,
                            Longitude = -84.24979103978627
                        },
                    };
                    await SetStatusAndGeolocation(store, locations);
                    break;
                case -84.065759351242278:
                    locations = new[]
                    {
                        new GeoLocationPoint
                        {
                            Latitude = 33.89487396008784,
                            Longitude = -84.06403205937107
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.893039386572944,
                            Longitude = -84.06477234903579
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.893217502236176,
                            Longitude = -84.06772277885892
                        },
                    };
                    await SetStatusAndGeolocation(store, locations);
                    break;
                case -84.036233595339553:
                    locations = new[]
                    {
                        new GeoLocationPoint
                        {
                            Latitude = 33.836736222150115,
                            Longitude = -84.03670567069476
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.83967702179082,
                            Longitude = -84.03663056884471
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.83822445764577,
                            Longitude = -84.03355139299296
                        },
                    };
                    await SetStatusAndGeolocation(store, locations);
                    break;
                case -84.399390346102436:
                    locations = new[]
                    {
                        new GeoLocationPoint
                        {
                            Latitude = 33.79324102450625,
                            Longitude = -84.40119274632492
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.79406131097396,
                            Longitude = -84.39754494217996
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.79255447435531,
                            Longitude = -84.39786680725157
                        },
                    };
                    await SetStatusAndGeolocation(store, locations);
                    break;
                case -84.345988801924491:
                    locations = new[]
                    {
                        new GeoLocationPoint
                        {
                            Latitude = 33.75751009779856,
                            Longitude = -84.34435801889492
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.755931257487944,
                            Longitude = -84.34521632575257
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.75778661486064,
                            Longitude = -84.3471797026894
                        },
                    };
                    await SetStatusAndGeolocation(store, locations);
                    break;
                case -84.359637630758513:
                    locations = new[]
                    {
                        new GeoLocationPoint
                        {
                            Latitude = 33.852353104851055,
                            Longitude = -84.35829649745776
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.85125716305919,
                            Longitude = -84.35803900540047
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.85224618431962,
                            Longitude = -84.36096797755216
                        },
                    };
                    await SetStatusAndGeolocation(store, locations);
                    break;
                case -84.019754396298723:
                    locations = new[]
                    {
                        new GeoLocationPoint
                        {
                            Latitude = 33.64521784731242,
                            Longitude = -84.01853131465428
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.64324394399913,
                            Longitude = -84.01908921411173
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.645271436816586,
                            Longitude = -84.01988314795504
                        },
                    };
                    await SetStatusAndGeolocation(store, locations);
                    break;
                case -84.482553453936745:
                    locations = new[]
                    {
                        new GeoLocationPoint
                        {
                            Latitude = 33.90574040440053,
                            Longitude = -84.48286463815775
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.907913066414075,
                            Longitude = -84.48155572019985
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.90691578580798,
                            Longitude = -84.48422720029426
                        },
                    };
                    await SetStatusAndGeolocation(store, locations);
                    break;
                case -84.501092882061712:
                    locations = new[]
                    {
                        new GeoLocationPoint
                        {
                            Latitude = 33.66980301634978,
                            Longitude = -84.4998912723253
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.66876724201434,
                            Longitude = -84.50071739267577
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.669731584037415,
                            Longitude = -84.5027773291341
                        },
                    };
                    await SetStatusAndGeolocation(store, locations);
                    break;
                case -84.236047724423315:
                    locations = new[]
                    {
                        new GeoLocationPoint
                        {
                            Latitude = 33.964632069475726,
                            Longitude = -84.23498554718456
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.96376892905148,
                            Longitude = -84.23426671519128
                        },
                        new GeoLocationPoint
                        {
                            Latitude = 33.96336850010727,
                            Longitude = -84.23736734871451
                        },
                    };
                    await SetStatusAndGeolocation(store, locations);
                    break;
            }
        }

        private async Task SetStatusAndGeolocation(StoreModel store, GeoLocationPoint[] locations)
        {
            CartModel cart1;
            CartModel cart2;
            CartModel cart3;

            TrackingDeviceModel? device1 = null;
            TrackingDeviceModel? device2 = null;
            TrackingDeviceModel? device3 = null;

            if (store.Carts is null) return;
            cart1 = store.Carts[0];
            cart2 = store.Carts[1];
            cart3 = store.Carts[2];

            cart1.Status = CartStatus.OutsideGeofence;
            cart2.Status = CartStatus.OutsideGeofence;
            cart3.Status = CartStatus.OutsideGeofence;

            await _cartRepository.UpdateCartAsync(cart1);
            await _cartRepository.UpdateCartAsync(cart2);
            await _cartRepository.UpdateCartAsync(cart3);


            var result1 =
                await _trackingDeviceRepository.GetTrackingDevicesByConditionAsync(x => x.CartId == cart1.Id);
            if (result1.IsSuccess)
            {
                device1 = result1.TrackingDevices.First();
                device1.Latitude = locations[0].Latitude;
                device1.Longitude = locations[0].Longitude;
                await _trackingDeviceRepository.UpdateTrackingDeviceAsync(device1);
            }

            var result2 =
                await _trackingDeviceRepository.GetTrackingDevicesByConditionAsync(x => x.CartId == cart2.Id);
            if (result2.IsSuccess)
            {
                device2 = result2.TrackingDevices.First();
                device2.Latitude = locations[1].Latitude;
                device2.Longitude = locations[1].Longitude;
                await _trackingDeviceRepository.UpdateTrackingDeviceAsync(device2);
            }

            var result3 =
                await _trackingDeviceRepository.GetTrackingDevicesByConditionAsync(x => x.CartId == cart3.Id);
            if (result3.IsSuccess)
            {
                device3 = result3.TrackingDevices.First();
                device3.Latitude = locations[2].Latitude;
                device3.Longitude = locations[2].Longitude;
                await _trackingDeviceRepository.UpdateTrackingDeviceAsync(device3);
            }
        }
    }
}
