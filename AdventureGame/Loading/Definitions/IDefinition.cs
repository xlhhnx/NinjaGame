namespace NinjaGame.Loading.Definitions
{
    public interface IDefinition<T>
    {
        string Id { get; set;  }
        string Name { get; set; }
        string AssetFile { get; set; }
        T Type { get; set; }
    }
}
