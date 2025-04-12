using Microsoft.Extensions.Logging;
using Quartz;
using RedisDemoAPI.Models;
using RedisDemoAPI.Services;
using System.Text.Json;

public class RedisQueueWorkerJob : IJob
{
    private readonly RedisService _redisService;
    private readonly ILogger<RedisQueueWorkerJob> _logger;

    public RedisQueueWorkerJob(RedisService redisService, ILogger<RedisQueueWorkerJob> logger)
    {
        _redisService = redisService;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var task = await _redisService.DequeueTaskAsync();
        if (task == null)
        {
            _logger.LogInformation("📭 Kuyruk boş. Bekleniyor...");
            return;
        }

        _logger.LogInformation("⏳ Yeni görev alındı: [{Type}] {Description}", task.Type, task.Description);

        switch (task.Type.ToLowerInvariant())
        {
            case "email":
                await ProcessEmailAsync(task);
                break;
            case "report":
                await ProcessReportAsync(task);
                break;
            case "notification":
                await ProcessNotificationAsync(task);
                break;
            default:
                _logger.LogWarning("❌ Bilinmeyen görev tipi: {Type}", task.Type);
                break;
        }
    }
    private Task ProcessEmailAsync(TaskItem task)
    {
        _logger.LogInformation("📧 Email gönderiliyor: {Description}", task.Description);
        return Task.Delay(500); // Simülasyon
    }

    private Task ProcessReportAsync(TaskItem task)
    {
        _logger.LogInformation("📄 Rapor hazırlanıyor: {Description}", task.Description);
        return Task.Delay(1000); // Simülasyon
    }

    private Task ProcessNotificationAsync(TaskItem task)
    {
        _logger.LogInformation("🔔 Bildirim gönderiliyor: {Description}", task.Description);
        return Task.Delay(300); // Simülasyon
    }
}
