/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
namespace WiiTrakApi.DTOs
{
    public class CompanyReportDto
    {
        public Guid CompanyId { get; set; }

        public int TotalStores { get; set; }

        public int TotalCarts { get; set; }

        public int TotalCartsAtStore { get; set; }

        public int TotalCartsOutsideStore { get; set; }

        public int TotalCartsNeedingRepair { get; set; }

        public int TotalCartsLost { get; set; }

        public int CartsOnVehicleToday { get; set; }

        public int CartsDeliveredToday { get; set; }

        public int CartsNeedingRepairToday { get; set; }

        public int CartsLostToday { get; set; }
    }
}
