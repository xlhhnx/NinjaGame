namespace NinjaGame.UI
{
    public struct StagedControl
    {
        public string Id { get; set; }
        public string FilePath { get; set; }
        public ControlType Type { get; set; }

        public StagedControl(string id, string filePath, ControlType type)
        {
            Id = id;
            FilePath = filePath;
            Type = type;
        }
    }
}
