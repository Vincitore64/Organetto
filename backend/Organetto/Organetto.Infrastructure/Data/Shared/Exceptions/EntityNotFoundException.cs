using Organetto.UseCases.Shared.Exceptions.Models;

namespace Organetto.Infrastructure.Data.Shared.Exceptions
{
    internal class EntityNotFoundException : AppException
    {
        public EntityNotFoundException(int status, string entityName) :
            base(status, $"{entityName} not found", nameof(AppErrorCode.ENTITY_NOT_FOUND), "Entity not found",
                new Dictionary<string, string[]> { { "entityName", new[] { entityName } } }, null)
        {
        }
    }
}
