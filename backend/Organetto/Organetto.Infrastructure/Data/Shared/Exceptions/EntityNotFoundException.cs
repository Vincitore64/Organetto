using Organetto.Infrastructure.Infrastructure.Shared.Exceptions.Models;

namespace Organetto.Infrastructure.Data.Shared.Exceptions
{
    internal class EntityNotFoundException : ApiException
    {
        public EntityNotFoundException(int status, string entityName) :
            base(status, $"{entityName} not found", "ENTITY_NOT_FOUND", "Entity not found",
                new Dictionary<string, string[]> { { "entityName", new[] { entityName } } }, null)
        {
        }
    }
}
