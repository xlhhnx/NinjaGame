using NinjaGame.Graphics2D.Assets;
using NinjaGame.Graphics2D.Batches;
using System.Threading.Tasks;

namespace NinjaGame.Graphics2D.Loading
{
    public interface IGraphic2DLoader
    {
        IGraphic2D LoadGraphic(string filePath, string id);
        IGraphic2D LoadGraphic(StagedGraphic stagedGraphic);
        IGraphic2DBatch LoadGraphicBatch(string filePath, string id);
        StagedGraphic StageGraphic(string filePath, string id);
    }
}