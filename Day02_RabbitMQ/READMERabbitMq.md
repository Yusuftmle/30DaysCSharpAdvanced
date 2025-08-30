# Day02 - RabbitMQ Çalışması

Bu proje, RabbitMQ message broker'ı kullanarak temel mesajlaşma işlemlerini öğrenmek için hazırlanmıştır.

## 📋 Proje İçeriği

- **RabbitMQConsumer**: Mesaj kuyruğundan mesaj okuma işlemleri
- **RabbitMQPublisher**: Mesaj kuyruğuna mesaj gönderme işlemleri
- **Temel RabbitMQ Yapıları**: Queue, Exchange, Routing gibi kavramlar

## 🚀 RabbitMQ Nedir?

RabbitMQ, Advanced Message Queuing Protocol (AMQP) protokolünü kullanan açık kaynaklı bir message broker'dır. Uygulamalar arası güvenilir mesajlaşma sağlar.

### Ana Kavramlar:

1. **Producer (Üretici)**: Mesaj gönderen uygulama
2. **Queue (Kuyruk)**: Mesajların saklandığı yer  
3. **Consumer (Tüketici)**: Mesaj alan uygulama
4. **Exchange**: Mesajları kuyruklara yönlendiren bileşen
5. **Routing Key**: Mesajların hangi kuyruğa gideceğini belirleyen anahtar

## 🛠️ Kurulum

### Gereksinimler:
- .NET Core 3.1+
- RabbitMQ Server
- RabbitMQ.Client NuGet paketi

### RabbitMQ Server Kurulumu:

#### Docker ile:
```bash
docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

#### Windows için:
1. [RabbitMQ'yu indirin](https://www.rabbitmq.com/download.html)
2. Erlang'ı kurun
3. RabbitMQ'yu kurun ve başlatın

### NuGet Paketi:
```bash
dotnet add package RabbitMQ.Client
```

## 📊 Exchange Türleri

### 1. Direct Exchange
```csharp
// Tam eşleşme ile routing
channel.ExchangeDeclare("direct_exchange", ExchangeType.Direct);
```

### 2. Fanout Exchange  
```csharp
// Tüm bağlı kuyruklara gönderir
channel.ExchangeDeclare("fanout_exchange", ExchangeType.Fanout);
```

### 3. Topic Exchange
```csharp
// Wildcard pattern ile routing (* ve # kullanarak)
channel.ExchangeDeclare("topic_exchange", ExchangeType.Topic);
```

### 4. Headers Exchange
```csharp
// Header bilgilerine göre routing
channel.ExchangeDeclare("headers_exchange", ExchangeType.Headers);
```

## 💻 Kullanım Örnekleri

### Producer Örneği:
```csharp
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare("test_queue", true, false, false, null);
    
    var message = "Merhaba RabbitMQ!";
    var body = Encoding.UTF8.GetBytes(message);
    
    channel.BasicPublish("", "test_queue", null, body);
    Console.WriteLine($"Gönderilen mesaj: {message}");
}
```

### Consumer Örneği:
```csharp
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Alınan mesaj: {message}");
    
    // Mesajın işlendiğini bildirme
    channel.BasicAck(ea.DeliveryTag, false);
};

channel.BasicConsume("test_queue", false, consumer);
```

## 🔧 Önemli Özellikler

### Message Durability (Kalıcılık)
```csharp
// Queue'yu kalıcı yapar
channel.QueueDeclare("persistent_queue", durable: true, false, false, null);

// Mesajı kalıcı yapar
var properties = channel.CreateBasicProperties();
properties.Persistent = true;
```

### Message Acknowledgment (Onaylama)
```csharp
// Manuel onaylama
channel.BasicConsume("queue_name", autoAck: false, consumer);

// Mesaj işlendikten sonra onaylama
channel.BasicAck(deliveryTag, false);
```

### Fair Dispatch (Adil Dağıtım)
```csharp
// Her consumer'a aynı anda sadece 1 mesaj gönderir
channel.BasicQos(0, 1, false);
```

## 🎯 Avantajları

- **Güvenilirlik**: Mesaj kaybolma riski minimum
- **Esneklik**: Farklı routing stratejileri
- **Performans**: Yüksek throughput
- **Cluster Desteği**: Yüksek erişilebilirlik
- **Management UI**: Web tabanlı yönetim arayüzü (port 15672)

## 📈 İzleme ve Yönetim

RabbitMQ Management Plugin ile:
- Queue durumları
- Mesaj sayıları  
- Connection bilgileri
- Performance metrikleri

Erişim: `http://localhost:15672` (guest/guest)

## 🔍 Debugging İpuçları

1. **Connection sorunları**: RabbitMQ server'ın çalıştığından emin olun
2. **Permission hatası**: User permissions kontrol edin
3. **Memory Issues**: Queue'larda biriken mesajları kontrol edin
4. **Network**: Port 5672'nin açık olduğundan emin olun

## 🚀 Sonraki Adımlar

- [ ] Clustering kurulumu
- [ ] SSL/TLS güvenliği
- [ ] Custom Exchange türleri
- [ ] Dead Letter Queues
- [ ] Message TTL (Time To Live)

---

**30 Days Challenge - Day 2** 🎯  
*RabbitMQ ile messaging patterns öğreniyoruz!*