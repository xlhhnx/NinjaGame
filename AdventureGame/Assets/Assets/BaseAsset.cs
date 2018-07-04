using NinjaGame.AssetManagement;

namespace NinjaGame.Assets
{
    public abstract class BaseAsset : IAsset
    {
        public virtual string Id { get { return _id; } }
        public virtual string Name { get { return _name; } }
        public virtual AssetType Type { get { return _type; } }

        public abstract bool Loaded { get; }


        protected string _id;
        protected string _name;
        protected AssetType _type;


        public BaseAsset(string id, string name)
        {
            _id = id;
            _name = name;
            _type = AssetType.None;
        }

        public abstract void Unload();
    }
}