using NAudio.Wave;

namespace NinjaGame.Assets
{
    public class AudioAsset : BaseAsset
    {
        public override bool Loaded
        {
            get { return _loaded; }
        }

        public virtual WaveStream Stream { get { return _stream; } }


        protected bool _loaded;
        protected WaveStream _stream;


        public AudioAsset(string id, string name, WaveStream stream) 
            : base(id, name)
        {
            _stream = stream;
            _loaded = true;
            _type = AssetType.AudioAsset;
        }

        public override void Unload()
        {
            _loaded = false;
        }
    }
}