using System.Threading.Tasks;

namespace NinjaGame.Tasks
{
    public class LoadAssetBatchTask : GameTask
    {
        public string Id { get; set; }

        public LoadAssetBatchTask(string filePath, string id)
            : base(new Task(() => MainGame.Instance.AssetManager.LoadAssetBatch(filePath, id)))
        {
            Id = id;
        }
    }
}
