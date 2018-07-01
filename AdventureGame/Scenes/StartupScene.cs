using System.Collections.Generic;
using NinjaGame.Config;
using NinjaGame.Graphics2D.Assets;
using NinjaGame.Graphics2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NinjaGame.Menus;
using NinjaGame.Tasks;
using NinjaGame.Assets.Batches;
using NinjaGame.Graphics2D.Batches;

namespace NinjaGame.Scenes
{
    public class StartupScene : IScene
    {        
        protected bool _assetsLoaded;
        protected Text _splashText;
        protected SpriteBatch _spriteBatch;

        public StartupScene()
        {
            _spriteBatch = new SpriteBatch(MainGame.Instance.GraphicsDevice);

            var spriteFont = MainGame.Instance.AssetManager.GetSpriteFontAsset("000000000003");
            _splashText = MainGame.Instance.GraphicsManager.GetText("000000000003");
            _splashText.Center();

            MainGame.Instance.AssetManager.AssetBatchLoadedEvent += HandleAssetBatchLoaded;
            MainGame.Instance.AssetManager.BatchAssetsLoadedEvent += HandleBatchAssetsLoaded;
            MainGame.Instance.GraphicsManager.GraphicBatchLoadedEvent += HandleGraphicBatchLoaded;
            MainGame.Instance.GraphicsManager.BatchGraphicsLoadedEvent += HandleBatchGraphicsLoaded;

            MainGame.Instance.AssetManager.LoadAssetBatchAsync(GlobalConfig.StartupAssetDefinitionFile, GlobalConfig.StartupAssetBatchId);
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
            _spriteBatch.DrawString(_splashText, MainGame.Instance.GetScreenCenter());
            _spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            // No op
        }

        public void HandleAssetBatchLoaded(IAssetBatch batch) 
        {
            if (batch.Id == GlobalConfig.StartupAssetBatchId)
                MainGame.Instance.AssetManager.LoadBatchAssetsAsync(batch.Id);
        }

        public void HandleBatchAssetsLoaded(IAssetBatch batch)
        {
            if (batch.Id == GlobalConfig.StartupAssetBatchId)
                MainGame.Instance.GraphicsManager.LoadGraphicBatchAsync(GlobalConfig.StartupGraphicDefinitionFile, GlobalConfig.StartupGraphicBatchId);
        }

        public void HandleGraphicBatchLoaded(IGraphic2DBatch batch)
        {
            if (batch.Id == GlobalConfig.StartupGraphicBatchId)
                MainGame.Instance.GraphicsManager.LoadBatchGraphicsAsync(batch.Id);
        }

        public void HandleBatchGraphicsLoaded(IGraphic2DBatch batch)
        {
            if (batch.Id == GlobalConfig.StartupGraphicBatchId)
            {
                var scene = new MainMenu();
                MainGame.Instance.PopScene();
                MainGame.Instance.PushScene(scene);
            }
        }
    }
}
