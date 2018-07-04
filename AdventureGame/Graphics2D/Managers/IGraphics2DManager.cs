using NinjaGame.Batches.Loading;
using NinjaGame.Graphics2D.Assets;
using NinjaGame.Graphics2D.Loading;
using System;

namespace NinjaGame.Graphics2D.Managers
{
    public interface IGraphics2DManager
    {
        event Action<ILoadBatch<IGraphic2D>> GraphicBatchLoadedEvent;
        event Action<ILoadBatch<IGraphic2D>> BatchGraphicsLoadedEvent;
        event Action<IGraphic2D> GraphicLoadedEvent;

        Graphic2DLoader Loader { get; set; }

        void LoadBatches(string filePath);
        void LoadBatchGraphics(ILoadBatch<IGraphic2D> batch);
        void LoadBatchGrahpicsById(string id);
        void LoadBatchGrahpicsByName(string name);
        void LoadBatchesAsync(string filePath);
        void LoadBatchGraphicsAsync(ILoadBatch<IGraphic2D> batch);
        void LoadBatchGraphicsByIdAsync(string id);
        void LoadBatchGraphicsByNameAsync(string name);
        void UnloadBatch(string id);
        void UnloadAll();
        void Recycle();
        bool ContainsBatch(string id);
        bool ContainsBatch(ILoadBatch<IGraphic2D> batch);
        bool GraphicLoaded(string id);
        bool GraphicLoaded(IGraphic2D graphic);
        ILoadBatch<IGraphic2D> GetBatch(string id);
        Image GetImage(string id);
        Sprite GetSprite(string id);
        Text GetText(string id);
        Effect GetEffect(string id);
    }
}