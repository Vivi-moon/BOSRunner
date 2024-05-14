using System;
using UnityEditor;

namespace BFB.Build
{
    public class CI
    {
        // public static void BuildAndroid()
        // {
        //     var process = new AndroidBuildProcess();
        //     process.Build();
        // }
        
        // public static void BuildIos()
        // {
        //     var process = new IosBuildProcess();
        //     process.Build();
        // }
        
        public static void BuildWindows()
        {
            var process = new WindowsBuildProcess();
        
            process.Build();
        }
        
        public static void BuildMac()
        {
            var process = new MacBuildProcess();
            process.Build();
        }
    }
}