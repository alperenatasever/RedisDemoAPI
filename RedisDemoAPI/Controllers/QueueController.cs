using Microsoft.AspNetCore.Mvc;
using RedisDemoAPI.Models;
using RedisDemoAPI.Services;

[ApiController]
[Route("[controller]")]
public class QueueController : ControllerBase
{
    private readonly RedisService _redisService;

    public QueueController(RedisService redisService)
    {
        _redisService = redisService;
    }

    [HttpPost("enqueue")]
    public async Task<IActionResult> Enqueue([FromBody] TaskItem task)
    {
        await _redisService.EnqueueTaskAsync(task);
        return Ok("Task enqueued!");
    }

    [HttpGet("dequeue")]
    public async Task<IActionResult> Dequeue()
    {
        var task = await _redisService.DequeueTaskAsync();
        if (task == null)
            return NotFound("Queue is empty");

        return Ok(task);
    }

    [HttpGet("length")]
    public async Task<IActionResult> Length()
    {
        var length = await _redisService.GetQueueLengthAsync();
        return Ok(new { Length = length });
    }
}