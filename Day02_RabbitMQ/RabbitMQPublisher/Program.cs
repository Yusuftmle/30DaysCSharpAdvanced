using RabbitMQ.Client;
using System.Text;


var factory = new ConnectionFactory() { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();
await channel.QueueDeclareAsync(queue: "hello-queue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.Write("Gönderilecek mesaj: ");
var message = Console.ReadLine();
var body = Encoding.UTF8.GetBytes(message);

var props = new BasicProperties(); // RabbitMQ.Client v7+otne
await channel.BasicPublishAsync(
    exchange: "",
    routingKey: "hello-queue",
    mandatory: false,
    basicProperties: props,
    body: body
);

Console.WriteLine($"[x] Mesaj gönderildi: {message}");
