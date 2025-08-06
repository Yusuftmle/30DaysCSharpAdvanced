using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IConnection _connection;
    private IChannel _channel; // IModel yerine IChannel (yeni RabbitMQ Client)

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            // Async connection oluþturma (await kullan)
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            // Queue declare
            await _channel.QueueDeclareAsync(
                queue: "hello-queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _logger.LogInformation("RabbitMQ connection established and queue declared.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting RabbitMQ worker");
            throw;
        }

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel == null)
        {
            _logger.LogError("Channel is not initialized");
            return;
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"[x] Message received: {message}");

                // Mesaj iþleme kodunuz buraya
                await ProcessMessage(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
            }

            // Lambda fonksiyonunda Task döndür
            await Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(
            queue: "hello-queue",
            autoAck: true,
            consumer: consumer);

        _logger.LogInformation("Started consuming messages. Waiting for messages...");

        // Servis çalýþmaya devam etsin
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task ProcessMessage(string message)
    {
        // Mesaj iþleme
        _logger.LogInformation($"Processing message: {message}");

        // Örnek: 1 saniye bekle (simüle iþlem)
        await Task.Delay(100);

        _logger.LogInformation($"Message processed: {message}");
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping RabbitMQ worker...");

        try
        {
            if (_channel != null)
            {
                await _channel.CloseAsync(); // CloseAsync kullan
            }

            if (_connection != null)
            {
                await _connection.CloseAsync(); // CloseAsync kullan
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during shutdown");
        }

        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        try
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disposing resources");
        }

        base.Dispose();
    }
}
