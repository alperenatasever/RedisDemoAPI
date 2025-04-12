using Microsoft.AspNetCore.Mvc;
using RedisDemoAPI.Models;
using RedisDemoAPI.Services;

namespace RedisDemoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly RedisService _redisService;

        public CacheController(RedisService redisService)
        {
            _redisService = redisService;
        }

        [HttpPost]
        public IActionResult Set([FromQuery] string key, [FromQuery] string value)
        {
            _redisService.SetString(key, value);
            return Ok("Key set");
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string key)
        {
            var value = _redisService.GetString(key);
            return Ok(value ?? "Key not found");
        }

        [HttpPost("set-product")]
        public async Task<IActionResult> SetProduct()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Red Apple",
                Price = 9.99m
            };

            // 60 saniyelik TTL
            await _redisService.SetProductAsync("product:1", product, TimeSpan.FromSeconds(60));
            return Ok("Product cached with TTL!");
        }

        [HttpGet("get-product")]
        public async Task<IActionResult> GetProduct()
        {
            var product = await _redisService.GetProductAsync("product:1");
            if (product == null)
                return NotFound("Product not found or expired");

            return Ok(product);
        }

        [HttpPost("set-user")]
        public async Task<IActionResult> SetUser()
        {
            var user = new User
            {
                Id = "1002",
                Name = "Your Name",
                Email = "mail@example.com"
            };

            await _redisService.SetUserHashAsync(user);
            return Ok("User hash kaydedildi.");
        }

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser([FromQuery] string id)
        {
            var user = await _redisService.GetUserHashAsync(id);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            return Ok(user);
        }

    }
}
