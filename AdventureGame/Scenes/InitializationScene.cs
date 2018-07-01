using System.Threading.Tasks;
using NinjaGame.Config;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using NinjaGame.Tasks;
using NinjaGame.Assets.Batches;
using NinjaGame.Graphics2D.Assets;
using NinjaGame.Graphics2D.Batches;
using System;
using System.Windows.Forms;
using NinjaGame.Common;

namespace NinjaGame.Scenes
{
    public class InitializationScene : IScene
    {
        protected TimeSpan _timeout;
        protected TimeSpan _elapsedTime;

        public InitializationScene(TimeSpan timeout)
            : this()
        {
            _timeout = timeout;
        }

        public InitializationScene()
        {
            _timeout = new TimeSpan(0, 0, 30);
            _elapsedTime = new TimeSpan();

            MainGame.Instance.SetScreenMode(ScreenMode.Borderless);
            MainGame.Instance.SetResolution();
            Console.WriteLine($"gamePos={MainGame.Instance.GraphicsDevice.Viewport.X} {MainGame.Instance.GraphicsDevice.Viewport.Y}");

            MainGame.Instance.AssetManager.AssetBatchLoadedEvent += HandleAssetBatchLoaded;
            MainGame.Instance.AssetManager.BatchAssetsLoadedEvent += HandleBatchAssetsLoaded;
            MainGame.Instance.GraphicsManager.GraphicBatchLoadedEvent += HandleGraphicsBatchLoaded;
            MainGame.Instance.GraphicsManager.BatchGraphicsLoadedEvent += HandleBatchGraphicsLoaded;

            MainGame.Instance.AssetManager.LoadAssetBatchAsync(GlobalConfig.InitialAssetDefinitionFile, GlobalConfig.InitialAssetBatchId);
        }

        public void Draw()
        {
            // No op
        }

        public void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;
            if (_elapsedTime > _timeout)
            {
                MessageBox.Show("The application has failed to load. It will now close.", "Fatal Error", MessageBoxButtons.OK);
                MainGame.Instance.Exit();
            }
        }

        public void HandleAssetBatchLoaded(IAssetBatch batch)
        {
            if (batch.Id == GlobalConfig.InitialAssetBatchId)
                MainGame.Instance.AssetManager.LoadBatchAssetsAsync(batch.Id);
        }

        public void HandleBatchAssetsLoaded(IAssetBatch batch)
        {
            if (batch.Id == GlobalConfig.InitialAssetBatchId)
                MainGame.Instance.GraphicsManager.LoadGraphicBatchAsync(GlobalConfig.InitialGraphicDefinitionFile, GlobalConfig.InitialGraphicBatchId);
        }

        public void HandleGraphicsBatchLoaded(IGraphic2DBatch batch)
        {
            if (batch.Id == GlobalConfig.InitialGraphicBatchId)
                MainGame.Instance.GraphicsManager.LoadBatchGraphicsAsync(batch.Id);
        }

        public void HandleBatchGraphicsLoaded(IGraphic2DBatch batch)
        {
            var startupScene = new StartupScene();
            MainGame.Instance.PopScene();
            MainGame.Instance.PushScene(startupScene);
        }

        public void Dispose()
        {
            MainGame.Instance.AssetManager.AssetBatchLoadedEvent -= HandleAssetBatchLoaded;
            MainGame.Instance.AssetManager.BatchAssetsLoadedEvent -= HandleBatchAssetsLoaded;
            MainGame.Instance.GraphicsManager.GraphicBatchLoadedEvent -= HandleGraphicsBatchLoaded;
            MainGame.Instance.GraphicsManager.BatchGraphicsLoadedEvent -= HandleBatchGraphicsLoaded;
        }
    }
}
