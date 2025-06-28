using MassTransit;

namespace Organetto.UseCases.Shared.MassTransit.Services
{
    public interface IRabbitBus : IBus { }
    public interface IKafkaBus : IBus { }
    public interface IInMemoryBus : IBus { }
}