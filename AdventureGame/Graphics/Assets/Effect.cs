using System;

namespace NinjaGame.Graphics2D.Assets
{
    public class Effect : BaseGraphic2D
    {
        public override bool Loaded { get { throw new NotImplementedException(); } }
        public override GraphicType GraphicType { get { return GraphicType.Effect; } }

        public override IGraphic2D Copy()
        {
            throw new NotImplementedException();
        }
    }
}