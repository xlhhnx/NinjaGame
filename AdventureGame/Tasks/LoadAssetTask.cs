using System.Threading.Tasks;

namespace NinjaGame.Tasks
{
    public class LoadAssetTask : GameTask
    {
        public LoadAssetTask(string filePath, string id, string batchId)
            : base(new Task(() => MainGame.Instance.AssetManager.LoadAsset(filePath, id, batchId)))
        { /* No op */ }
    }
}
