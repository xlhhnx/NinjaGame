using NinjaGame.Common;
using NinjaGame.Common.Loading;
using NinjaGame.UI.Controls;

namespace NinjaGame.UI.Loading
{
    public interface IControlLoader
    {
        IControl LoadControl(string filePath, string id);
        IControl LoadControlByName(string filePath, string name);
        IControl LoadControl(IDefinition<ControlType> stagedControl);
        ILoadBatch<IControl> LoadControlBatch(string filePath, string id);
        ILoadBatch<IControl> LoadControlBatchByName(string filePath, string name);
        IDefinition<ControlType> StageControl(string filePath, string id);
        IDefinition<ControlType> StageControlByName(string filePath, string name);
    }
}
