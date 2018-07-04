using NinjaGame.Assets.Config;
using NinjaGame.Assets.Management;
using NinjaGame.Common.Extensions;
using NinjaGame.Graphics2D.Assets;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NinjaGame.Common;
using NinjaGame.Common.Loading;

namespace NinjaGame.Graphics2D.Loading
{
    public class Graphic2DLoader : IGraphic2DLoader
    {
        protected IAssetManager _assetManager;
        protected Dictionary<string, string> _nameIdDict;
        protected Dictionary<string, Definition<GraphicType>> _definitions;

        public Graphic2DLoader(IAssetManager assetManager)
        {
            _assetManager = assetManager;
            _nameIdDict = new Dictionary<string, string>();
            _definitions = new Dictionary<string, Definition<GraphicType>>();
        }

        public IGraphic2D LoadGraphic(IDefinition<GraphicType> stagedGraphic)
        {
            IGraphic2D graphic = null;
            switch (stagedGraphic.Type)
            {
                case (GraphicType.Image):
                    graphic = ParseImage(stagedGraphic.FilePath, stagedGraphic.Id);
                    break;
                case (GraphicType.Effect):
                    graphic = ParseEffect(stagedGraphic.FilePath, stagedGraphic.Id);
                    break;
                case (GraphicType.Sprite):
                    graphic = ParseSprite(stagedGraphic.FilePath, stagedGraphic.Id);
                    break;
                case (GraphicType.Text):
                    graphic = ParseText(stagedGraphic.FilePath, stagedGraphic.Id);
                    break;
            }
            return graphic;
        }        

