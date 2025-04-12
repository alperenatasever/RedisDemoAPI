using StackExchange.Redis;
using System;

namespace RedisDemoAPI.Services
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _redis;
        public IDatabase db { get; }

        public RedisService(string redisHost)
        {
            _redis = ConnectionMultiplexer.Connect(redisHost);
            db = _redis.GetDatabase();
        }

        public void SetString(string key, string value)
        {
            db.StringSet(key, value);
        }

        public string GetString(string key)
        {
            return db.StringGet(key);
        }
    }
}
