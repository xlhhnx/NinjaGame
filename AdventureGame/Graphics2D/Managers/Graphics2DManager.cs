using NinjaGame.Common.Extensions;
using NinjaGame.Assets.Management;
using NinjaGame.Graphics2D.Assets;
using NinjaGame.Graphics2D.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NinjaGame.Common;

namespace NinjaGame.Graphics2D.Managers
{
    public class Graphics2DManager : IGraphics2DManager
    {
        public event Action<ILoadBatch<IGraphic2D>> GraphicBatchLoadedEvent = delegate { };
        public event Action<ILoadBatch<IGraphic2D>> BatchGraphicsLoadedEvent = delegate { };
        public event Action<IGraphic2D> GraphicLoadedEvent = delegate { };

        public IGraphic2DLoader Loader
        {
            get { return _graphicLoader; }
            set { _graphicLoader = value; }
        }

        protected IGraphic2DLoader _graphicLoader;
        protected Dictionary<string, ILoadBatch<IGraphic2D>> _graphicBatches;
        protected Dictionary<string, IGraphic2D> _graphicDict;

        public Graphics2DManager(IAssetManager assetManager)
        {
            _graphicLoader = new Graphic2DLoader(assetManager);
            _graphicBatches = new Dictionary<string, ILoadBatch<IGraphic2D>>();
            _graphicDict = new Dictionary<string, IGraphic2D>();
        }

        public Image GetImage(string id)
        {
            _graphicDict.TryGetValue(id, out IGraphic2D graphic);
            return graphic.Copy() as Image;
        }

        public Sprite GetSprite(string id)
        {
            _graphicDict.TryGetValue(id, out IGraphic2D graphic);
            return graphic.Copy() as Sprite;
        }

        public Text GetText(string id)
        {
            _graphicDict.TryGetValue(id, out IGraphic2D graphic);
            return graphic.Copy() as Text;
        }

        public Effect GetEffect(string id)
        {
            _graphicDict.TryGetValue(id, out IGraphic2D graphic);
            return graphic.Copy() as Effect;
        }

        public void LoadGraphic(string filePath, string id, string batchId)
        {
            _graphicBatches.TryGetValue(batchId, out ILoadBatch<IGraphic2D> batch);

            if (batch is null || _graphicDict.ContainsKey(id))
                return;

            var graphic = _graphicLoader.LoadGraphic(filePath, id);
            batch.AddValue(graphic);
            _graphicDict.Add(id, graphic);
            GraphicLoadedEvent(graphic);
        }

        public void LoadGraphicBatch(string filePath, string id)
        {
            if (filePath is null || id is null || _graphicBatches.ContainsKey(id))
                return;

            var batch = _graphicLoader.LoadGraphicBatch(filePath, id);

            if (batch is null)
                return;

            if (!_graphicBatches.ContainsKey(id))
            {
                _graphicBatches.Add(id, batch);
                GraphicBatchLoadedEvent(batch);
            }
        }

        public void LoadBatchGraphics(string id)
        {
            _graphicBatches.TryGetValue(id, out ILoadBatch<IGraphic2D> batch);

            if (batch is null)
                return;

            foreach (var graphicFile in batch.FileIdDict.Keys)
            {
                var idList = batch.FileIdDict[graphicFile];
                foreach (var i in idList)
                {
                    _graphicDict.TryGetValue(i, out IGraphic2D graphic);

                    if (graphic is null)
                        graphic = _graphicLoader.LoadGraphic(graphicFile, i);

                    batch.AddValue(graphic);

                    if (!_graphicDict.ContainsKey(i))
                    {
                        _graphicDict.Add(i, graphic);
                        GraphicLoadedEvent(graphic);
                    }
                }
                BatchGraphicsLoadedEvent(batch);
            }
        }

        public void Recycle()
        {
            _graphicDict = new Dictionary<string, IGraphic2D>();
        }

        public void UnloadAll()
        {
            List<string> ids;
            ids = _graphicBatches.Keys.ToList();

            foreach (var i in ids)
                UnloadBatch(i);
        }

        public ILoadBatch<IGraphic2D> GetBatch(string id)
        {
            _graphicBatches.TryGetValue(id, out ILoadBatch<IGraphic2D> batch);
            return batch;
        }

        public void UnloadBatch(string id)
        {
            _graphicBatches.TryGetValue(id, out ILoadBatch<IGraphic2D> batch);

            if (batch is null)
                return;

            foreach (var g in batch.Values)
                _graphicDict.Remove(g.Id);
        }

        public async void LoadGraphicAsync(string filePath, string id, string batchId)
        {
            _graphicBatches.TryGetValue(batchId, out ILoadBatch<IGraphic2D> batch);

            if (batch is null)
                return;

            var t = Task.Run(() => _graphicLoader.LoadGraphic(filePath, id));
            var graphic = await t;
            batch.AddValue(graphic);

            if (!_graphicDict.ContainsKey(id))
            {
                _graphicDict.Add(id, graphic);
                GraphicLoadedEvent(graphic);
            }
        }

        public async void LoadGraphicBatchAsync(string filePath, string id)
        {
            if (filePath is null || id is null || _graphicBatches.ContainsKey(id))
                return;

            var t = Task.Run(() => _graphicLoader.LoadGraphicBatch(filePath, id));
            var batch = await t;

            if (batch is null)
                return;

            if (!_graphicBatches.ContainsKey(id))
            {
                _graphicBatches.Add(id, batch);
                GraphicBatchLoadedEvent(batch);
            }
        }

        public async void LoadBatchGraphicsAsync(string id)
        {
            _graphicBatches.TryGetValue(id, out ILoadBatch<IGraphic2D> batch);

            if (batch is null)
                return;

            var tasks = new List<Task<IGraphic2D>>();
            foreach (var graphicFile in batch.FileIdDict.Keys)
            {
                var idList = batch.FileIdDict[graphicFile];
                foreach (var i in idList)
                {
                    _graphicDict.TryGetValue(i, out IGraphic2D graphic);

                    if (graphic is null)
                    {
                        var t = Task.Run(() => _graphicLoader.LoadGraphic(graphicFile, i));
                        tasks.Add(t);
                    }
                }
            }

            foreach (var t in tasks.InCompletionOrder())
            {
                var graphic = await t;
                batch.AddValue(graphic);

                if (!_graphicDict.ContainsKey(graphic.Id))
                {
                    _graphicDict.Add(graphic.Id, graphic);
                    GraphicLoadedEvent(graphic);
                }
            }
            BatchGraphicsLoadedEvent(batch);
        }

        public bool ContainsBatch(string id)
        {
            return _graphicBatches.ContainsKey(id);
        }

        public bool ContainsBatch(ILoadBatch<IGraphic2D> batch)
        {
            return _graphicBatches.ContainsKey(batch.Id);
        }

        public bool GraphicLoaded(string id)
        {
            return _graphicDict.ContainsKey(id);
        }

        public bool GraphicLoaded(IGraphic2D graphic)
        {
            return _graphicDict.ContainsKey(graphic.Id);
        }
    }
}