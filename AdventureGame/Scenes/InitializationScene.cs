using System.Threading.Tasks;
using NinjaGame.Config;
using Microsoft.Xna.Framework;

namespace NinjaGame.Scenes
{
    public class InitializationScene : IScene
    {
        protected float _status;
        protected Task _task;
        protected StartupScene _startupScene;

        public InitializationScene()
        {
            MainGame.Instance.SetFullScreen(true);
            MainGame.Instance.SetResolution();

            _task = new Task(Load);
            _task.Start();
        }

        public void Draw()
        {
            // No op
        }

        public void Update(GameTime gameTime)
        {
            if (_task.IsCompleted)
            {
                MainGame.Instance.PopScene();
                MainGame.Instance.PushScene(_startupScene);
            }
        }

        private void Load() 
        {
            MainGame.Instance.AssetManager.LoadAssetBatch(GlobalConfig.InitialAssetDefinitionFile, GlobalConfig.InitialAssetBatchId);
            MainGame.Instance.AssetManager.LoadBatchAssets(GlobalConfig.InitialAssetBatchId);

            _startupScene = new StartupScene();
        }
    }
}
