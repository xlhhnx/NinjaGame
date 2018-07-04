using Newtonsoft.Json;
using NinjaGame.Assets;
using NinjaGame.Batches.Loading;
using NinjaGame.Graphics2D.Assets;
using NinjaGame.Graphics2D.Definitions;
using NinjaGame.Loading;
using NinjaGame.Loading.Definitions;
using System;
using System.Collections.Generic;
using System.IO;

namespace NinjaGame.Graphics2D.Loading
{
    public class Graphic2DLoader : Loader<IGraphic2D, GraphicType>
    {
        public List<IGraphic2D> LoadGraphics(ILoadBatch<IGraphic2D> batch)
        {            
            var graphics = new List<IGraphic2D>();
            foreach (var file in batch.Files)
            {
                var tmpAssets = LoadGraphics(file);

                if(tmpAssets != null)
                    graphics.AddRange(tmpAssets);
            }
            return graphics;
        }
        
        public override List<IAsset> LoadAssets(ILoadBatch<IGraphic2D> batch)
        {
            return null;
        }

        private List<IGraphic2D> LoadGraphics(string fileName)
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

            var definitions = new List<Definition<GraphicType>>();
            try
            {
                var tmpDefs = JsonConvert.DeserializeObject<List<Definition<GraphicType>>>(fileContents);
                definitions.AddRange(tmpDefs);
            }
            catch (JsonException ex)
            {
                // TODO: Log ex
                Console.WriteLine(ex.Message);
                return null;
            }

            var graphics = new List<IGraphic2D>();
            foreach (var def in definitions)
            {
                switch (def.Type)
                {
                    case (GraphicType.Text):
                        {
                            var text = CreateText(def as TextDefinition);
                            if (!(text is null))
                                graphics.Add(text);
                        }
                        break;
                    case (GraphicType.Image):
                        {
                            var image = CreateImage(def as ImageDefinition);
                            if (!(image is null))
                                graphics.Add(image);
                        }
                        break;
                    case (GraphicType.Sprite):
                        {
                            var sprite = CreateSprite(def as SpriteDefinition);
                            if (!(sprite is null))
                                graphics.Add(sprite);
                        }
                        break;
                    case (GraphicType.Effect):
                        {
                            var effect = CreateEffect(def as EffectDefinition);
                            if (!(effect is null))
                                graphics.Add(effect);
                        }
                        break;
                }
            }

            return graphics;
        }

        private Text CreateText(TextDefinition definition)
        {
            var spriteFont = MainGame.Instance.AssetManager.GetSpriteFontAsset(definition.SpriteFontId);
            var text = new Text(definition.Id, definition.Name, spriteFont)
            {
                Color = definition.Color,
                Dimensions = definition.Dimensions,
                FullText = definition.FullText
            };
            return text;
        }

        private Image CreateImage(ImageDefinition definition)
        {
            var texture = MainGame.Instance.AssetManager.GetTexture2DAsset(definition.Texture2DId);
            var image = new Image(definition.Id, definition.Name, texture, definition.SourcePosition, definition.SourceDimensions)
            {
                Color = definition.Color,
                Dimensions = definition.Dimensions          
            };
            return image;
        }

        private Sprite CreateSprite(SpriteDefinition definition)
        {
            var texture = MainGame.Instance.AssetManager.GetTexture2DAsset(definition.Texture2DId);
            var sprite = new Sprite(definition.Id, definition.Name, definition.Rows, definition.Columns, texture, definition.SourcePosition, definition.SourceDimensions)
            {
                Color = definition.Color,
                Dimensions = definition.Dimensions,
                FrameTime = definition.FrameTime,
                Looping = definition.Looping
            };
            return sprite;
        }

        private Effect CreateEffect(EffectDefinition definition)
        {
            var effect = new Effect(definition.Id, definition.Name);
            return effect;
        }
    }
}
