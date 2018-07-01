using System.IO;
using System.Linq;

namespace NinjaGame.Config
{
    public static class GlobalConfig
    {
        public static string DefaultGraphicDefinitionFile { get; set; }
        public static string DefaultAssetDefinitionsFile { get; set; }
        public static string InitialAssetDefinitionFile { get; set; }
        public static string InitialAssetBatchId { get; set; }
        public static string InitialGraphicDefinitionFile { get; set; }
        public static string InitialGraphicBatchId { get; set; }
        public static string StartupAssetDefinitionFile { get; set; }
        public static string StartupAssetBatchId { get; set; }
        public static string StartupGraphicDefinitionFile { get; set; }
        public static string StartupGraphicBatchId { get; set; }

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
                    case ("initialassetdefinitionfile"):
                        InitialAssetDefinitionFile = pair[1].Trim();
                        break;
                    case ("initialassetbatchid"):
                        InitialAssetBatchId = pair[1].Trim();
                        break;
                    case ("initialgraphicdefinitionfile"):
                        InitialGraphicDefinitionFile = pair[1].Trim();
                        break;
                    case ("initialgraphicbatchid"):
                        InitialGraphicBatchId = pair[1].Trim();
                        break;
                    case ("startupassetdefinitionfile"):
                        StartupAssetDefinitionFile = pair[1].Trim();
                        break;
                    case ("startupassetbatchid"):
                        StartupAssetBatchId = pair[1].Trim();
                        break;
                    case ("startupgraphicdefinitionfile"):
                        StartupGraphicDefinitionFile = pair[1].Trim();
                        break;
                    case ("startupgraphicbatchid"):
                        StartupGraphicBatchId = pair[1].Trim();
                        break;
                    case ("defaultgraphicdefinitionfile"):
                        DefaultGraphicDefinitionFile = pair[1].Trim();
                        break;
                    case ("defaultassetdefinitionfile"):
                        DefaultAssetDefinitionsFile = pair[1].Trim();
                        break;
                }
            }
        }

    }
}
