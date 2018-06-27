using NinjaGame.AssetManagement;
using NinjaGame.Assets.Batches;

namespace NinjaGame.Assets.Management
{
    public interface IAssetManager
    {
        void LoadAsset(string filePath, string id, string batchId = "");
        void LoadAssetBatch(string filePath, string id);
        void LoadBatchAssets(string id);
        void UnloadBatch(string id);
        void Recycle();
        void UnloadAll();
        IAssetBatch GetAssetBatch(string id);
        AudioAsset GetAudioAsset(string id);
        SpriteFontAsset GetSpriteFontAsset(string id);
        Texture2DAsset GetTexture2DAsset(string id);
    }
}
