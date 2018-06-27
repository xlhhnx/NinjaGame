using NinjaGame.AssetManagement;
using NinjaGame.Assets.Batches;
using NinjaGame.Assets.Loading;
using System;
using System.Collections.Generic;

namespace NinjaGame.Assets.Management
{
    public class AssetManager : IAssetManager
    {
        IAssetLoader _assetLoader;
        IServiceProvider _serviceProvider;
        Dictionary<string, IAssetBatch> _assetBatches;
        Dictionary<string, IAsset> _assetDict;


        public AssetManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _assetLoader = new AssetLoader();
            _assetBatches = new Dictionary<string, IAssetBatch>();
            _assetDict = new Dictionary<string, IAsset>();
        }

        public IAssetBatch GetAssetBatch(string id)
        {
            IAssetBatch batch = null;
            lock (_assetBatches)
                _assetBatches.TryGetValue(id, out batch);

            return batch;
        }

        public AudioAsset GetAudioAsset(string id)
        {
            IAsset asset = null;
            lock (_assetDict)
                _assetDict.TryGetValue(id, out asset);

            return asset as AudioAsset;
        }

        public SpriteFontAsset GetSpriteFontAsset(string id)
        {
            IAsset asset = null;
            lock (_assetDict)
                _assetDict.TryGetValue(id, out asset);

            return asset as SpriteFontAsset;
        }

        public Texture2DAsset GetTexture2DAsset(string id)
        {
            IAsset asset = null;
            lock (_assetDict)
                _assetDict.TryGetValue(id, out asset);

            return asset as Texture2DAsset;
        }

        public void LoadAsset(string filePath, string id, string batchId)
        {
            IAssetBatch batch = null;
            lock (_assetBatches)
                _assetBatches.TryGetValue(batchId, out batch);

            if (ReferenceEquals(null, batch))
                return;

            var asset = _assetLoader.LoadAsset(filePath, id, batch.Content);

            lock (batch)
                batch.AddAsset(asset);

            lock (_assetDict)
                _assetDict.Add(id, asset);
        }
    
        public void LoadAssetBatch(string filePath, string id)
        {
            var batch = _assetLoader.LoadBatch(filePath, id, _serviceProvider);
            lock (_assetBatches)
            {
                if (!_assetBatches.ContainsKey(id))
                    _assetBatches.Add(id, batch);
            }
        }

        public void LoadBatchAssets(string id)
        {
            IAssetBatch batch = null;
            lock (_assetBatches)
                _assetBatches.TryGetValue(id, out batch);

            if (ReferenceEquals(null, batch))
                return;

            foreach (var assetFile in batch.FileIdDictionary.Keys)
            {
                var idList = batch.FileIdDictionary[assetFile];
                foreach (var i in idList)
                {
                    IAsset asset = null;
                    lock (_assetDict)
                        _assetDict.TryGetValue(i, out asset);

                    if(ReferenceEquals(null, asset))
                        asset = _assetLoader.LoadAsset(assetFile, i, batch.Content);
                    
                    lock(batch)
                        batch.AddAsset(asset);

                    lock (_assetDict)
                    {
                        if (!_assetDict.ContainsKey(i))
                            _assetDict.Add(i, asset);
                    }
                }

            }
        }

        public void Recycle()
        {
            lock (_assetDict)
                _assetDict = new Dictionary<string, IAsset>();
        }

        public void UnloadAll()
        {
            lock (_assetBatches)
            {
                foreach (var batch in _assetBatches.Values)
                {
                    lock (batch)
                        batch.Unload();
                }
            }
        }

        public void UnloadBatch(string id)
        {
            IAssetBatch batch = null;
            lock (_assetBatches)
                _assetBatches.TryGetValue(id, out batch);

            if (ReferenceEquals(null, batch))
            {
                lock (batch)
                    batch.Unload();
            }
        }
    }
}
