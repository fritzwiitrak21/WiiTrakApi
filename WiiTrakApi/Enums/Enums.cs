namespace WiiTrakApi.Enums
{
   public enum CartCondition
    {
        Good,
        Damage,
        DamageBeyondRepair
    }

    public enum CartStatus
    {
        InsideGeofence,
        OutsideGeofence,
        PickedUp,
        Lost,
        Trashed
    }

    public enum CartOrderedFrom
    {
        Manufacture,
        Seller,
        Lessor
    }
}
