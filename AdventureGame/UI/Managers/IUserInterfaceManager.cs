using NinjaGame.Common.Loading;
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
        void LoadControlByName(string filePath, string name, string batchId);
        void LoadControlBatch(string filePath, string id);
        void LoadControlBatchByName(string filePath, string name);
        void LoadBatchControls(string id);
        void LoadBatchControlsByName(string name);
        void LoadControlAsync(string filePath, string id, string batchId);
        void LoadControlByNameAsync(string filePath, string name, string batchId);
        void LoadControlBatchAsync(string filePath, string id);
        void LoadControlBatchByNameAsync(string filePath, string name);
        void LoadBatchControlsAsync(string id);
        void LoadBatchControlsByNameAsync(string name);
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
