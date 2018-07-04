using NinjaGame.AssetManagement;

namespace NinjaGame.Assets
{
    public interface IAsset
    {
        string Id { get; }
        string Name { get; }
        bool Loaded { get; }
        AssetType Type { get; }

        void Unload();
    }
}