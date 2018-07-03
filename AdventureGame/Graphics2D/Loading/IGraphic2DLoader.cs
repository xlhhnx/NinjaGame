using NinjaGame.Common;
using NinjaGame.Graphics2D.Assets;

namespace NinjaGame.Graphics2D.Loading
{
    public interface IGraphic2DLoader
    {
        IGraphic2D LoadGraphic(string filePath, string id);
        IGraphic2D LoadGraphic(StagedGraphic stagedGraphic);
        ILoadBatch<IGraphic2D> LoadGraphicBatch(string filePath, string id);
        StagedGraphic StageGraphic(string filePath, string id);
    }
}