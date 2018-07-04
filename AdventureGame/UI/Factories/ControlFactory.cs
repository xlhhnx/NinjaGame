using System;
using Microsoft.Xna.Framework;
using NinjaGame.Graphics2D.Factories;
using NinjaGame.UI.Controls;

namespace NinjaGame.UI.Factories
{
    public static class ControlFactory
    {
        public static Button CreateButton(string blurredImageId, string focusedImageId, string clickedImageId, string buttonTextId, string strButtonText,
            Color blurredColor, Color focusedColor, Color clickedColor, Color textColor, Action action)
        {
            var blurredImage = GraphicFactory.CreateImage(blurredImageId, blurredColor);
            var focusedImage = GraphicFactory.CreateImage(focusedImageId, focusedColor);
            var clickedImage = GraphicFactory.CreateImage(clickedImageId, clickedColor);
            var buttonText = GraphicFactory.CreateText(buttonTextId, textColor, strButtonText);

            var button = new Button(blurredImage, focusedImage, clickedImage, buttonText)
            {
                Action = action
            };            
            return button;
        }

        public static Button CreateDebugButton(string strButtonText, Action action)
        {
            var button = CreateButton("000000000001", "000000000001", "000000000001", "000000000001", strButtonText, Color.White, Color.Yellow, Color.Red, Color.Black, action);
            return button;
        }
    }
}
