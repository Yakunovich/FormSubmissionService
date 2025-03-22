namespace FormSubmissionService.Configuration
{
    public class DatabaseSettings
    {
        public const string SectionName = "Database";
        public string Provider { get; set; } = "InMemory";
        public bool MigrationEnabled { get; set; } = false;
    }
} 