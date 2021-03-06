/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Enums;

namespace WiiTrakApi.DTOs
{
    public record PickupCreationDto
    {
        public DateTime CreatedAt { get; set; }

        public Guid CartId { get; set; }

        public DateTime PickedUpAt { get; set; }

        public DateTime DroppedOffAt { get; set; }

        public Guid ServiceProviderId { get; set; } = Guid.Empty;

        public Guid StoreId { get; set; } = Guid.Empty;

        public Guid DriverId { get; set; } = Guid.Empty;

        public CartCondition Condition { get; set; }

        public double PickupLongitude { get; set; }

        public double PickupLatitude { get; set; }
    }
}
