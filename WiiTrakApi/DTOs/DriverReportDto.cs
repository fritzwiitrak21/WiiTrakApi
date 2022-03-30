namespace WiiTrakApi.DTOs
{
    public class DriverReportDto
    {
        public Guid DriverId { get; set; }

        public int TotalStores { get; set; }

        public int TotalCarts { get; set; }

        public int TotalCartsAtStore { get; set; }

        public int TotalCartsOutsideStore { get; set; }

        public int TotalCartsNeedingRepair { get; set; }

        public int TotalCartsLost { get; set; }

        public int TotalCartsTrashed { get; set; }

        public int TotalCartsGood { get; set; }

        public int CartsOnVehicleToday { get; set; }

        public int CartsDeliveredToday { get; set; }

        public int CartsNeedingRepairToday { get; set; }

        public int CartsLostToday { get; set; }

    }
}
