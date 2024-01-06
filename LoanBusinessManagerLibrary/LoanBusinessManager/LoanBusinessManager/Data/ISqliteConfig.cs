namespace LBMLibrary.Data
{
    public interface ISqliteConfig
    {
        public string Path { get; set; }
        public string DbName { get; set; }
        public string ExtensionType { get; set; }
    }
}
