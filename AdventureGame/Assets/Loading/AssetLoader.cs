﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NinjaGame.Assets.Batches;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NAudio.Wave;

namespace NinjaGame.Assets.Loading
{
    public class AssetLoader : IAssetLoader
    {
        protected Dictionary<string, string> _nameIdDict;
        protected Dictionary<string, AssetDefinition> _definitions;

        public AssetLoader()
        {
            _nameIdDict = new Dictionary<string, string>();
            _definitions = new Dictionary<string, AssetDefinition>();
        }

        public IAsset LoadAsset(string filePath, string id, ContentManager contentManager)
        {
            var definition = LoadDefinition(filePath, id, contentManager);

            if (definition is null)
                return null;

            var asset = LoadAsset(definition);
            return asset;
        }

        public IAsset LoadAsset(AssetDefinition definition)
        {
            IAsset asset = null;

            switch (definition.Type)
            {
                case (AssetType.AudioAsset):
                    asset = new AudioAsset(definition.Id, definition.Name, new AudioFileReader(definition.FilePath));
                    break;
                case (AssetType.SpriteFontAsset):
                    {
                        var sf = definition.Content.Load<SpriteFont>(definition.FilePath);
                        asset = new SpriteFontAsset(definition.Id, definition.Name, sf);
                    }
                    break;
                case (AssetType.Texture2DAsset):
                    {
                        var td = definition.Content.Load<Texture2D>(definition.FilePath);
                        asset = new Texture2DAsset(definition.Id, definition.Name, td);
                    }
                    break;
            }
            return asset;
        }

        public IAsset LoadAssetByName(string filePath, string name, ContentManager contentManager)
        {
            var stagedAsset = LoadDefinitionByName(filePath, name, contentManager);

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

        public AssetDefinition LoadDefinition(string filePath, string id, ContentManager contentManager)
        {
            _definitions.TryGetValue(id, out var def);
            if (!(def is null))
                return def;

            var line = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("asset") && l.Contains($"id={id}"))
                                .FirstOrDefault();

            if (line.Length == 0)
                return null;

            var work = line.Split(';');

            var name = "";
            var fileName = "";
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
                    case ("name"):
                        id = pair[1].Trim().ToLower();
                        break;
                }
            }

            if (fileName == string.Empty || type == AssetType.None)
                return null;

            var definition = new AssetDefinition(id, name, fileName, type, contentManager);

            if (!(definition is null) && !_definitions.ContainsKey(id))
                _definitions.Add(id, definition);
            
            if (!_nameIdDict.ContainsKey(definition.Name))
                _nameIdDict.Add(definition.Name, definition.Id);

            return definition;
        }

        public AssetDefinition LoadDefinitionByName(string filePath, string name, ContentManager contentManager)
        {
            _nameIdDict.TryGetValue(name, out var tmpId);
            if (!(tmpId is null))
            {
                _definitions.TryGetValue(tmpId, out var def);
                if (!(def is null))
                    return def;
            }

            var line = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("asset") && l.Contains($"name={name}"))
                                .FirstOrDefault();

            if (line.Length == 0)
                return null;

            var work = line.Split(';');

            var id = "";
            var fileName = "";
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
                    case ("id"):
                        id = pair[1].Trim();
                        break;
                }
            }

            if (fileName == string.Empty || type == AssetType.None)
                return null;

            var definition = new AssetDefinition(id, name, fileName, type, contentManager);
            
            if (!(definition is null) && !_definitions.ContainsKey(id))
                _definitions.Add(id, definition);
            
            if (!_nameIdDict.ContainsKey(definition.Name))
                _nameIdDict.Add(definition.Name, definition.Id);

            return definition;
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

