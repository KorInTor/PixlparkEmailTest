using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.RegularExpressions;

namespace EmailSenderServer
{
    internal partial class Program
    {
		static async Task Main(string[] args)
        {
			var factory = new ConnectionFactory { HostName = "localhost" };
			using var connection = await factory.CreateConnectionAsync();
			using var channel = await connection.CreateChannelAsync();

			await channel.QueueDeclareAsync(queue: "emailCodes", durable: false, exclusive: false, autoDelete: false,
				arguments: null);

			Console.WriteLine(" [*] Waiting for codes.");

			var consumer = new AsyncEventingBasicConsumer(channel);
			consumer.ReceivedAsync += (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				var match = EmailCodeRegex().Match(message);
				if (match.Success)
				{
					var email = match.Groups[1].Value;
					var code = match.Groups[2].Value;
					Console.WriteLine($"{DateTime.Now} {email} код: {code}");
				}
				return Task.CompletedTask;
			};

			await channel.BasicConsumeAsync("emailCodes", autoAck: true, consumer: consumer);

			Console.ReadLine();
		}

		[GeneratedRegex(@"^([^:]+):(.+)$")]
		public static partial Regex EmailCodeRegex();
	}
}
