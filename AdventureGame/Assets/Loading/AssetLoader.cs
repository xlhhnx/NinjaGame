using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NAudio.Wave;
using Newtonsoft.Json;
using NinjaGame.Assets.Batches;
using NinjaGame.Batches.Loading;
using NinjaGame.Loading;
using NinjaGame.Loading.Definitions;
using System;
using System.Collections.Generic;
using System.IO;

namespace NinjaGame.Assets.Loading
{
    public class AssetLoader : IAssetLoader
    {
        public string RootDirectory { get; set; }
        public IServiceProvider ServiceProvider { get; set; }

        public AssetLoader(string rootDirectory, IServiceProvider serviceProvider)
        {
            RootDirectory = rootDirectory;
            ServiceProvider = serviceProvider;
        }

        public List<IAsset> LoadAssets(IAssetBatch batch)
        {
            var assets = new List<IAsset>();
            foreach (var file in batch.Files)
            {
                var tmpAssets = LoadAssets(file, batch.Content);

                if(tmpAssets != null)
                    assets.AddRange(tmpAssets);
            }
            return assets;
        }

        public List<IAssetBatch> LoadBatches(string filePath)
        {
            var contents = "";
            try
            {
                contents = File.ReadAllText(filePath);
            }
            catch (FileNotFoundException ex)
            {
                // TODO: Log ex
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                // TODO: Log ex
                Console.WriteLine(ex.Message);
                return null;
            }

            var batches = new List<IAssetBatch>();
            try
            {
                var tmpBatches = JsonConvert.DeserializeObject<List<AssetBatch>>(contents);
                foreach (var b in tmpBatches)
                    batches.Add(b);
            }
            catch (JsonException ex)
            {
                // TODO: Log ex
                Console.WriteLine(ex.Message);
                return null;
            }
            return batches;
        }

        private List<IAsset> LoadAssets(string fileName, ContentManager content)
        {
            var fileContents = "";
            try
            {
                fileContents = File.ReadAllText(fileName);
            }
            catch (FileNotFoundException ex)
            {
                // TODO: Log ex
                Console.WriteLine(ex.Message);
                return null;
            }
                        
            var definitions = new List<AssetDefinition>();
            try
            {
                var tmpDefs = JsonConvert.DeserializeObject<List<Definition<AssetType>>>(fileContents);
                foreach (var t in tmpDefs)
                    definitions.Add(
                        new AssetDefinition(t.Id, t.Name, t.AssetFile, t.Type, content)
                    );
            }
            catch (JsonException ex)
            {
                // TODO: Log ex
                Console.WriteLine(ex.Message);
                return null;
            }

            var assets = new List<IAsset>();
            foreach (var def in definitions)
            {
                IAsset asset = null;
                switch (def.Type)
                {
                    case (AssetType.AudioAsset):
                        asset = new AudioAsset(def.Id, def.Name, new AudioFileReader(def.AssetFile));
                        break;
                    case (AssetType.SpriteFontAsset):
                        {
                            var sf = def.Content.Load<SpriteFont>(def.AssetFile);
                            asset = new SpriteFontAsset(def.Id, def.Name, sf);
                        }
                        break;
                    case (AssetType.Texture2DAsset):
                        {
                            var td = def.Content.Load<Texture2D>(def.AssetFile);
                            asset = new Texture2DAsset(def.Id, def.Name, td);
                        }
                        break;
                }
                if (!(asset is null))
                    assets.Add(asset);
            }

            return assets;
        }
    }
}
