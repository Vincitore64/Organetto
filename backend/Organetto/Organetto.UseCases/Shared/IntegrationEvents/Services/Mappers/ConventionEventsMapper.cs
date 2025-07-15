using AutoMapper;
using Organetto.Core.Shared.Events.Models;
using Organetto.UseCases.Shared.IntegrationEvents.Models;
using Organetto.UseCases.Shared.IntegrationEvents.Services.Mappers.Configuration.Extensions;

namespace Organetto.UseCases.Shared.IntegrationEvents.Services.Mappers
{
    public sealed class ConventionEventsMapper : IEventsMapper
    {
        private readonly IMapper _mapper;

        public ConventionEventsMapper(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> domainEvents)
        {
            //foreach (var de in domainEvents)
            //{
            //    var ieType = de.GetIntegrationEventType();

            //    var ieObj = _mapper.Map(de, de.GetType(), ieType);

            //    if (ieObj == null) continue;

            //    try
            //    {
            //        var ie = (IIntegrationEvent)ieObj;
            //        yield return ie;
            //    }
            //    catch (Exception)
            //    {
            //        continue;
            //    }
            //}

            return domainEvents.Select(de =>
            {
                var ieType = de.GetIntegrationEventType();

                var ieObj = _mapper.Map(de, de.GetType(), ieType);
                return ieObj;
                //if (ieObj == null) return null;

                //try
                //{
                //    var ie = (IIntegrationEvent)ieObj;
                //    return ie;
                //}
                //catch (Exception)
                //{
                //    return null;
                //}
            }).OfType<IIntegrationEvent>();
        }
    }
}
