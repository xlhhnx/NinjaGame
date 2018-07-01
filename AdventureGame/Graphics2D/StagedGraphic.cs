namespace NinjaGame.Graphics2D
{
    public struct StagedGraphic
    {
        public string Id { get; set; }
        public string FilePath { get; set; }
        public GraphicType Type { get; set; }

        public StagedGraphic(string id, string filePath, GraphicType type)
        {
            Id = id;
            FilePath = filePath;
            Type = type;
        }
    }
}
