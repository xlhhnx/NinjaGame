using NinjaGame.Common.Extensions;
using NinjaGame.Assets.Management;
using NinjaGame.Graphics2D.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NinjaGame.Batches.Loading;
using NinjaGame.Loading;
using NinjaGame.Graphics2D.Loading;

namespace NinjaGame.Graphics2D.Managers
{
    public class Graphics2DManager : IGraphics2DManager
    {
        public event Action<ILoadBatch<IGraphic2D>> GraphicBatchLoadedEvent = delegate { };
        public event Action<ILoadBatch<IGraphic2D>> BatchGraphicsLoadedEvent = delegate { };
        public event Action<IGraphic2D> GraphicLoadedEvent = delegate { };

        public Graphic2DLoader Loader { get; set; }
        
        protected Dictionary<string, ILoadBatch<IGraphic2D>> _graphicBatches;
        protected Dictionary<string, IGraphic2D> _graphicDict;

        public Graphics2DManager(IAssetManager assetManager)
        {
            Loader = new Graphic2DLoader();
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

        public void LoadBatches(string filePath)
        {
            var batches = Loader.LoadBatches(filePath);
            
            if (batches is null)
                return;

            foreach (var b in batches)
            {
                if (!_graphicBatches.ContainsKey(b.Id))
                {
                    _graphicBatches.Add(b.Id, b);
                    GraphicBatchLoadedEvent(b);
                }
            }
        }

        public void LoadBatchGrahpicsById(string id)
        {
            if (_graphicBatches.TryGetValue(id, out var batch))
            {
                LoadBatchGraphics(batch);
            }
        }

        public async void LoadBatchesAsync(string filePath)
        {
            var task = Task.Run(() => Loader.LoadBatches(filePath));
            var batches = await task;
            
            if (batches is null)
                return;

            foreach (var b in batches)
            {
                if (!_graphicBatches.ContainsKey(b.Id))
                {
                    _graphicBatches.Add(b.Id, b);
                    GraphicBatchLoadedEvent(b);
                }
            }
        }

        public void LoadBatchGraphicsByIdAsync(string id)
        {
            if (_graphicBatches.TryGetValue(id, out var batch))
            {
                LoadBatchGraphicsAsync(batch);
            }
        }

        public void LoadBatchGraphics(ILoadBatch<IGraphic2D> batch)
        {
            var graphics = Loader.LoadGraphics(batch);
            foreach (var g in graphics)
            {
                if (!batch.Values.Contains(g))
                    batch.Values.Add(g);

                if (!_graphicDict.ContainsKey(g.Id))
                {
                    _graphicDict.Add(g.Id, g);
                    GraphicLoadedEvent(g);
                }
            }
            BatchGraphicsLoadedEvent(batch);
        }

        public void LoadBatchGrahpicsByName(string name)
        {
            foreach (var id in _graphicBatches.Keys)
            {
                if (_graphicBatches[id].Name == name)
                {
                    LoadBatchGraphics(_graphicBatches[id]);
                    break;
                }
            }
        }

        public async void LoadBatchGraphicsAsync(ILoadBatch<IGraphic2D> batch)
        {
            var task = Task.Run(() => Loader.LoadGraphics(batch));
            var graphics = await task;
            foreach (var g in graphics)
            {
                if (!batch.Values.Contains(g))
                    batch.Values.Add(g);

                if (!_graphicDict.ContainsKey(g.Id))
                {
                    _graphicDict.Add(g.Id, g);
                    GraphicLoadedEvent(g);
                }
            }
            BatchGraphicsLoadedEvent(batch);
        }

        public void LoadBatchGraphicsByNameAsync(string name)
        {
            foreach (var id in _graphicBatches.Keys)
            {
                if (_graphicBatches[id].Name == name)
                {
                    LoadBatchGraphicsAsync(_graphicBatches[id]);
                    break;
                }
            }
        }
    }
}