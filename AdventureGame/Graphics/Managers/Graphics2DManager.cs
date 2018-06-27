using NinjaGame.Graphics2D.Assets;
using NinjaGame.Graphics2D.Loading;
using System.Collections.Generic;
using System.Linq;

namespace NinjaGame.Graphics2D.Managers
{
    public class Graphics2DManager : IGraphics2DManager
    {
        public IGraphic2DLoader Loader
        {
            get { return _loader; }
            set { _loader = value; }
        }


        protected IGraphic2DLoader _loader;
        protected Dictionary<string, Image> _images;
        protected Dictionary<string, Sprite> _sprites;
        protected Dictionary<string, Text> _texts;
        protected Dictionary<string, Effect> _effects;


        public Graphics2DManager(IGraphic2DLoader loader)
        {
            _loader = loader;
            _images = new Dictionary<string, Image>();
            _sprites = new Dictionary<string, Sprite>();
            _texts = new Dictionary<string, Text>();
            _effects = new Dictionary<string, Effect>();
        }

        public void LoadGraphic(string filePath, string id, GraphicType graphicType)
        {
            switch (graphicType)
            {
                case (GraphicType.Text):
                    {
                        var text = _loader.LoadText(filePath, id);
                        AddGraphic(text, true);
                    }
                    break;
                case (GraphicType.Image):
                    {
                        var image = _loader.LoadImage(filePath, id);
                        AddGraphic(image, true);
                    }
                    break;
                case (GraphicType.Sprite):
                    {
                        var sprite = _loader.LoadSprite(filePath, id);
                        AddGraphic(sprite, true);
                    }
                    break;
                case (GraphicType.Effect):
                    {
                        var effect = _loader.LoadEffect(filePath, id);
                        AddGraphic(effect, true);
                    }
                    break;
            }
        }

        public void LoadGraphic(string filePath, string id)
        {
            var graphic = _loader.LoadGraphic(filePath, id);

            if (graphic is Text) AddGraphic(graphic as Text, true);
            else if (graphic is Image) AddGraphic(graphic as Image, true);
            else if (graphic is Sprite) AddGraphic(graphic as Sprite, true);
            else if (graphic is Effect) AddGraphic(graphic as Effect, true);
        }

        public void LoadGraphics(string filePath, GraphicType graphicType)
        {
            var graphics = _loader.LoadGraphics(filePath);

            switch (graphicType)
            {
                case (GraphicType.Text):
                    {
                        var texts = graphics.Where(g => g is Text);
                        foreach (var t in texts) AddGraphic(t, true);
                    }
                    break;
                case (GraphicType.Image):
                    {
                        var images = graphics.Where(g => g is Image);
                        foreach (var i in images) AddGraphic(i, true);
                    }
                    break;
                case (GraphicType.Sprite):
                    {
                        var sprites = graphics.Where(g => g is Sprite);
                        foreach (var s in sprites) AddGraphic(s, true);
                    }
                    break;
                case (GraphicType.Effect):
                    {
                        var effect = graphics.Where(g => g is Effect);
                        foreach (var e in effect) AddGraphic(e, true);
                    }
                    break;
            }
        }

        public void LoadGraphics(string filePath)
        {
            var graphics = _loader.LoadGraphics(filePath);

            foreach (var g in graphics) AddGraphic(g, true);
        }

        public void AddGraphic(IGraphic2D graphic, bool overwrite = false)
        {
            if (graphic is Text) AddGraphic(graphic as Text, overwrite);
            else if (graphic is Image) AddGraphic(graphic as Image, overwrite);
            else if (graphic is Sprite) AddGraphic(graphic as Sprite, overwrite);
            else if (graphic is Effect) AddGraphic(graphic as Effect, overwrite);
        }

        public void AddGraphic(Image image, bool overwrite = false)
        {
            if (!_images.ContainsKey(image.Id)) _images.Add(image.Id, image);
            else if (overwrite) _images[image.Id] = image;
        }

        public void AddGraphic(Sprite sprite, bool overwrite = false)
        {
            if (!_sprites.ContainsKey(sprite.Id)) _sprites.Add(sprite.Id, sprite);
            else if (overwrite) _sprites[sprite.Id] = sprite;
        }

        public void AddGraphic(Text text, bool overwrite = false)
        {
            if (!_texts.ContainsKey(text.Id)) _texts.Add(text.Id, text);
            else if (overwrite) _texts[text.Id] = text;
        }

        public void AddGraphic(Effect effect, bool overwrite = false)
        {
            if (!_effects.ContainsKey(effect.Id)) _effects.Add(effect.Id, effect);
            else if (overwrite) _effects[effect.Id] = effect;
        }

        public void Recycle(GraphicType graphicType)
        {
            switch (graphicType)
            {
                case (GraphicType.Text): _texts = new Dictionary<string, Text>(); break;
                case (GraphicType.Image): _images = new Dictionary<string, Image>(); break;
                case (GraphicType.Sprite): _sprites = new Dictionary<string, Sprite>(); break;
                case (GraphicType.Effect): _effects = new Dictionary<string, Effect>(); break;
            }
        }

        public void Recycle()
        {
            _texts = new Dictionary<string, Text>();
            _images = new Dictionary<string, Image>();
            _sprites = new Dictionary<string, Sprite>();
            _effects = new Dictionary<string, Effect>();
        }

        public Image GetImage(string id)
        {
            Image image = null;
            _images.TryGetValue(id, out image);
            return image;
        }

        public Sprite GetSprite(string id)
        {
            Sprite sprite = null;
            _sprites.TryGetValue(id, out sprite);
            return sprite;
        }

        public Text GetText(string id)
        {
            Text text = null;
            _texts.TryGetValue(id, out text);
            return text;
        }

        public Effect GetEffect(string id)
        {
            Effect effect = null;
            _effects.TryGetValue(id, out effect);
            return effect;
        }
    }
}