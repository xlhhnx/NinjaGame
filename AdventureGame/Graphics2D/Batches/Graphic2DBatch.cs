using NinjaGame.Graphics2D.Assets;
using System;
using System.Collections.Generic;

namespace NinjaGame.Graphics2D.Batches
{
    public class Graphic2DBatch : IGraphic2DBatch
    {
        public string Id { get => _id; protected set => _id = value; }
        public List<IGraphic2D> Graphics { get => _graphics; set => _graphics = value; }
        public Dictionary<string, List<string>> FileIdDict { get => _fileIdDict; set => _fileIdDict = value; }


        protected string _id;
        protected List<IGraphic2D> _graphics;
        protected Dictionary<string, List<string>> _fileIdDict;


        public Graphic2DBatch(string id)
        {
            Id = id;
            Graphics = new List<IGraphic2D>();
            FileIdDict = new Dictionary<string, List<string>>();
        }

        public void AddGraphic(IGraphic2D graphic)
        {
            if (!Graphics.Contains(graphic))
                Graphics.Add(graphic);
        }

        public void AddGraphicDefinition(string filePath, string graphicId)
        {
            if (!FileIdDict.ContainsKey(filePath))
                FileIdDict.Add(filePath, new List<string>());

            if (FileIdDict[filePath] is null)
                FileIdDict[filePath] = new List<string>();

            FileIdDict[filePath].Add(graphicId);
        }

        public List<Tuple<string, string>> GetAllFileIdPairs()
        {
            var pairs = new List<Tuple<string, string>>();
            foreach (var file in FileIdDict.Keys)
            {
                foreach (var id in FileIdDict[file])
                    pairs.Add(new Tuple<string, string>(file, id));
            }
            return pairs;
        }
    }
}
