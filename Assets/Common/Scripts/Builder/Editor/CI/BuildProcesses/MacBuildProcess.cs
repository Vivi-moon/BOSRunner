using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BFB.Build
{
    public class MacBuildProcess : BuildProcess
    {
        private ApplicationPlayerSettings _appPlayerSettings;
        
        public override void Build()
        {
            PerformSettings();
            base.Build();
        }

        protected override string GetPackageName()
        {
            throw new System.NotImplementedException();
        }

        protected override void Setup()
        {
            BuildTarget = BuildTarget.StandaloneOSX;
            base.Setup();
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
            
            _appPlayerSettings = new ApplicationPlayerSettings("Stellar Age", GetPackageName(), BuildProcessArguments.CompanyName, default(SigningSettings));

            return _appPlayerSettings;
        }

        protected override string GetProductName()
        {
            var productionName = ApplicationPlayerSettings.ProductName;

            if (BuildProcessArguments.DistributionType == DistributionType.Production && BuildProcessArguments.BehaviourType == BehaviourType.Production)
            {
                Debug.Log($"[BUILD PROJECT] Product name: {productionName}");
                return productionName;
            }

            var distribution = BuildProcessArguments.DistributionType;
            var behaviour = GetShortBehaviorType();
            var developmentName = $"Stellar Age {distribution}({behaviour})";
            Debug.Log($"[BUILD PROJECT] Product name: {developmentName}");
            return developmentName;
        }

        protected override string GetFileName()
        {
            return string.Empty;
        }
    }
}