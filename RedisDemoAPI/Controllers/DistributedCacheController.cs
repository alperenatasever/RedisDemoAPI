using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RedisDemoAPI.Models;
using System.Text.Json;
using System.Text;

[ApiController]
[Route("[controller]")]
public class DistributedCacheController : ControllerBase
{
    private readonly IDistributedCache _cache;

    public DistributedCacheController(IDistributedCache cache)
    {
        _cache = cache;
    }

    [HttpPost("set")]
    public async Task<IActionResult> SetCache()
    {
        var product = new Product
        {
            Id = 10,
            Name = "Cached Phone",
            Price = 19999.99m
        };

        var json = JsonSerializer.Serialize(product);
        var bytes = Encoding.UTF8.GetBytes(json);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        await _cache.SetAsync("product:10", bytes, options);
        return Ok("Product cached with Redis IDistributedCache.");
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetCache()
    {
        var bytes = await _cache.GetAsync("product:10");
        if (bytes == null) return NotFound("Not found");

        var json = Encoding.UTF8.GetString(bytes);
        var product = JsonSerializer.Deserialize<Product>(json);

        return Ok(product);
    }
}
