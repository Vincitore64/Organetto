using MassTransit;

namespace Organetto.Infrastructure.Infrastructure.MassTransit.Services
{
    public interface IRabbitBus : IBus { }
    public interface IKafkaBus : IBus { }
    public interface IInMemoryBus : IBus { }
}
