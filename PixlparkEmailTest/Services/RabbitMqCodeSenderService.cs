using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace PixlparkEmailTest.Services
{
    public class RabbitMqOptions
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
    }

    public class RabbitMqCodeSenderService : ICodeSenderService
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly string _queueName;

        private RabbitMqCodeSenderService(IConnection connection, IChannel channel, string queueName)
        {
            _connection = connection;
            _channel = channel;
            _queueName = queueName;
        }

        public static async Task<RabbitMqCodeSenderService> CreateAsync(IOptions<RabbitMqOptions> options)
        {
            var factory = new ConnectionFactory()
            {
                HostName = options.Value.HostName,
                UserName = options.Value.UserName,
                Password = options.Value.Password
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: options.Value.QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            return new RabbitMqCodeSenderService(connection, channel, options.Value.QueueName);
        }

        public async Task SendCodeAsync(string email,string code)
        {
            string stringBody = $"{email}:{code}";

            var body = Encoding.UTF8.GetBytes(stringBody);

            await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: _channel.CurrentQueue, body: body);
        }
    }

}
