using AutoMapper;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;

namespace WiiTrakApi.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            // Automapper profiles 

            CreateMap<DriverModel, DriverDto>().ReverseMap();
            CreateMap<DriverModel, DriverCreationDto>().ReverseMap();
            CreateMap<DriverModel, DriverUpdateDto>().ReverseMap();

            CreateMap<AssetModel, AssetDto>().ReverseMap();
            CreateMap<AssetModel, AssetCreationDto>().ReverseMap();
            CreateMap<AssetModel, AssetUpdateDto>().ReverseMap();

            CreateMap<ServiceProviderModel, ServiceProviderDto>().ReverseMap();
            CreateMap<ServiceProviderModel, ServiceProviderCreationDto>().ReverseMap();
            CreateMap<ServiceProviderModel, ServiceProviderUpdateDto>().ReverseMap();

            CreateMap<TechnicianModel, TechnicianDto>().ReverseMap();
            CreateMap<TechnicianModel, TechnicianCreationDto>().ReverseMap();
            CreateMap<TechnicianModel, TechnicianUpdateDto>().ReverseMap();

            CreateMap<StoreModel, StoreDto>().ReverseMap();
            CreateMap<StoreModel, StoreCreationDto>().ReverseMap();
            CreateMap<StoreModel, StoreUpdateDto>().ReverseMap();

            CreateMap<TrackingDeviceModel, TrackingDeviceDto>().ReverseMap();
            CreateMap<TrackingDeviceModel, TrackingDeviceCreationDto>().ReverseMap();
            CreateMap<TrackingDeviceModel, TrackingDeviceUpdateDto>().ReverseMap();

            CreateMap<CompanyModel, CompanyDto>().ReverseMap();
            CreateMap<CompanyModel, CompanyUpdateDto>().ReverseMap();
            CreateMap<CompanyModel, CompanyCreationDto>().ReverseMap();

            CreateMap<RepairIssueModel, RepairIssueDto>().ReverseMap();
            CreateMap<RepairIssueModel, RepairIssueUpdateDto>().ReverseMap();
            CreateMap<RepairIssueModel, RepairIssueCreationDto>().ReverseMap();

            CreateMap<SystemOwnerModel, SystemOwnerDto>().ReverseMap();

        }
    }
}
