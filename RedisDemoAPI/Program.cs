using Quartz;
using RedisDemoAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton(new RedisService("localhost:6379"));
var redisService = new RedisService("localhost:6379");
builder.Services.AddHostedService<RedisSubscriberHostedService>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "RedisDemo_";
});
redisService.Subscribe("test-channel", (channel, message) =>
{
    Console.WriteLine($"📥 Mesaj alındı -> Kanal: {channel}, İçerik: {message}");
});
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("RedisQueueWorker");

    q.AddJob<RedisQueueWorkerJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("RedisQueueTrigger")
        .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(10)
            .RepeatForever()
        ));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