        public ILoadBatch<IGraphic2D> LoadGraphicBatch(string filePath, string id)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("graphicbatch") && l.Contains($"id={id}"))
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
                        name = pair[1].Trim().ToLower();
                }
                else if (work[i].Contains(':'))
                {
                    var pair = work[i].Split(':');
                    var ids = pair[1].Trim()
                                   .Trim('{', '}')
                                   .Split(',')
                                   .Select(l => l.Trim())
                                   .ToList();

                    fileIdDict.Add(pair[0].Trim(), ids);
                }
                
            }

            var batch = new LoadBatch<IGraphic2D>(id, name);
            batch.FileIdDict = fileIdDict;
            return batch;
        }

        public IDefinition<GraphicType> LoadDefinition(string filePath, string id)
        {
            _definitions.TryGetValue(id, out var def);
            if (!(def is null))
                return def;

            var line = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("graphic") && l.Contains($"id={id}"))
                                .FirstOrDefault();

            if (line.Length == 0)
                return null;

            var work = line.Split(';');

            var name = "";
            GraphicType type = GraphicType.None;
            for (int i = 0; i < work.Length; i++)
            {
                var pair = work[i].Split('=');
                switch (pair[0].Trim().ToLower())
                {
                    case ("graphic"):
                        type = ParseType(pair[1].Trim().ToLower());
                        break;
                    case ("name"):
                        name = pair[1].Trim().ToLower();
                        break;
                }
            }

            if (type == GraphicType.None)
                return null;

            var definition = new Definition<GraphicType>(id, name, filePath, type);

            if (!(definition is null) && !_definitions.ContainsKey(id))
                _definitions.Add(id, definition);

            if (!_nameIdDict.ContainsKey(definition.Name))
                _nameIdDict.Add(definition.Name, definition.Id);

            return definition;
        }

        public IGraphic2D LoadGraphic(string filePath, string id)
        {
            var definition = LoadDefinition(filePath, id);

            if (definition is null)
                return null;

            var graphic = LoadGraphic(definition);
            return graphic;
        }

        public IGraphic2D LoadGraphicByName(string filePath, string name)
        {
            var definition = LoadDefinitionByName(filePath, name);

            if (definition is null)
                return null;

            var graphic = LoadGraphic(definition);
            return graphic;
        }

        public ILoadBatch<IGraphic2D> LoadGraphicBatchByName(string filePath, string name)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("graphicbatch") && l.Contains($"name={name}"))
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
                                   .Trim('{', '}')
                                   .Split(',')
                                   .Select(l => l.Trim())
                                   .ToList();

                    fileIdDict.Add(pair[0].Trim(), ids);
                }                
            }

            var batch = new LoadBatch<IGraphic2D>(id, name);
            batch.FileIdDict = fileIdDict;
            return batch;
        }

        public IDefinition<GraphicType> LoadDefinitionByName(string filePath, string name)
        {
            _nameIdDict.TryGetValue(name, out var tmpId);
            if (!(tmpId is null))
            {
                _definitions.TryGetValue(tmpId, out var def);
                if (!(def is null))
                    return def;
            }

            var line = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("graphic") && l.Contains($"name={name}"))
                                .FirstOrDefault();

            if (line.Length == 0)
                return null;

            var work = line.Split(';');

            var id = "";
            GraphicType type = GraphicType.None;
            for (int i = 0; i < work.Length; i++)
            {
                var pair = work[i].Split('=');
                switch (pair[0].Trim().ToLower())
                {
                    case ("graphic"):
                        type = ParseType(pair[1].Trim().ToLower());
                        break;
                    case ("id"):
                        id = pair[1].Trim();
                        break;
                }
            }

            if (type == GraphicType.None)
                return null;

            var definition = new Definition<GraphicType>(id, name, filePath, type);          
            
            if (!(definition is null) && !_definitions.ContainsKey(id))
                _definitions.Add(id, definition);
            
            if (!_nameIdDict.ContainsKey(definition.Name))
                _nameIdDict.Add(definition.Name, definition.Id);

            return definition;
        }

        private Text ParseText(string filePath, string id)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.Contains("graphic") && l.Contains($"id={id}"))
                                .FirstOrDefault();

            var parameters = definition.Split(';')
                                .Where(p => p.Contains("="))
                                .ToList();

            var name = "";
            var spriteFontAssetId = "";
            Color color = new Color();
            Color disabledColor = new Color();
            Vector2 positionOffset = new Vector2();
            Vector2 dimensions = new Vector2();
            var fullText = "";

            foreach (var p in parameters)
            {
                var pair = p.Split('=');
                switch (pair[0].Trim().ToLower())
                {
                    case ("name"):
                        name = pair[1].ToLower();
                        break;
                    case ("spritefontasset"):
                        spriteFontAssetId = pair[1].Trim().ToLower();
                        break;
                    case ("color"):
                        color = pair[1].Trim().ToLower().ToColor();
                        break;
                    case ("disabledcolor"):
                        disabledColor = pair[1].Trim().ToLower().ToColor();
                        break;
                    case ("positionoffset"):
                        positionOffset = pair[1].Trim().ToLower().ToVector2();
                        break;
                    case ("dimensions"):
                        dimensions = pair[1].Trim().ToLower().ToVector2();
                        break;
                    case ("fulltext"):
                        fullText = pair[1];
                        break;
                }
            }

            var spriteFontAsset = _assetManager.GetSpriteFontAsset(spriteFontAssetId);
            if (spriteFontAsset == null)
                spriteFontAsset = AssetConfig.DefaultSpriteFontAsset;

            return new Text(id, name, spriteFontAsset, color, disabledColor, positionOffset, dimensions, fullText);
        }

        private Image ParseImage(string filePath, string id)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.Contains("graphic") && l.Contains($"id={id}"))
                                .FirstOrDefault();

            var parameters = definition.Split(';')
                                .Where(p => p.Contains("="))
                                .ToList();

            var name = "";
            var texture2DAssetId = "";
            Color color = new Color();
            Vector2 sourcePosition = new Vector2();
            Vector2 sourceDimensions = new Vector2();
            Vector2 positionOffset = new Vector2();
            Vector2 dimensions = new Vector2();
            var enabled = true;
            var visible = true;

            foreach (var p in parameters)
            {
                var pair = p.Split('=');
                switch (pair[0].Trim().ToLower())
                {
                    case ("name"):
                        name = pair[1].ToLower();
                        break;
                    case ("texture2dasset"):
                        texture2DAssetId = pair[1].Trim().ToLower();
                        break;
                    case ("color"):
                        color = pair[1].Trim().ToLower().ToColor();
                        break;
                    case ("sourceposition"):
                        sourcePosition = pair[1].Trim().ToLower().ToVector2();
                        break;
                    case ("sourcedimensions"):
                        sourceDimensions = pair[1].Trim().ToLower().ToVector2();
                        break;
                    case ("positionoffset"):
                        positionOffset = pair[1].Trim().ToLower().ToVector2();
                        break;
                    case ("dimensions"):
                        dimensions = pair[1].Trim().ToLower().ToVector2();
                        break;
                    case ("enabled"):
                        enabled = pair[1].Trim().ToLower().ToBool();
                        break;
                    case ("visible"):
                        visible = pair[1].Trim().ToLower().ToBool();
                        break;
                }
            }

            var texture2DAsset = _assetManager.GetTexture2DAsset(texture2DAssetId);
            if (texture2DAsset is null)
                texture2DAsset = AssetConfig.DefaultTexture2DAsset;

            return new Image(id, name, texture2DAsset, sourcePosition, sourceDimensions, color, positionOffset, dimensions, enabled, visible);
        }

        private Sprite ParseSprite(string filePath, string id)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.Contains("graphic") && l.Contains($"id={id}"))
                                .FirstOrDefault();

            var parameters = definition.Split(';')
                                .Where(p => p.Contains("="))
                                .ToList();

            var name = "";
            var texture2DAssetId = "";
            Color color = new Color();
            Vector2 sourcePosition = new Vector2();
            Vector2 sourceDimensions = new Vector2();
            Vector2 positionOffset = new Vector2();
            Vector2 dimensions = new Vector2();
            var rows = 1;
            var columns = 1;
            var frameTime = -1;
            var looping = true;
            var enabled = true;
            var visible = true;

            foreach (var p in parameters)
            {
                var pair = p.Split('=');
                switch (pair[0].Trim().ToLower())
                {
                    case ("name"):

                        name = pair[1].ToLower();
                        break;
                    case ("texture2dasset"):
                        texture2DAssetId = pair[1].Trim().ToLower();
                        break;
                    case ("color"):
                        color = pair[1].Trim().ToLower().ToColor();
                        break;
                    case ("sourceposition"):
                        sourcePosition = pair[1].Trim().ToLower().ToVector2();
                        break;
                    case ("sourcedimensions"):
                        sourceDimensions = pair[1].Trim().ToLower().ToVector2();
                        break;
                    case ("positionoffset"):
                        positionOffset = pair[1].Trim().ToLower().ToVector2();
                        break;
                    case ("dimensions"):
                        dimensions = pair[1].Trim().ToLower().ToVector2();
                        break;
                    case ("rows"):
                        rows = pair[1].Trim().ToLower().ToInt32();
                        break;
                    case ("columns"):
                        columns = pair[1].Trim().ToLower().ToInt32();
                        break;
                    case ("frametime"):
                        frameTime = pair[1].Trim().ToLower().ToInt32();
                        break;
                    case ("looping"):
                        looping = pair[1].Trim().ToLower().ToBool();
                        break;
                    case ("enabled"):
                        enabled = pair[1].Trim().ToLower().ToBool();
                        break;
                    case ("visible"):
                        visible = pair[1].Trim().ToLower().ToBool();
                        break;
                }
            }

            var texture2DAsset = _assetManager.GetTexture2DAsset(texture2DAssetId);
            if (texture2DAsset == null)
                texture2DAsset = AssetConfig.DefaultTexture2DAsset;

            return new Sprite(id, name, texture2DAsset, sourcePosition, sourceDimensions, color, positionOffset, dimensions, rows, columns, frameTime, looping, enabled, visible);
        }

        private Effect ParseEffect(string filePath, string id)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.Contains("graphic") && l.Contains($"id={id}"))
                                .FirstOrDefault();

            var parameters = definition.Split(';')
                                .Where(p => p.Contains("="))
                                .ToList();

            var name = "";

            return new Effect(id, name);
        }
        
        private GraphicType ParseType(string typeString)
        {
            switch (typeString.Trim().ToLower())
            {
                case ("effect"):
                    return GraphicType.Effect;
                case ("image"):
                    return GraphicType.Image;
                case ("sprite"):
                    return GraphicType.Sprite;
                case ("text"):
                    return GraphicType.Text;
            }
            return GraphicType.None;
        }
    }
}