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
                    asset = new AudioAsset(stagedAsset.Id, new AudioFileReader(stagedAsset.FilePath));
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

        public IAsset LoadAssetByName(string filePath, string name, ContentManager contentManager)
        {
            var stagedAsset = StageAssetByName(filePath, name, contentManager);

            if (stagedAsset.Content == null || stagedAsset.Type == AssetType.None || stagedAsset.Id == string.Empty || stagedAsset.FilePath == string.Empty)
                return null;

            var asset = LoadAsset(stagedAsset);
            return asset;
        }

        public IAssetBatch LoadBatch(string filePath, string id, IServiceProvider serviceProvider)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("assetbatch") && l.Contains($"id={id}"))
                                .FirstOrDefault();

            var work = definition.Split(';');

            var name = "";
            var fileIdDict = new Dictionary<string, List<string>>();
            for (int i = 0; i < work.Length; i++)
            {
                if (work[i].Contains('='))
                {
                    var pair = work[i].Split('=');
                    if (pair[0].Trim().ToLower() == "name")
                        name = pair[1].Trim();
                }
                else if (work[i].Contains(':'))
                {
                    var pair = work[i].Split(':');
                    var ids = pair[1].Trim()
                                .Trim('{','}')
                                .Split(',')
                                .Select(l => l.Trim())
                                .ToList();

                    fileIdDict.Add(pair[0].Trim(), ids);
                }
            }

            var batch = new AssetBatch(id, name, serviceProvider) { FileIdDict = fileIdDict };
            return batch;
        }

        public IAssetBatch LoadBatchByName(string filePath, string name, IServiceProvider serviceProvider)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("assetbatch") && l.Contains($"name={name}"))
                                .FirstOrDefault();

            var work = definition.Split(';');

            var id = "";
            var fileIdDict = new Dictionary<string, List<string>>();
            for (int i = 0; i < work.Length; i++)
            {
                if (work[i].Contains('='))
                {
                    var pair = work[i].Split('=');
                    if (pair[0].Trim().ToLower() == "id")
                        id = pair[1].Trim();
                }
                else if (work[i].Contains(':'))
                {
                    var pair = work[i].Split(':');
                    var ids = pair[1].Trim()
                                .Trim('{','}')
                                .Split(',')
                                .Select(l => l.Trim())
                                .ToList();

                    fileIdDict.Add(pair[0].Trim(), ids);
                }
            }

            var batch = new AssetBatch(id, name, serviceProvider) { FileIdDict = fileIdDict };
            return batch;
        }

        public StagedAsset StageAsset(string filePath, string id, ContentManager contentManager)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("asset") && l.Contains($"id={id}"))
                                .FirstOrDefault();

            if (definition.Length == 0)
                return new StagedAsset();

            var work = definition.Split(';');

            string fileName = "";
            AssetType type = AssetType.None;
            for (int i = 0; i < work.Length; i++)
            {
                var pair = work[i].Split('=');
                switch (pair[0].Trim().ToLower())
                {
                    case ("filepath"):
                        fileName = pair[1].Trim();
                        break;
                    case ("asset"):
                        type = ParseType(pair[1].Trim().ToLower());
                        break;
                }
            }

            if (fileName == string.Empty || type == AssetType.None)
                return new StagedAsset();

            var stagedAsset = new StagedAsset(id, fileName, type, contentManager);
            return stagedAsset;
        }

        public StagedAsset StageAssetByName(string filePath, string name, ContentManager contentManager)
        {
            throw new NotImplementedException();
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

