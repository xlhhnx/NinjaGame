using NinjaGame.Assets.Config;
using NinjaGame.Assets.Management;
using NinjaGame.Common.Extensions;
using NinjaGame.Graphics2D.Assets;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NinjaGame.Graphics2D.Batches;
using System.Threading.Tasks;

namespace NinjaGame.Graphics2D.Loading
{
    public class Graphic2DLoader : IGraphic2DLoader
    {
        protected IAssetManager _assetManager;

        public Graphic2DLoader(IAssetManager assetManager)
        {
            _assetManager = assetManager;
        }

        public IGraphic2D LoadGraphic(StagedGraphic stagedGraphic)
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

        public IGraphic2DBatch LoadGraphicBatch(string filePath, string id)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("graphicbatch") && l.Contains(id))
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

            var batch = new Graphic2DBatch(id);
            batch.FileIdDict = fileIdDict;
            return batch;
        }

        public StagedGraphic StageGraphic(string filePath, string id)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.ToLower().StartsWith("graphic") && l.Contains(id))
                                .FirstOrDefault();

            if (definition.Length == 0)
                return new StagedGraphic();

            var work = definition.Split(';');
            
            GraphicType type = GraphicType.None;
            for (int i = 0; i < work.Length; i++)
            {
                var pair = work[i].Split('=');
                if (pair[0].Trim().ToLower() == "graphic")
                {
                    type = ParseType(pair[1].Trim().ToLower());
                    break;
                }
            }

            if (type == GraphicType.None)
                return new StagedGraphic();

            var stagedGraphic = new StagedGraphic(id, filePath, type);
            return stagedGraphic;
        }

        public IGraphic2D LoadGraphic(string filePath, string id)
        {
            var stagedGraphic = StageGraphic(filePath, id);

            if (stagedGraphic.FilePath == string.Empty || stagedGraphic.Id == string.Empty || stagedGraphic.Type == GraphicType.None)
                return null;

            var graphic = LoadGraphic(stagedGraphic);
            return graphic;
        }

        private Text ParseText(string filePath, string id)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.Contains("graphic") && l.Contains(id))
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
                        {
                            name = pair[1].ToLower();
                        }
                        break;
                    case ("spritefontassetid"):
                        {
                            spriteFontAssetId = pair[1].Trim().ToLower();
                        }
                        break;
                    case ("color"):
                        {
                            color = pair[1].Trim().ToLower().ToColor();
                        }
                        break;
                    case ("disabledcolor"):
                        {
                            disabledColor = pair[1].Trim().ToLower().ToColor();
                        }
                        break;
                    case ("positionoffset"):
                        {
                            positionOffset = pair[1].Trim().ToLower().ToVector2();
                        }
                        break;
                    case ("dimensions"):
                        {
                            dimensions = pair[1].Trim().ToLower().ToVector2();
                        }
                        break;
                    case ("fulltext"):
                        {
                            fullText = pair[1].ToLower();
                        }
                        break;
                }
            }

            var spriteFontAsset = _assetManager.GetSpriteFontAsset(spriteFontAssetId);
            if (spriteFontAsset == null)
                spriteFontAsset = AssetConfig.DefaultSpriteFontAsset;

            return new Text(id, spriteFontAsset, color, disabledColor, positionOffset, dimensions, fullText);
        }

        private Image ParseImage(string filePath, string id)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.Contains("graphic") && l.Contains(id))
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
                        {
                            name = pair[1].ToLower();
                        }
                        break;
                    case ("texture2dassetid"):
                        {
                            texture2DAssetId = pair[1].Trim().ToLower();
                        }
                        break;
                    case ("color"):
                        {
                            color = pair[1].Trim().ToLower().ToColor();
                        }
                        break;
                    case ("sourceposition"):
                        {
                            sourcePosition = pair[1].Trim().ToLower().ToVector2();
                        }
                        break;
                    case ("sourcedimensions"):
                        {
                            sourceDimensions = pair[1].Trim().ToLower().ToVector2();
                        }
                        break;
                    case ("positionoffset"):
                        {
                            positionOffset = pair[1].Trim().ToLower().ToVector2();
                        }
                        break;
                    case ("dimensions"):
                        {
                            dimensions = pair[1].Trim().ToLower().ToVector2();
                        }
                        break;
                    case ("enabled"):
                        {
                            enabled = pair[1].Trim().ToLower().ToBool();
                        }
                        break;
                    case ("visible"):
                        {
                            visible = pair[1].Trim().ToLower().ToBool();
                        }
                        break;
                }
            }

            var texture2DAsset = _assetManager.GetTexture2DAsset(texture2DAssetId);
            if (texture2DAsset is null)
                texture2DAsset = AssetConfig.DefaultTexture2DAsset;

            return new Image(id, texture2DAsset, sourcePosition, sourceDimensions, color, positionOffset, dimensions, enabled, visible);
        }

        private Sprite ParseSprite(string filePath, string id)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.Contains("graphic") && l.Contains(id))
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
                        {
                            name = pair[1].ToLower();
                        }
                        break;
                    case ("texture2dassetid"):
                        {
                            texture2DAssetId = pair[1].Trim().ToLower();
                        }
                        break;
                    case ("color"):
                        {
                            color = pair[1].Trim().ToLower().ToColor();
                        }
                        break;
                    case ("sourceposition"):
                        {
                            sourcePosition = pair[1].Trim().ToLower().ToVector2();
                        }
                        break;
                    case ("sourcedimensions"):
                        {
                            sourceDimensions = pair[1].Trim().ToLower().ToVector2();
                        }
                        break;
                    case ("positionoffset"):
                        {
                            positionOffset = pair[1].Trim().ToLower().ToVector2();
                        }
                        break;
                    case ("dimensions"):
                        {
                            dimensions = pair[1].Trim().ToLower().ToVector2();
                        }
                        break;
                    case ("rows"):
                        {
                            rows = pair[1].Trim().ToLower().ToInt32();
                        }
                        break;
                    case ("columns"):
                        {
                            columns = pair[1].Trim().ToLower().ToInt32();
                        }
                        break;
                    case ("frametime"):
                        {
                            frameTime = pair[1].Trim().ToLower().ToInt32();
                        }
                        break;
                    case ("looping"):
                        {
                            looping = pair[1].Trim().ToLower().ToBool();
                        }
                        break;
                    case ("enabled"):
                        {
                            enabled = pair[1].Trim().ToLower().ToBool();
                        }
                        break;
                    case ("visible"):
                        {
                            visible = pair[1].Trim().ToLower().ToBool();
                        }
                        break;
                }
            }

            var texture2DAsset = _assetManager.GetTexture2DAsset(texture2DAssetId);
            if (texture2DAsset == null)
                texture2DAsset = AssetConfig.DefaultTexture2DAsset;

            return new Sprite(id, texture2DAsset, sourcePosition, sourceDimensions, color, positionOffset, dimensions, rows, columns, frameTime, looping, enabled, visible);
        }

        private Effect ParseEffect(string filePath, string id)
        {
            var definition = File.ReadAllLines(filePath).Where(l => l.Length > 0)
                                .Where(l => l.Contains("graphic") && l.Contains(id))
                                .FirstOrDefault();

            var parameters = definition.Split(';')
                                .Where(p => p.Contains("="))
                                .ToList();

            return new Effect(id);
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