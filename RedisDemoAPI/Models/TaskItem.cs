namespace RedisDemoAPI.Models
{
    public class TaskItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}
