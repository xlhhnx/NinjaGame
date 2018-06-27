using NinjaGame.AssetManagement;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaGame.Assets
{
    public struct StagedAsset
    {
        public string Id { get; set; }
        public string FilePath { get; set; }
        public AssetType Type { get; set; }
        public ContentManager Content { get; set; }

        public StagedAsset(string id, string filePath, AssetType type, ContentManager content)
        {
            Id = id;
            FilePath = filePath;
            Type = type;
            Content = content;
        }
    }
}
