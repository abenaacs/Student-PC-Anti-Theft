namespace Safe_Campus.Models
{
    public class CampusDatabaseSettings : ICampusDatabaseSettings
    {
        public string ConnectionString { get ; set ; } = String.Empty;
        public string DatabaseName { get ; set; } = String.Empty;
        public string CollectionsName { get; set; } = String.Empty;
    }
}
