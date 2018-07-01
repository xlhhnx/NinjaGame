using NinjaGame.Graphics2D.Assets;
using System;
using System.Collections.Generic;

namespace NinjaGame.Graphics2D.Batches
{
    public interface IGraphic2DBatch
    {
        string Id { get; }
        List<IGraphic2D> Graphics { get; set; }
        Dictionary<string, List<string>> FileIdDict { get; set; }
        
        void AddGraphicDefinition(string filePath, string graphicId);
        void AddGraphic(IGraphic2D graphic);
        List<Tuple<string, string>> GetAllFileIdPairs();
    }
}
