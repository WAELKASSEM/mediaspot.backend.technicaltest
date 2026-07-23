using RabbitMQ.Client;

namespace Mediaspot.Infrastructure.Queuing;

public class RabbitMqConnectionProvider
{
    private readonly Lazy<Task<IConnection>> _connection;

    public RabbitMqConnectionProvider(string connectionString)
    {
        _connection = new Lazy<Task<IConnection>>(async () =>
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(connectionString)
            };

            return await factory.CreateConnectionAsync();
        });
    }

    public Task<IConnection> GetConnectionAsync() => _connection.Value;
}