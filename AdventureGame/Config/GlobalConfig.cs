using System.IO;
using System.Linq;

namespace NinjaGame.Config
{
    public static class GlobalConfig
    {
        public static string InitialAssetBatchFile { get; set; }
        public static string InitialGraphicBatchFile { get; set; }
        public static string AssetBatchFile { get; set; }
        public static string GraphicBatchFile { get; set; }

        public static void Initialize()
        {
            var lines = File.ReadAllLines("config.ini").Where(l => l.Length > 0)
                            .Where(l => l.Contains('='))
                            .ToList();

            foreach (var l in lines)
            {
                var pair = l.Split('=');
                switch (pair[0].Trim().ToLower())
                {
                    case ("initialassetbatchile"):
                        InitialAssetBatchFile = pair[1].Trim();
                        break;
                    case ("initialgraphicbatchfile"):
                        InitialGraphicBatchFile = pair[1].Trim();
                        break;
                    case ("assetbatchfile"):
                        AssetBatchFile = pair[1].Trim();
                        break;
                    case ("grahpicbatchfile"):
                        GraphicBatchFile = pair[1].Trim();
                        break;
                }
            }
        }

    }
}
