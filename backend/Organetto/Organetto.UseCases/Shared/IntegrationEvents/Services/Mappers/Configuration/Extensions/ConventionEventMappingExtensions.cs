using Organetto.Core.Shared.Events.Models;
using Organetto.UseCases.Shared.IntegrationEvents.Models;
using System.Reflection;

namespace Organetto.UseCases.Shared.IntegrationEvents.Services.Mappers.Configuration.Extensions
{
    internal static class ConventionEventMappingExtensions
    {
        private static Type[] _iEventTypes = Array.Empty<Type>();

        public static Type? GetIntegrationEventType(this Type deType)
        {
            if (_iEventTypes.Length == 0)
            {
                var iEventTypesAssembly = Assembly.GetExecutingAssembly();
                var iEventTypes = iEventTypesAssembly
                    .GetTypes()
                    .Where(t => typeof(IIntegrationEvent).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                    .ToArray();
                _iEventTypes = iEventTypes;
            }
            var ieTypeName = deType.Name.Replace("DomainEvent", string.Empty) + "IntegrationEvent";
            var ieType = _iEventTypes.FirstOrDefault(t => t.Name == ieTypeName);
            return ieType;
        }

        public static Type? GetIntegrationEventType(this IDomainEvent domainEvent)
        {
            return domainEvent.GetType().GetIntegrationEventType();
        }
    }
}
