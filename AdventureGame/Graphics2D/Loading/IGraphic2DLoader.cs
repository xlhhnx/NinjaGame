using NinjaGame.Common;
using NinjaGame.Common.Loading;
using NinjaGame.Graphics2D.Assets;

namespace NinjaGame.Graphics2D.Loading
{
    public interface IGraphic2DLoader
    {
        IGraphic2D LoadGraphic(string filePath, string id);
        IGraphic2D LoadGraphicByName(string filePath, string name);
        IGraphic2D LoadGraphic(IDefinition<GraphicType> stagedGraphic);
        ILoadBatch<IGraphic2D> LoadGraphicBatch(string filePath, string id);
        ILoadBatch<IGraphic2D> LoadGraphicBatchByName(string filePath, string name);
        IDefinition<GraphicType> LoadDefinition(string filePath, string id);
    }
}