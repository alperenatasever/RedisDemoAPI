using Microsoft.AspNetCore.Mvc;
using RedisDemoAPI.Services;

[ApiController]
[Route("[controller]")]
public class PubSubController : ControllerBase
{
    private readonly RedisService _redisService;

    public PubSubController(RedisService redisService)
    {
        _redisService = redisService;
    }

    [HttpPost("publish")]
    public async Task<IActionResult> Publish([FromQuery] string channel, [FromQuery] string message)
    {
        await _redisService.PublishAsync(channel, message);
        return Ok("Mesaj yayınlandı!");
    }
}
