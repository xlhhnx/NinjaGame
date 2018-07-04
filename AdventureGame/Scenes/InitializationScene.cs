using NinjaGame.Config;
using Microsoft.Xna.Framework;
using NinjaGame.Assets.Batches;
using NinjaGame.Graphics2D.Assets;
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

            //MainGame.Instance.SetScreenMode(ScreenMode.FullScreen);
            MainGame.Instance.SetScreenMode(ScreenMode.Borderless);
            MainGame.Instance.SetResolution();
            
            var form = (Form)Control.FromHandle(MainGame.Instance.Window.Handle);
            form.Location = new System.Drawing.Point(-MainGame.Instance.GraphicsDevice.Viewport.Width, 0);
            //form.Location = new System.Drawing.Point(0, 0);

            Console.WriteLine($"gamePos={MainGame.Instance.GraphicsDevice.Viewport.X} {MainGame.Instance.GraphicsDevice.Viewport.Y}");

            MainGame.Instance.AssetManager.AssetBatchLoadedEvent += HandleAssetBatchLoaded;
            MainGame.Instance.AssetManager.BatchAssetsLoadedEvent += HandleBatchAssetsLoaded;
            MainGame.Instance.GraphicsManager.GraphicBatchLoadedEvent += HandleGraphicsBatchLoaded;
            MainGame.Instance.GraphicsManager.BatchGraphicsLoadedEvent += HandleBatchGraphicsLoaded;

            MainGame.Instance.AssetManager.LoadAssetBatchByNameAsync(GlobalConfig.AssetDefinitionFile, "initial");
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
            if (batch.Name == "initial")
                MainGame.Instance.AssetManager.LoadBatchAssetsAsync(batch.Id);
        }

        public void HandleBatchAssetsLoaded(IAssetBatch batch)
        {
            if (batch.Name == "initial")
                MainGame.Instance.GraphicsManager.LoadGraphicBatchByNameAsync(GlobalConfig.GrahpicDefinitionFile, "initial");
        }

        public void HandleGraphicsBatchLoaded(ILoadBatch<IGraphic2D> batch)
        {
            if (batch.Name == "initial")
                MainGame.Instance.GraphicsManager.LoadBatchGraphicsAsync(batch.Id);
        }

        public void HandleBatchGraphicsLoaded(ILoadBatch<IGraphic2D> batch)
        {
            var startupScene = new StartupScene(new TimeSpan(0,0,5));
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
