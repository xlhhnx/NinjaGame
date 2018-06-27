using NinjaGame.AssetManagement;

namespace NinjaGame.Assets
{
    public abstract class BaseAsset : IAsset
    {
        public virtual string Id { get { return _id; } }
        public virtual AssetType Type { get { return _type; } }

        public abstract bool Loaded { get; }


        protected string _id;
        protected AssetType _type;


        public BaseAsset(string id)
        {
            _id = id;
            _type = AssetType.None;
        }

        public abstract void Unload();
    }
}