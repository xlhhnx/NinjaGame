using NinjaGame.Config;
using NinjaGame.Graphics2D.Assets;
using NinjaGame.Graphics2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NinjaGame.Menus;
using NinjaGame.Assets.Batches;
using System;
using NinjaGame.UI.Controls;
using NinjaGame.Batches.Loading;

namespace NinjaGame.Scenes
{
    public class StartupScene : IScene
    {
        protected bool _fullyLoaded;
        protected Text _splashText;
        protected SpriteBatch _spriteBatch;
        protected TimeSpan _splashTime;
        protected TimeSpan _elapsedTime;

        public StartupScene(TimeSpan splashTime)
        {
            _splashTime = splashTime;

            _spriteBatch = new SpriteBatch(MainGame.Instance.GraphicsDevice);
            _elapsedTime = new TimeSpan();

            var spriteFont = MainGame.Instance.AssetManager.GetSpriteFontAsset("000000000003");
            _splashText = MainGame.Instance.GraphicsManager.GetText("000000000003");
            CalcTime();
            _splashText.Center(MainGame.Instance.GetScreenSize());

            MainGame.Instance.AssetManager.AssetBatchLoadedEvent += HandleAssetBatchLoaded;
            MainGame.Instance.AssetManager.BatchAssetsLoadedEvent += HandleBatchAssetsLoaded;
            MainGame.Instance.GraphicsManager.GraphicBatchLoadedEvent += HandleGraphicBatchLoaded;
            MainGame.Instance.GraphicsManager.BatchGraphicsLoadedEvent += HandleBatchGraphicsLoaded;

            MainGame.Instance.AssetManager.LoadAssetBatchesAsync(GlobalConfig.AssetBatchFile);
        }

        public void Dispose()
        {
            MainGame.Instance.AssetManager.AssetBatchLoadedEvent -= HandleAssetBatchLoaded;
            MainGame.Instance.AssetManager.BatchAssetsLoadedEvent -= HandleBatchAssetsLoaded;
            MainGame.Instance.GraphicsManager.GraphicBatchLoadedEvent -= HandleGraphicBatchLoaded;
            MainGame.Instance.GraphicsManager.BatchGraphicsLoadedEvent -= HandleBatchGraphicsLoaded;
        }

        public void Draw()
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_splashText, Vector2.Zero);
            _spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;
            if (_elapsedTime > _splashTime && _fullyLoaded)
            {
                var scene = new MainMenu();
                MainGame.Instance.PopScene();
                MainGame.Instance.PushScene(scene);
            }
            CalcTime();
        }

        public void HandleAssetBatchLoaded(IAssetBatch batch) 
        {
            if (batch.Name == "Startup")
                MainGame.Instance.AssetManager.LoadBatchAssetsAsyncById(batch.Id);
        }

        public void HandleBatchAssetsLoaded(IAssetBatch batch)
        {
            if (batch.Name == "Startup")
                MainGame.Instance.GraphicsManager.LoadBatchesAsync(GlobalConfig.GraphicBatchFile);
        }

        public void HandleGraphicBatchLoaded(ILoadBatch<IGraphic2D> batch)
        {
            if (batch.Name == "Startup")
                MainGame.Instance.GraphicsManager.LoadBatchGraphicsByIdAsync(batch.Id);
        }

        public void HandleBatchGraphicsLoaded(ILoadBatch<IGraphic2D> batch)
        {
            _fullyLoaded = true;
        }

        private void CalcTime()
        {
            var timeRemaining = _splashTime - _elapsedTime;
            var seconds = timeRemaining.Seconds.ToString();
            var milliseconds = timeRemaining.Milliseconds.ToString("D3").Substring(0, 1);
            _splashText.FullText = $"Splash! {seconds}.{milliseconds} seconds remaining...";
        }
    }
}
