namespace Safe_Campus.Models
{
    public interface ICampusDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionsName { get; set; }
    }
}
