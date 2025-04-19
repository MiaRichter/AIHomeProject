namespace AIHomeProject.Models
{
    public class Component
    {
        public string ComponentId { get; set; }  // Основной идентификатор
        public string Name { get; set; }
        public string Description { get; set; }
        public string ComponentType { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
        // Остальные поля серверной модели
        public int Id { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}