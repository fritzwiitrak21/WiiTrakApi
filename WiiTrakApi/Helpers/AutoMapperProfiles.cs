/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using AutoMapper;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.SPModels;

namespace WiiTrakApi.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            // Automapper profiles 

            CreateMap<DriverModel, DriverDto>().ReverseMap();

            CreateMap<CartModel, CartDto>().ReverseMap();
            CreateMap<CartModel, CartCreationDto>().ReverseMap();
            CreateMap<CartModel, CartUpdateDto>().ReverseMap();

            CreateMap<ServiceProviderModel, ServiceProviderDto>().ReverseMap();
            CreateMap<ServiceProviderModel, ServiceProviderCreationDto>().ReverseMap();
            CreateMap<ServiceProviderModel, ServiceProviderUpdateDto>().ReverseMap();

            CreateMap<TechnicianModel, TechnicianDto>().ReverseMap();

            CreateMap<StoreModel, StoreDto>().ReverseMap();

            CreateMap<TrackingDeviceModel, TrackingDeviceDto>().ReverseMap();
            CreateMap<TrackingDeviceModel, TrackingDeviceCreationDto>().ReverseMap();
            CreateMap<TrackingDeviceModel, TrackingDeviceUpdateDto>().ReverseMap();

            CreateMap<CompanyModel, CompanyDto>().ReverseMap();
            
            CreateMap<CorporateModel, CorporateDto>().ReverseMap();

            CreateMap<DeliveryTicketModel, DeliveryTicketDto>().ReverseMap();
            CreateMap<DeliveryTicketModel, DeliveryTicketUpdateDto>().ReverseMap();
            CreateMap<DeliveryTicketModel, DeliveryTicketCreationDto>().ReverseMap();

            CreateMap<RepairIssueModel, RepairIssueDto>().ReverseMap();
            CreateMap<RepairIssueModel, RepairIssueDto>().ReverseMap();
            CreateMap<RepairIssueModel, RepairIssueDto>().ReverseMap();

            CreateMap<CartHistoryModel, CartHistoryDto>().ReverseMap();
            CreateMap<CartHistoryModel, CartHistoryCreationDto>().ReverseMap();
            CreateMap<CartHistoryModel, CartHistoryUpdateDto>().ReverseMap();

            CreateMap<WorkOrderModel, WorkOrderDto>().ReverseMap();
            CreateMap<WorkOrderModel, WorkOrderCreationDto>().ReverseMap();
            CreateMap<WorkOrderModel, WorkOrderUpdateDto>().ReverseMap();

            CreateMap<SystemOwnerModel, SystemOwnerDto>().ReverseMap();

            CreateMap<UsersModel, UserDto>().ReverseMap();
            CreateMap<DriverStoreModel, DriverStoreDto>().ReverseMap();
            CreateMap<DriverStoreModel, DriverStoreDetailsDto>().ReverseMap();

            CreateMap<DriverStoreDetailsDto, SpGetDriverAssignedStoresByCompany>().ReverseMap();
            CreateMap<DriverStoreDetailsDto, SpGetDriverAssignedStoresBySystemOwner>().ReverseMap();
            CreateMap<StoreDto, SpGetDriverAssignedStores>().ReverseMap();
            CreateMap<StoreDto, SPGetStoresBySystemOwnerId>().ReverseMap();
            CreateMap<DeliveryTicketDto, SPGetDeliveryTicketsById>().ReverseMap();
            CreateMap<ServiceBoardDto, SPGetServiceBoardDetailsById>().ReverseMap(); 

            CreateMap<NotificationModel, NotificationDto>().ReverseMap();
            CreateMap<CountyCodeModel, CountyCodeDto>().ReverseMap();
            CreateMap<SimCardModel, SimCardsDto>().ReverseMap();
            CreateMap<DevicesModel, DevicesDto>().ReverseMap();

            CreateMap<NotificationDto, SpGetNotification>().ReverseMap();
            CreateMap<TrackingDeviceDetailsDto, SPGetTrackingDeviceDetailsById>().ReverseMap();
            CreateMap<DeviceHistoryModel,DeviceHistoryDto >().ReverseMap();
            CreateMap<SimCardHistoryModel, SimCardHistoryDto>().ReverseMap();

            CreateMap<MessagesModel, MessagesDto>().ReverseMap();
            CreateMap<MessagesDto, SpGetMessagesById>().ReverseMap();
        }
    }
}
