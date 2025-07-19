namespace Organetto.Core.Shared.Services.Extensions
{
    public static class ITrackingRepositoryExtensions
    {
        public static TConcreteRepository WithTracking<TConcreteRepository>(this TConcreteRepository repository)
        {
            if (repository is ITrackingRepository<TConcreteRepository> trackingRepository)
            {
                return trackingRepository.WithTracking();
            }

            return repository;
        }

        public static TConcreteRepository WithoutTracking<TConcreteRepository>(this TConcreteRepository repository)
        {
            if (repository is ITrackingRepository<TConcreteRepository> trackingRepository)
            {
                return trackingRepository.WithoutTracking();
            }

            return repository;
        }
    }
}
