using System.Threading.Tasks;

namespace NinjaGame.Tasks
{
    public class LoadBatchGraphicsTask : GameTask
    {
        public LoadBatchGraphicsTask(string id) 
            : base(new Task(() => MainGame.Instance.GraphicsManager.LoadBatchGraphics(id)))
        { /* No op */ }
    }
}
