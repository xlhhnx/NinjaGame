using System.Threading.Tasks;

namespace NinjaGame.Tasks
{
    public class LoadGraphicTask : GameTask
    {
        public LoadGraphicTask(string filePath, string id, string batchId) 
            : base(new Task(() => MainGame.Instance.GraphicsManager.LoadGraphic(filePath, id, batchId)))
        { /* No op */ }
    }
}
