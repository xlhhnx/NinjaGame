using NinjaGame;
using System.Threading.Tasks;

namespace NinjaGame.Tasks
{
    public class LoadGraphicsBatchTask : GameTask
    {
        public string Id { get; set; }

        public LoadGraphicsBatchTask(string filePath, string id)
            : base(new Task(() => MainGame.Instance.GraphicsManager.LoadGraphicBatch(filePath, id)))
        {
            Id = id;
        }
    }
}
