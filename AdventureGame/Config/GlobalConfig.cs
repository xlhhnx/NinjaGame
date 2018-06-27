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
        public static string StartupAssetDefinitionFile { get; set; }
        public static string StartupAssetBatchId { get; set; }

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
                    case ("startupassetdefinitionfile"):
                        StartupAssetDefinitionFile = pair[1].Trim();
                        break;
                    case ("startupassetbatchid"):
                        StartupAssetBatchId = pair[1].Trim();
                        break;
                }
            }
        }

    }
}
