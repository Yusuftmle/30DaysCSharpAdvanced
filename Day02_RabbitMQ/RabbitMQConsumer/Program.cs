using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };

await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync("hello-queue", durable: false, exclusive: false, autoDelete: false);

var consumer = new AsyncEventingBasicConsumer(channel);

 consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"[x] Received: {message}");
    await Task.Yield(); // async context içinde kalsın
};

await channel.BasicConsumeAsync(
    queue: "hello-queue",
    autoAck: true,
    consumer: consumer
);

Console.WriteLine(" [*] Waiting for messages. Press [enter] to exit.");
Console.ReadLine();
