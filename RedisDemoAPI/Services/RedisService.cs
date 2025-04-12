using RedisDemoAPI.Models;
using StackExchange.Redis;
using System;
using System.Text.Json;

namespace RedisDemoAPI.Services
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _redis;
        public IDatabase _db { get; }

        public RedisService(string redisHost)
        {
            _redis = ConnectionMultiplexer.Connect(redisHost);
            _db = _redis.GetDatabase();
        }

        public void SetString(string key, string value)
        {
            _db.StringSet(key, value);
        }

        public string GetString(string key)
        {
            return _db.StringGet(key);
        }

         // 🔸 JSON veri kaydetme + TTL
        public async Task SetProductAsync(string key, Product product, TimeSpan? expiry = null)
        {
            string json = JsonSerializer.Serialize(product);
            await _db.StringSetAsync(key, json, expiry);
        }

        // 🔸 JSON veri okuma
        public async Task<Product?> GetProductAsync(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<Product>(value!);
        }
        public async Task SetUserHashAsync(User user)
        {
            var key = $"user:{user.Id}";
            var entries = new HashEntry[]
            {
                new HashEntry("name", user.Name),
                new HashEntry("email", user.Email)
            };

            await _db.HashSetAsync(key, entries);
        }

        public async Task<User?> GetUserHashAsync(string id)
        {
            var key = $"user:{id}";
            if (!await _db.KeyExistsAsync(key))
                return null;

            var entries = await _db.HashGetAllAsync(key);
            return new User
            {
                Id = id,
                Name = entries.FirstOrDefault(e => e.Name == "name").Value,
                Email = entries.FirstOrDefault(e => e.Name == "email").Value
            };
        }

        public async Task EnqueueTaskAsync(TaskItem task)
        {
            string json = JsonSerializer.Serialize(task);
            await _db.ListRightPushAsync("task_queue", json);
        }

        public async Task<TaskItem?> DequeueTaskAsync()
        {
            var json = await _db.ListLeftPopAsync("task_queue");
            if (json.IsNullOrEmpty) return null;

            return JsonSerializer.Deserialize<TaskItem>(json!);
        }

        public async Task<long> GetQueueLengthAsync()
        {
            return await _db.ListLengthAsync("task_queue");
        }

        public async Task PublishAsync(string channel, string message)
        {
            var sub = _redis.GetSubscriber();
            await sub.PublishAsync(channel, message);
        }

        public void Subscribe(string channel, Action<RedisChannel, RedisValue> handler)
        {
            var sub = _redis.GetSubscriber();
            sub.Subscribe(channel, handler);
        }

    }
}
