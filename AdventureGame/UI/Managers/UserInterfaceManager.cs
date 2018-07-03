using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NinjaGame.Common;
using NinjaGame.Common.Extensions;
using NinjaGame.Graphics2D.Managers;
using NinjaGame.UI.Controls;
using NinjaGame.UI.Loading;

namespace NinjaGame.UI.Managers
{
    public class UserInterfaceManager : IUserInterfaceManager
    {
        public event Action<ILoadBatch<IControl>> ControlBatchLoadedEvent;
        public event Action<ILoadBatch<IControl>> BatchControlsLoadedEvent;
        public event Action<IControl> ControlLoadedEvent;
        
        public IControlLoader Loader { get; set; }

        protected Dictionary<string, IControl> _controlDict;
        protected Dictionary<string, ILoadBatch<IControl>> _controlBatches;

        public UserInterfaceManager(IGraphics2DManager _graphicsManager)
        {
            Loader = new ControlLoader(_graphicsManager);

            _controlBatches = new Dictionary<string, ILoadBatch<IControl>>();
            _controlDict = new Dictionary<string, IControl>();
        }

        public bool ContainsBatch(string id)
        {
            return _controlBatches.ContainsKey(id);
        }

        public bool ContainsBatch(ILoadBatch<IControl> batch)
        {
            return _controlBatches.ContainsKey(batch.Id);
        }

        public bool ControlLoaded(string id)
        {
            return _controlDict.ContainsKey(id);
        }

        public bool ControlLoaded(IControl control)
        {
            return _controlDict.ContainsKey(control.Id);
        }

        public ILoadBatch<IControl> GetBatch(string id)
        {
            _controlBatches.TryGetValue(id, out var batch);
            return batch as LoadBatch<IControl>;
        }

        public void LoadBatchControls(string id)
        {
            _controlBatches.TryGetValue(id, out var batch);

            if (batch is null)
                return;

            foreach (var controlFile in batch.FileIdDict.Keys)
            {
                var idList = batch.FileIdDict[controlFile];
                foreach (var i in idList)
                {
                    _controlDict.TryGetValue(i, out var control);

                    if (control is null)
                        control = Loader.LoadControl(controlFile, i);

                    batch.AddValue(control);

                    if (!_controlDict.ContainsKey(i))
                    {
                        _controlDict.Add(i, control);
                        ControlLoadedEvent(control);
                    }
                }
                BatchControlsLoadedEvent(batch);
            }
        }

        public async void LoadBatchControlsAsync(string id)
        {
            _controlBatches.TryGetValue(id, out var batch);

            if (batch is null)
                return;

            var tasks = new List<Task<IControl>>();
            foreach (var controlFile in batch.FileIdDict.Keys)
            {
                var idList = batch.FileIdDict[controlFile];
                foreach (var i in idList)
                {
                    _controlDict.TryGetValue(i, out var control);

                    if (control is null)
                    {
                        var t = Task.Run(() => Loader.LoadControl(controlFile, i));
                        tasks.Add(t);
                    }
                }
            }

            foreach (var t in tasks.InCompletionOrder())
            {
                var control = await t;
                batch.AddValue(control);

                if (!_controlDict.ContainsKey(control.Id))
                {
                    _controlDict.Add(control.Id, control);
                    ControlLoadedEvent(control);
                }
            }
            BatchControlsLoadedEvent(batch);
        }

        public void LoadControl(string filePath, string id, string batchId)
        {
            _controlBatches.TryGetValue(batchId, out var batch);

            if (batch is null || _controlDict.ContainsKey(id))
                return;

            var control = Loader.LoadControl(filePath, id);
            batch.AddValue(control);

            _controlDict.Add(id, control);
            ControlLoadedEvent(control);
        }

        public async void LoadControlAsync(string filePath, string id, string batchId)
        {
            _controlBatches.TryGetValue(batchId, out var batch);

            if (batch is null || _controlDict.ContainsKey(id))
                return;

            var task = Task.Run(() => Loader.LoadControl(filePath, id));
            var control = await task;
            batch.AddValue(control);

            _controlDict.Add(id, control);
            ControlLoadedEvent(control);
        }

        public void LoadControlBatch(string filePath, string id)
        {
            if (filePath is null || id is null || _controlBatches.ContainsKey(id))
                return;

            var batch = Loader.LoadControlBatch(filePath, id);

            if (batch is null)
                return;

            _controlBatches.Add(id, batch);
            ControlBatchLoadedEvent(batch);
        }

        public async void LoadControlBatchAsync(string filePath, string id)
        {
            if (filePath is null || id is null || _controlBatches.ContainsKey(id))
                return;

            var task = Task.Run(() => Loader.LoadControlBatch(filePath, id));
            var batch = await task;

            if (batch is null)
                return;

            _controlBatches.Add(id, batch);
            ControlBatchLoadedEvent(batch);
        }

        public void Recycle()
        {
            _controlDict = new Dictionary<string, IControl>();
        }

        public void UnloadAll()
        {
            List<string> ids;
            ids = _controlBatches.Keys.ToList();

            foreach (var i in ids)
                UnloadBatch(i);
        }

        public void UnloadBatch(string id)
        {
            _controlBatches.TryGetValue(id, out var batch);

            if (batch is null)
                return;

            foreach (var c in batch.Values)
                _controlDict.Remove(c.Id);
        }

        public Button GetButton(string id)
        {
            _controlDict.TryGetValue(id, out var button);
            return button as Button;
        }
    }
}
