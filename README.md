# RedisDemoAPI

RedisDemoAPI, Redis ile çalışan bir ASP.NET Core Web API projesidir. Bu proje, Redis'in çeşitli özelliklerini kullanarak veri önbellekleme, mesaj yayınlama/abone olma (Pub/Sub) ve kuyruk işlemleri gibi işlevleri gerçekleştirmektedir.

## Özellikler

- **Redis Cache**: Redis'i bir önbellek olarak kullanarak hızlı veri erişimi sağlar.
- **Pub/Sub**: Redis'in yayınlama/abone olma mekanizmasını kullanarak mesajlaşma işlemleri gerçekleştirir.
- **Quartz Scheduler**: Quartz.NET ile zamanlanmış görevler çalıştırır.
- **Redis Queue**: Redis kuyruklarını kullanarak işleme görevlerini yönetir.

## Proje Yapısı

- `Controllers/`: API uç noktalarını içeren kontrolörler.
  - `CacheController.cs`: Redis önbellekleme işlemleri için API uç noktaları.
  - `DistributedCacheController.cs`: Dağıtılmış önbellek işlemleri için API uç noktaları.
  - `PubSubController.cs`: Redis Pub/Sub işlemleri için API uç noktaları.
  - `QueueController.cs`: Redis kuyruk işlemleri için API uç noktaları.
  - `WeatherForecastController.cs`: Örnek bir kontrolör.
- `Services/`: Uygulama hizmetlerini içeren sınıflar.
  - `RedisService.cs`: Redis ile etkileşim sağlayan hizmet.
  - `RedisSubscriberHostedService.cs`: Redis Pub/Sub için arka plan hizmeti.
- `Jobs/`: Quartz zamanlanmış görevlerini içeren sınıflar.
  - `RedisQueueWorkerJob.cs`: Redis kuyruğundaki işleri işleyen görev.
- `Models/`: Veri modellerini içeren sınıflar.
  - `Product.cs`, `TaskItem.cs`, `User.cs`: Örnek veri modelleri.

## Kullanım

1. Projeyi klonlayın:
   ```bash
   git clone <repository-url>
   cd RedisDemoAPI
   ```

2. Gerekli bağımlılıkları yükleyin:
   ```bash
   dotnet restore
   ```

3. Uygulamayı çalıştırın:
   ```bash
   dotnet run
   ```

4. Swagger arayüzüne erişerek API uç noktalarını test edin:
   - [https://localhost:5001/swagger](https://localhost:5001/swagger)

## Gereksinimler

- .NET 8.0 SDK
- Redis Sunucusu (localhost:6379 varsayılan olarak kullanılır)

## Yapılandırma

Redis bağlantı ayarları `appsettings.json` dosyasında yapılandırılabilir:

```json
"Redis": {
  "Host": "localhost:6379"
}
```

## Lisans

Bu proje MIT Lisansı ile lisanslanmıştır. Daha fazla bilgi için [LICENSE](LICENSE) dosyasına bakın.