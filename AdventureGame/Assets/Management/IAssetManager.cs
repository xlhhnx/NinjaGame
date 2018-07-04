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
        
        void LoadAssetBatches(string filePath);
        void LoadBatchAssets(IAssetBatch batch);
        void LoadBatchAssetsById(string id);
        void LoadBatchAssetsByName(string name);
        void LoadAssetBatchesAsync(string filePath);
        void LoadBatchAssetsAsync(IAssetBatch batch);
        void LoadBatchAssetsAsyncById(string id);
        void LoadBatchAssetsAsyncByName(string name);
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
