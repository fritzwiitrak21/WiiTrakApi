namespace WiiTrakApi.Repository.Contracts
{
    public interface ITrackSolidRepository
    {
        Task<(bool IsSuccess, string? ErrorMessage)> GetDataFromTrackSolidAsync();
    }
}
