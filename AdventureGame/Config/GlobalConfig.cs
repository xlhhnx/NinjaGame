using System.IO;
using System.Linq;

namespace NinjaGame.Config
{
    public static class GlobalConfig
    {
        public static string AssetDefinitionFile { get; set; }
        public static string GrahpicDefinitionFile { get; set; }
        public static string ControlDefinitionFile { get; set; }

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
                    case ("assetdefinitionfile"):
                        AssetDefinitionFile = pair[1].Trim();
                        break;
                    case ("grahpicdefinitionfile"):
                        GrahpicDefinitionFile = pair[1].Trim();
                        break;
                    case ("controldefinitionfile"):
                        ControlDefinitionFile = pair[1].Trim();
                        break;
                }
            }
        }

    }
}
