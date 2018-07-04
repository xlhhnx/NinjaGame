using Microsoft.Xna.Framework;
using NinjaGame.Graphics2D.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaGame.Graphics2D.Factories
{
    public static class GraphicFactory
    {
        public static Image CreateImage(string id, Color color, Vector2 positionOffset, Vector2 dimensions)
        {
            var image = MainGame.Instance.GraphicsManager.GetImage(id);

            if (image is null)
                return null;

            image.Color = color;
            image.PositionOffset = positionOffset;
            image.Dimensions = dimensions;

            return image;
        }
        
        public static Image CreateImage(string id, Color color, Vector2 dimensions)
        {
            var image = MainGame.Instance.GraphicsManager.GetImage(id);

            if (image is null)
                return null;

            image.Color = color;
            image.Dimensions = dimensions;

            return image;
        }

        public static Image CreateImage(string id, Color color)
        {
            var image = MainGame.Instance.GraphicsManager.GetImage(id);

            if (image is null)
                return null;

            image.Color = color;

            return image;
        }

        public static Text CreateText(string id, Color color, Vector2 positionOffset, Vector2 dimensions, string fullText = "")
        {
            var text = MainGame.Instance.GraphicsManager.GetText(id);

            if (text is null)
                return null;

            text.Color = color;
            text.PositionOffset = positionOffset;
            text.Dimensions = dimensions;
            text.FullText = fullText;

            return text;
        }
        
        public static Text CreateText(string id, Color color, Vector2 dimensions, string fullText = "")
        {
            var text = MainGame.Instance.GraphicsManager.GetText(id);

            if (text is null)
                return null;

            text.Color = color;
            text.Dimensions = dimensions;

            return text;
        }
        
        public static Text CreateText(string id, Color color, string fullText = "")
        {
            var text = MainGame.Instance.GraphicsManager.GetText(id);

            if (text is null)
                return null;

            text.Color = color;

            return text;
        }
    }
}
