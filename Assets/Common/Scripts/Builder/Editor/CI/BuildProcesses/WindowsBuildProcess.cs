using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LightBuzz.Archiver;
using UnityEditor;
using UnityEngine;

namespace BFB.Build
{
    public class WindowsBuildProcess : BuildProcess
    {
        private ApplicationPlayerSettings _appPlayerSettings;
        
        public override void Build()
        {
            PerformSettings();
            base.Build();
        }

        protected override string GetPackageName()
        {
            var packageName = $"Software.Neurotech.{Application.productName}";
            return packageName;
        }

        protected override void Setup()
        {
            BuildTarget = BuildTarget.StandaloneWindows64;
            base.Setup();
        }

        protected override void PostBuild()
        {
            base.PostBuild();

            string gamePath = Path.GetDirectoryName(GetLocationPath()) + @"\";

            string timeStr = DateTime.Now.ToString("yyyy_MM_dd_HH_mm");
            string simulateStr = BuildProcessArguments.IsControlAppSimulate
                    ? "(SIMULATE)"
                    : "";
            string zipPath = string.Format(@"{0}\{1}_{2}{3}.zip"
                , Path.GetDirectoryName(BuildProcessArguments.BuildPath)
                , GetFileNameWithoutExtension().Replace(" ", "_")
                , timeStr
                , simulateStr
            );

            Archiver.Compress(gamePath, zipPath);
        }

        protected override string GetLocationPath()
        {
            return Path.Combine(BuildProcessArguments.BuildPath, GetFileNameWithoutExtension());
        }

        protected override void SetupSymbols()
        {
            var symbols = BuildProcessArguments.BuildSymbols;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(GetBuildTargetGroup(), symbols);
        }
    
        protected override ApplicationPlayerSettings GetApplicationPlayerSettings()
        {
            if (_appPlayerSettings != null)
            {
                return _appPlayerSettings;
            }
            
            _appPlayerSettings = new ApplicationPlayerSettings(Application.productName, GetPackageName(), BuildProcessArguments.CompanyName, default(SigningSettings));

            return _appPlayerSettings;
        }

        protected override string GetProductName()
        {
            var productionName = ApplicationPlayerSettings.ProductName;
            return productionName;
        }

        protected override string GetFileName()
        {
            var behaviour = BuildProcessArguments.BehaviourType;
            
            var fileName = $"{GetFileNameWithoutExtension()}.exe";
            Debug.Log("[BUILD PROJECT] Output file name: " + fileName);
            return $"/{fileName}";
        }

        private string GetFileNameWithoutExtension() => Application.productName;
    }
}