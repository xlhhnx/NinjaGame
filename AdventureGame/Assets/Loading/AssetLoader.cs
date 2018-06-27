using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NinjaGame.AssetManagement;
using NinjaGame.Assets.Batches;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NAudio.Wave;

namespace NinjaGame.Assets.Loading
{
    public class AssetLoader : IAssetLoader
    {
        public IAsset LoadAsset(string filePath, string id, ContentManager contentManager)
        {
            var stagedAsset = StageAsset(filePath, id, contentManager);

            if (stagedAsset.Content == null || stagedAsset.Type == AssetType.None || stagedAsset.Id == string.Empty || stagedAsset.FilePath == string.Empty)
                return null;

            var asset = LoadAsset(stagedAsset);
            return asset;
        }

        public IAsset LoadAsset(StagedAsset stagedAsset)
        {
            IAsset asset = null;

            switch (stagedAsset.Type)
            {
                case (AssetType.AudioAsset):
                    {
                        asset = new AudioAsset(stagedAsset.Id, new AudioFileReader(stagedAsset.FilePath));
                    }
                    break;
                case (AssetType.SpriteFontAsset):
                    {
                        SpriteFont sf;
                        lock (stagedAsset.Content)
                            sf = stagedAsset.Content.Load<SpriteFont>(stagedAsset.FilePath);

                        asset = new SpriteFontAsset(stagedAsset.Id, sf);
                    }
                    break;
                case (AssetType.Texture2DAsset):
                    {
                        Texture2D td;
                        lock (stagedAsset.Content)
                            td = stagedAsset.Content.Load<Texture2D>(stagedAsset.FilePath);

                        asset = new Texture2DAsset(stagedAsset.Id, td);
                    }
                    break;
            }
            return asset;
        }

        public IAssetBatch LoadBatch(string filePath, string id, IServiceProvider serviceProvider)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("batch") && l.Contains(id))
                                .FirstOrDefault();

            var work = definition.Split(';');

            var fileIdDict = new Dictionary<string, List<string>>();
            for (int i = 0; i < work.Length; i++)
            {
                var pair = work[i].Split('=');
                if (pair.Length > 1 && pair[0].Trim().Length > 0)
                {
                    var ids = pair[1].Trim()
                                .Trim('{','}')
                                .Split(',')
                                .Select(l => l.Trim())
                                .ToList();

                    fileIdDict.Add(pair[0].Trim(), ids);
                }
            }

            var batch = new AssetBatch(id, serviceProvider);
            batch.FileIdDictionary = fileIdDict;
            return batch;
        }

        public StagedAsset StageAsset(string filePath, string id, ContentManager contentManager)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("asset") && l.Contains(id))
                                .FirstOrDefault();

            if (definition.Length == 0)
                return new StagedAsset();

            var work = definition.Split(';');
            var typeString = work[0].Split('=')[1].Trim().ToLower();
            var type = ParseType(typeString);

            if (type == AssetType.None)
                return new StagedAsset();

            string fileName = "";
            for (int i = 1; i < work.Length; i++)
            {
                var pair = work[i].Split('=');
                if (pair[0].Trim().ToLower() == "filepath")
                    fileName = pair[1].Trim();
            }

            var stagedAsset = new StagedAsset(id, fileName, type, contentManager);
            return stagedAsset;
        }

        private AssetType ParseType(string typeString)
        {
            switch (typeString.Trim().ToLower())
            {
                case ("texture2d"):
                    return AssetType.Texture2DAsset;
                case ("spritefont"):
                    return AssetType.SpriteFontAsset;
                case ("audio"):
                    return AssetType.AudioAsset;
            }
            return AssetType.None;
        }
    }
}

