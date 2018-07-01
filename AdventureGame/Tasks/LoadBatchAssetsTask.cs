using System.Threading.Tasks;

namespace NinjaGame.Tasks
{
    public class LoadBatchAssetsTask : GameTask
    {
        public LoadBatchAssetsTask(string id) 
            : base(new Task(() => MainGame.Instance.AssetManager.LoadBatchAssets(id)))
        { /* No op */ }
    }
}
