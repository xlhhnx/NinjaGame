using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace NinjaGame.Assets.Batches
{
    public class AssetBatch : IAssetBatch
    {
        public string Id
        {
            get { return _id; }
        }

        public ContentManager Content { get { return _contentManager; } }

        public Dictionary<string, List<string>> FileIdDictionary
        {
            get { return _fileIdDictionary; }
            set { _fileIdDictionary = value; }
        }

        public List<IAsset> Assets
        {
            get { return _assets; }
            set { _assets = value; }
        }

        protected string _id;
        protected bool _loaded;
        protected ContentManager _contentManager;
        // FilePath => List<Id>
        protected Dictionary<string, List<string>> _fileIdDictionary;
        protected List<IAsset> _assets;


        public AssetBatch(string id, IServiceProvider serviceProvider)
        {
            _id = id;
            _contentManager = new ContentManager(serviceProvider);
            _fileIdDictionary = new Dictionary<string, List<string>>();
            _assets = new List<IAsset>();
        }

        public AssetBatch(string id, IServiceProvider serviceProvider, string rootDirectory)
        {
            _id = id;
            _contentManager = new ContentManager(serviceProvider, rootDirectory);
            _fileIdDictionary = new Dictionary<string, List<string>>();
            _assets = new List<IAsset>();
        }

        public void AddAssetDefinition(string filePath, string assetId)
        {
            if (_fileIdDictionary.ContainsKey(filePath))
            {
                _fileIdDictionary[filePath].Add(assetId);
            }
            else
            {
                _fileIdDictionary.Add(filePath, new List<string>() { assetId });
            }
        }

        public void AddAsset(IAsset asset)
        {
            if (!_assets.Contains(asset))
            {
                _assets.Add(asset);
            }
        }

        public List<Tuple<string, string>> GetAllFileIdPairs()
        {
            var pairs = new List<Tuple<string, string>>();
            foreach (var file in _fileIdDictionary.Keys)
            {
                foreach (var id in _fileIdDictionary[file])
                    pairs.Add(new Tuple<string, string>(file, id));
            }
            return pairs;
        }

        public void Unload()
        {
            _loaded = false;
            foreach (IAsset a in _assets)
            {
                a.Unload();
            }
            _contentManager.Unload();
        }
    }
}