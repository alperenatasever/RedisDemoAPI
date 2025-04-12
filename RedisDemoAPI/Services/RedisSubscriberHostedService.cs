using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace RedisDemoAPI.Services
{
    public class RedisSubscriberHostedService : BackgroundService
    {
        private readonly RedisService _redisService;

        public RedisSubscriberHostedService(RedisService redisService)
        {
            _redisService = redisService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _redisService.Subscribe("test-channel", (channel, message) =>
            {
                Console.WriteLine($"📥 [RedisSubscriber] Kanal: {channel}, Mesaj: {message}");
            });

            return Task.CompletedTask; // Abone olduktan sonra başka iş yapmıyoruz
        }
    }
}
