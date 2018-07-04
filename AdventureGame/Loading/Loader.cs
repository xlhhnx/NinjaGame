using Newtonsoft.Json;
using NinjaGame.Assets;
using NinjaGame.Batches.Loading;
using System;
using System.Collections.Generic;
using System.IO;

namespace NinjaGame.Loading
{
    public abstract class Loader<T, Type> : ILoader<T, Type>
    {
        public abstract List<IAsset> LoadAssets(ILoadBatch<T> batch);

        public virtual List<ILoadBatch<T>> LoadBatches(string filePath)
        {
            var contents = "";
            try
            {
                contents = File.ReadAllText(filePath);
            }
            catch (FileNotFoundException ex)
            {
                // TODO: Log ex
                Console.WriteLine(ex.Message);
                return null;
            }

            var batches = new List<ILoadBatch<T>>();
            try
            {
                var tmpBatches = JsonConvert.DeserializeObject<List<LoadBatch<T>>>(contents);
                foreach (var b in tmpBatches)
                    batches.Add(b);
            }
            catch (JsonException ex)
            {
                // TODO: Log ex
                Console.WriteLine(ex.Message);
                return null;
            }
            return batches;
        }
    }
}
