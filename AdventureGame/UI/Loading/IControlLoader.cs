using NinjaGame.Common;
using NinjaGame.UI.Controls;

namespace NinjaGame.UI.Loading
{
    public interface IControlLoader
    {
        IControl LoadControl(string filePath, string id);
        IControl LoadControl(StagedControl stagedControl);
        ILoadBatch<IControl> LoadControlBatch(string filePath, string id);
        StagedControl StageControl(string filePath, string id);
    }
}
