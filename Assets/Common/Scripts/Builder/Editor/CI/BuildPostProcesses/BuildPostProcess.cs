using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Collections.Generic;

namespace BFB.Build
{
    public class BuildPostProcess
    {
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
    #if PRODUCTION
            
            string configPath = Path.Combine(Path.GetDirectoryName(path), PlayerSettings.productName + "_Data", "StreamingAssets", "configs.json");
            if (!File.Exists(configPath))
            {
                Debug.LogError("[PostBuild] Config path not found: " + configPath);
            }
            else
            {
                string[] data = File.ReadAllLines(configPath);
                var loadListData = new List<string>(data);

                loadListData.RemoveAll(str => str.Contains("SkipBosConnection"));
                loadListData.RemoveAll(str => str.Contains("SimulateConnection"));

                File.WriteAllLines(configPath, loadListData.ToArray());
            }
    #endif
        }
    }
}