using NinjaGame.Common.Extensions;
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

        protected string _rootDirectory;
        protected IAssetLoader _assetLoader;
        protected IServiceProvider _serviceProvider;
        protected Dictionary<string, IAssetBatch> _assetBatches;
        protected Dictionary<string, IAsset> _assetDict;


        public AssetManager(string rootDirectory, IServiceProvider serviceProvider)
        {
            _rootDirectory = rootDirectory;
            _serviceProvider = serviceProvider;
            
            Loader = new AssetLoader(_rootDirectory, _serviceProvider);
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

        public void LoadAssetBatches(string filePath)
        {
            var batches = Loader.LoadBatches(filePath);
            
            if (batches is null)
                return;

            foreach (var b in batches)
            {
                if (!_assetBatches.ContainsKey(b.Id))
                {
                    _assetBatches.Add(b.Id, b);
                    AssetBatchLoadedEvent(b);
                }
            }
        }

        public async void LoadAssetBatchesAsync(string filePath)
        {
            var task = Task.Run(() => Loader.LoadBatches(filePath));
            var batches = await task;

            if (batches is null)
                return;

            foreach (var b in batches)
            {
                if (!_assetBatches.ContainsKey(b.Id))
                {
                    _assetBatches.Add(b.Id, b);
                    AssetBatchLoadedEvent(b);
                }
            }
        }

        public void LoadBatchAssets(IAssetBatch batch)
        {
            var assets = Loader.LoadAssets(batch);
            foreach (var a in assets)
            {
                if (!batch.Values.Contains(a))
                    batch.Values.Add(a);

                if (!_assetDict.ContainsKey(a.Id))
                {
                    _assetDict.Add(a.Id, a);
                    AssetLoadedEvent(a);
                }
            }
            BatchAssetsLoadedEvent(batch);
        }

        public void LoadBatchAssetsById(string id)
        {
            if (_assetBatches.TryGetValue(id, out var batch))
            {
                LoadBatchAssets(batch);
            }
        }

        public void LoadBatchAssetsByName(string name)
        {
            foreach (var id in _assetBatches.Keys)
            {
                if (_assetBatches[id].Name == name)
                {
                    LoadBatchAssets(_assetBatches[id]);
                    break;
                }
            }
        }

        public async void LoadBatchAssetsAsync(IAssetBatch batch)
        {
            var task = Task.Run(() => Loader.LoadAssets(batch));
            var assets = await task;
            foreach (var a in assets)
            {
                if (!batch.Values.Contains(a))
                    batch.Values.Add(a);

                if (!_assetDict.ContainsKey(a.Id))
                {
                    _assetDict.Add(a.Id, a);
                    AssetLoadedEvent(a);
                }
            }
        }

        public void LoadBatchAssetsAsyncById(string id)
        {
            if (_assetBatches.TryGetValue(id, out var batch))
            {
                LoadBatchAssetsAsync(batch);
            }
        }

        public void LoadBatchAssetsAsyncByName(string name)
        {
            foreach (var id in _assetBatches.Keys)
            {
                if (_assetBatches[id].Name == name)
                {
                    LoadBatchAssetsAsync(_assetBatches[id]);
                    break;
                }
            }
        }
    }
}
