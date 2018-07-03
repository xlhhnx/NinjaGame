using NinjaGame.Common.Extensions;
using NinjaGame.AssetManagement;
using NinjaGame.Assets.Batches;
using NinjaGame.Assets.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NinjaGame.Assets.Management
{
    public class AssetManager : IAssetManager
    {
        public event Action<IAssetBatch> AssetBatchLoadedEvent = delegate { };
        public event Action<IAssetBatch> BatchAssetsLoadedEvent = delegate { };
        public event Action<IAsset> AssetLoadedEvent = delegate { };


        public IAssetLoader Loader
        {
            get { return _assetLoader; }
            set { _assetLoader = value; }
        }


        protected IAssetLoader _assetLoader;
        protected IServiceProvider _serviceProvider;
        protected Dictionary<string, IAssetBatch> _assetBatches;
        protected Dictionary<string, IAsset> _assetDict;


        public AssetManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
            _assetLoader = new AssetLoader();
            _assetBatches = new Dictionary<string, IAssetBatch>();
            _assetDict = new Dictionary<string, IAsset>();
        }

        public bool ContainsBatch(string id)
        {
            return _assetBatches.ContainsKey(id);
        }

        public bool ContainsBatch(IAssetBatch batch)
        {
            return _assetBatches.ContainsKey(batch.Id);
        }

        public IAssetBatch GetAssetBatch(string id)
        {
            _assetBatches.TryGetValue(id, out IAssetBatch batch);
            return batch;
        }

        public AudioAsset GetAudioAsset(string id)
        {
            _assetDict.TryGetValue(id, out IAsset asset);
            return asset as AudioAsset;
        }

        public SpriteFontAsset GetSpriteFontAsset(string id)
        {
            _assetDict.TryGetValue(id, out IAsset asset);
            return asset as SpriteFontAsset;
        }

        public Texture2DAsset GetTexture2DAsset(string id)
        {
            _assetDict.TryGetValue(id, out IAsset asset);
            return asset as Texture2DAsset;
        }

        public bool GraphicLoaded(string id)
        {
            return _assetDict.ContainsKey(id);
        }

        public bool GraphicLoaded(IAssetBatch graphic)
        {
            return _assetDict.ContainsKey(graphic.Id);
        }

        public void LoadAsset(string filePath, string id, string batchId)
        {
            _assetBatches.TryGetValue(batchId, out IAssetBatch batch);

            if (batch is null)
                return;

            var asset = _assetLoader.LoadAsset(filePath, id, batch.Content);
            batch.AddValue(asset);
            _assetDict.Add(id, asset);
            AssetLoadedEvent(asset);
        }

        public async void LoadAssetAsync(string filePath, string id, string batchId)
        {
            _assetBatches.TryGetValue(batchId, out IAssetBatch batch);

            if (batch is null)
                return;

            var t = Task.Run(() => _assetLoader.LoadAsset(filePath, id, batch.Content));
            var asset = await t;
            batch.AddValue(asset);

            if (!_assetDict.ContainsKey(id))
            {
                _assetDict.Add(id, asset);
                AssetLoadedEvent(asset);
            }
        }

        public void LoadAssetBatch(string filePath, string id)
        {
            var batch = _assetLoader.LoadBatch(filePath, id, _serviceProvider);

            if (batch is null)
                return;

            if (!_assetBatches.ContainsKey(id))
            {
                _assetBatches.Add(id, batch);
                AssetBatchLoadedEvent(batch);
            }
        }

        public async void LoadAssetBatchAsync(string filePath, string id)
        {
            if (_assetBatches.ContainsKey(id))
                return;

            var t = Task.Run(() => _assetLoader.LoadBatch(filePath, id, _serviceProvider));
            var batch = await t;

            if (batch is null)
                return;

            if (!_assetBatches.ContainsKey(id))
            {
                _assetBatches.Add(id, batch);
                AssetBatchLoadedEvent(batch);
            }
        }

        public void LoadBatchAssets(string id)
        {
            _assetBatches.TryGetValue(id, out IAssetBatch batch);

            if (batch is null)
                return;

            foreach (var assetFile in batch.FileIdDict.Keys)
            {
                var idList = batch.FileIdDict[assetFile];
                foreach (var i in idList)
                {
                    _assetDict.TryGetValue(i, out IAsset asset);

                    if (asset is null)
                        asset = _assetLoader.LoadAsset(assetFile, i, batch.Content);

                    batch.AddValue(asset);

                    if (!_assetDict.ContainsKey(i))
                        _assetDict.Add(i, asset);
                }
            }
        }

        public async void LoadBatchAssetsAsync(string id)
        {
            _assetBatches.TryGetValue(id, out IAssetBatch batch);

            if (batch is null)
                return;

            var tasks = new List<Task<IAsset>>();
            foreach (var assetFile in batch.FileIdDict.Keys)
            {
                var idList = batch.FileIdDict[assetFile];
                foreach (var i in idList)
                {
                    _assetDict.TryGetValue(i, out IAsset graphic);

                    if (graphic is null)
                    {
                        var t = Task.Run(() => _assetLoader.LoadAsset(assetFile, i, batch.Content));
                        tasks.Add(t);
                    }
                }
            }

            foreach (var t in tasks.InCompletionOrder())
            {
                var asset = await t;
                batch.AddValue(asset);

                if (!_assetDict.ContainsKey(asset.Id))
                {
                    _assetDict.Add(asset.Id, asset);
                    AssetLoadedEvent(asset);
                }
            }
            BatchAssetsLoadedEvent(batch);
        }

        public void Recycle()
        {
            _assetDict = new Dictionary<string, IAsset>();
        }

        public void UnloadAll()
        {
            List<string> ids;
            ids = _assetBatches.Keys.ToList();

            foreach (var i in ids)
                UnloadBatch(i);
        }

        public void UnloadBatch(string id)
        {
            _assetBatches.TryGetValue(id, out IAssetBatch batch);

            if (batch is null)
                return;

            foreach (var a in batch.Values)
                _assetDict.Remove(a.Id);

            batch.Unload();
        }
    }
}
