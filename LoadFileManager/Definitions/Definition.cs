namespace LoadFileManager.Definitions
{
    public class Definition<T> : IDefinition<T>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AssetFile { get; set; }
        public T Type { get; set; }

        public Definition(string id, string name, string assetFile, T type)
        {
            Id = id;
            Name = name;
            AssetFile = assetFile;
            Type = type;
        }
    }
}
