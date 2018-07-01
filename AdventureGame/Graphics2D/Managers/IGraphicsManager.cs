using NinjaGame.Graphics2D.Assets;
using NinjaGame.Graphics2D.Batches;
using NinjaGame.Graphics2D.Loading;
using System;

namespace NinjaGame.Graphics2D.Managers
{
    public interface IGraphics2DManager
    {
        event Action<IGraphic2DBatch> GraphicBatchLoadedEvent;
        event Action<IGraphic2DBatch> BatchGraphicsLoadedEvent;
        event Action<IGraphic2D> GraphicLoadedEvent;

        IGraphic2DLoader Loader { get; set; }

        void LoadGraphic(string filePath, string id, string batchId);
        void LoadGraphicBatch(string filePath, string id);
        void LoadBatchGraphics(string id);
        void LoadGraphicAsync(string filePath, string id, string batchId);
        void LoadGraphicBatchAsync(string filePath, string id);
        void LoadBatchGraphicsAsync(string id);
        void UnloadBatch(string id);
        void UnloadAll();
        void Recycle();
        bool ContainsBatch(string id);
        bool ContainsBatch(IGraphic2DBatch batch);
        bool GraphicLoaded(string id);
        bool GraphicLoaded(IGraphic2D graphic);
        IGraphic2DBatch GetBatch(string id);
        Image GetImage(string id);
        Sprite GetSprite(string id);
        Text GetText(string id);
        Effect GetEffect(string id);
    }
}