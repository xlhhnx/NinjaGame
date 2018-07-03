using NinjaGame.Common;
using NinjaGame.UI.Controls;
using NinjaGame.UI.Loading;
using System;

namespace NinjaGame.UI.Managers
{
    public interface IUserInterfaceManager
    {
        event Action<ILoadBatch<IControl>> ControlBatchLoadedEvent;
        event Action<ILoadBatch<IControl>> BatchControlsLoadedEvent;
        event Action<IControl> ControlLoadedEvent;

        IControlLoader Loader { get; set; }

        void LoadControl(string filePath, string id, string batchId);
        void LoadControlBatch(string filePath, string id);
        void LoadBatchControls(string id);
        void LoadControlAsync(string filePath, string id, string batchId);
        void LoadControlBatchAsync(string filePath, string id);
        void LoadBatchControlsAsync(string id);
        void UnloadBatch(string id);
        void UnloadAll();
        void Recycle();
        bool ContainsBatch(string id);
        bool ContainsBatch(ILoadBatch<IControl> batch);
        bool ControlLoaded(string id);
        bool ControlLoaded(IControl control);
        ILoadBatch<IControl> GetBatch(string id);
        Button GetButton(string id);
    }
}
