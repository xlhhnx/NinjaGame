using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaGame.Config
{
    public static class GlobalConfig
    {
        public static string InitialAssetDefinitionFile { get; set; }
        public static string InitialAssetBatchId { get; set; }
        public static List<string> InitialGraphicIds { get; set; }
        public static string StartupAssetDefinitionFile { get; set; }
        public static string StartupAssetBatchId { get; set; }
        public static List<string> StartupGraphicIds { get; set; }
        public static string DefaultGraphicsDefinitionFile { get; set; }

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
                    case ("initialgraphicids"):
                        {
                            var gids = pair[1].Trim().Split(',');
                            for (int i = 0; i < gids.Length; i++)
                                gids[i] = gids[i].Trim();

                            InitialGraphicIds = new List<string>(gids);
                        }
                        break;
                    case ("startupassetdefinitionfile"):
                        StartupAssetDefinitionFile = pair[1].Trim();
                        break;
                    case ("startupassetbatchid"):
                        StartupAssetBatchId = pair[1].Trim();
                        break;
                    case ("startupgraphicids"):
                        {
                            var gids = pair[1].Trim().Split(',');
                            for (int i = 0; i < gids.Length; i++)
                                gids[i] = gids[i].Trim();

                            StartupGraphicIds = new List<string>(gids);
                        }
                        break;
                    case ("defaultgraphicsdefinitionfile"):
                        DefaultGraphicsDefinitionFile = pair[1].Trim();
                        break;
                }
            }
        }

    }
}
