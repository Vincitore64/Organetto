namespace Organetto.Core.Shared.Services
{
    public interface ITrackingRepository<TRepository>
    {
        TRepository WithTracking();
        TRepository WithoutTracking();
    }
}
