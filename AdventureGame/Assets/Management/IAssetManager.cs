using NinjaGame.AssetManagement;
using NinjaGame.Assets.Batches;
using NinjaGame.Assets.Loading;
using System;

namespace NinjaGame.Assets.Management
{
    public interface IAssetManager
    {
        event Action<IAssetBatch> AssetBatchLoadedEvent;
        event Action<IAssetBatch> BatchAssetsLoadedEvent;
        event Action<IAsset> AssetLoadedEvent;

        IAssetLoader Loader { get; set; }

        void LoadAsset(string filePath, string id, string batchId);
        void LoadAssetBatch(string filePath, string id);
        void LoadBatchAssets(string id);
        void LoadAssetAsync(string filePath, string id, string batchId);
        void LoadAssetBatchAsync(string filePath, string id);
        void LoadBatchAssetsAsync(string id);
        void UnloadBatch(string id);
        void UnloadAll();
        void Recycle();
        bool ContainsBatch(string id);
        bool ContainsBatch(IAssetBatch batch);
        bool GraphicLoaded(string id);
        bool GraphicLoaded(IAssetBatch graphic);
        IAssetBatch GetAssetBatch(string id);
        AudioAsset GetAudioAsset(string id);
        SpriteFontAsset GetSpriteFontAsset(string id);
        Texture2DAsset GetTexture2DAsset(string id);
    }
}
