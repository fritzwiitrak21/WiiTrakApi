namespace WiiTrakApi.DTOs
{
    public class DeliveryTicketSummaryDto
    {
        public int Delivered { get; set; }

        public int Lost { get; set; }

        public int Damaged { get; set; }

        public int Trashed { get; set; }
    }
}
