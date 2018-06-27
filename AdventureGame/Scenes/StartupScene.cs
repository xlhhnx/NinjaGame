using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NinjaGame.Config;
using NinjaGame.Graphics2D.Assets;
using NinjaGame.Graphics2D.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NinjaGame.Scenes
{
    public class StartupScene : IScene
    {
        protected float Status { get { return _runningTasks.Count / (_runningTasks.Count + _completedTasks.Count); } }

        protected int _batchTaskId;
        protected int _currentId;
        protected Text _splashText;
        protected SpriteBatch _spriteBatch;
        protected List<Task> _completedTasks;
        protected Dictionary<int, Task> _runningTasks;

        public StartupScene()
        {
            _currentId = 0;
            _runningTasks = new Dictionary<int, Task>();
            _completedTasks = new List<Task>();
            _spriteBatch = new SpriteBatch(MainGame.Instance.GraphicsDevice);

            var spriteFont = MainGame.Instance.AssetManager.GetSpriteFontAsset("000000000003");
            _splashText = new Text(spriteFont, Color.White, Color.Gray, new Vector2(), new Vector2(200,200), "SPLASH!");
            _splashText.Center();

            var task = Task.Factory.StartNew(LoadBatch);
            _batchTaskId = _currentId;
            _runningTasks.Add(_currentId, task);
            _currentId++;
        }

        public void Draw()
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_splashText, MainGame.Instance.GetScreenCenter());
            _spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            Dictionary<int, Task> taskDict = null;
            lock (_runningTasks)
                taskDict = new Dictionary<int, Task>(_runningTasks);

            List<int> ids = new List<int>();
            foreach (var id in taskDict.Keys)
                ids.Add(id);

            for (int i = 0; i < ids.Count; i++)
            {
                var task = taskDict[ids[i]];
                if (task.IsCompleted)
                {
                    _runningTasks.Remove(ids[i]);
                    _completedTasks.Add(task);

                    if (task.Id == _batchTaskId)
                    {
                        var batch = MainGame.Instance.AssetManager.GetAssetBatch(GlobalConfig.StartupAssetBatchId);
                        foreach (var p in batch.GetAllFileIdPairs())
                        {
                            var t = Task.Factory.StartNew(() => LoadAsset(p.Item1, p.Item2, batch.Id));
                            _runningTasks.Add(_currentId, t);
                            _currentId++;
                        }
                    }
                }
            }
        }

        private void LoadBatch()
        {
            MainGame.Instance.AssetManager.LoadAssetBatch(GlobalConfig.StartupAssetDefinitionFile, GlobalConfig.StartupAssetBatchId);
        }

        private void LoadAsset(string fileName, string id, string batchId)
        {
            MainGame.Instance.AssetManager.LoadAsset(fileName, id, batchId);
        }
    }
}
