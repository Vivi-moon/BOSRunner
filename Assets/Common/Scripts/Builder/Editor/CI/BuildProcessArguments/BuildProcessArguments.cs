using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace BFB.Build
{
    public class BuildProcessArguments 
    {
    
        public enum TargetServerType
        {
            Prod,
            Staging,
            Pre,
        }

        private const string SharedLogicVersionValue = "sharedLogicVersion";
        private const string BaseIosVersionValue = "baseIosVersion";
        private const string BuildSymbolsValue = "buildSymbols";
        private const string TargetRealmValue = "targetRealm";
        private const string DistributionTypeValue = "distribution";
        private const string StatProjectValue = "statProject";
        private const string BundleCodeValue = "buildNumber";
        private const string TargetServerValue = "targetServer";
        private const string DeviceIdValue = "deviceId";
        private const string CommitHashValue = "commit";
        private const string GitBranchValue = "branch";
        private const string VersionValue = "version";
        private const string AtlasesValue = "atlases";
        private const string BackendValue = "backend";
        private const string DebugValue = "debug";
        private const string SignValue = "sign";

        public BuildProcess.DistributionType DistributionType { get; private set; }
        public BuildProcess.BehaviourType BehaviourType { get; private set; }
        public ScriptingImplementation BackendType { get; private set; }
        public string BuildSymbols { get; private set; }
        public int BuildNumber { get; private set; }
        public string Version { get; private set; }
        public string CompanyName { get; private set; }
        public string PackagePattern { get; private set; }
        public string BuildPath { get; private set; }

        public bool IsDebug { get; private set; }
        public bool IsControlAppSimulate { get; private set; }
        public bool ExportToAndroidProject { get; private set; }
        

        private string _buildVersion = string.Empty;
        private BuildTarget _buildTarget;

        public static BuildProcessArguments CreateFromEditor(EditorBuildProcessArguments args)
        {
            var arguments = new BuildProcessArguments
            {
                _buildTarget = args.BuildTarget,
                BuildSymbols = args.BuildSymbols,
                Version = args.Version,
                BuildNumber = args.BuildNumber,
                DistributionType = args.DistributionType,
                BackendType = args.BackendType,
                IsDebug = args.IsDebug,
                IsControlAppSimulate = args.IsControlAppSimulate,
                BehaviourType = args.BehaviourType,
                CompanyName = args.CompanyName,
                PackagePattern = args.PackagePattern,
                ExportToAndroidProject = args.ExportToAndroidProject,
                BuildPath = args.BuildPath
            };
            return arguments;
        }

        public BuildProcessArguments()
        {
            
        }

        public BuildProcessArguments(BuildTarget buildTarget)
        {
            _buildTarget = buildTarget;
            
            BuildSymbols = GetCommandLineArg(BuildSymbolsValue);
            Version = GetCommandLineArg(VersionValue);

            DistributionType = GetDistributionType();

            BackendType = GetBackendType();
            
            int buildNumber;
            if (!int.TryParse(GetCommandLineArg(BundleCodeValue), out buildNumber))
            {
                throw new Exception("BuildProcessArgumentParseException: Invalid build number has been passed " + BundleCodeValue);
            }
            
            BuildNumber = buildNumber;
            
            bool isDebug;
            bool.TryParse(GetCommandLineArg(DebugValue), out isDebug);
            IsDebug = isDebug;
            IsControlAppSimulate = false;
        }
    
        public override string ToString()
        {
            var result = "";
            result += $"[BUILD ARGUMENT] Parsed arguments below ------- \n";
            result += $"\n";
            if (!string.IsNullOrEmpty(BuildSymbols)) result += $"[BUILD ARGUMENT] Symbols : {BuildSymbols}\n";
            if (!string.IsNullOrEmpty(Version)) result += $"[BUILD ARGUMENT] Version : {Version}\n";
            result += $"[BUILD ARGUMENT] Distribution Type : {DistributionType}\n";
            result += $"[BUILD ARGUMENT] Behaviour type : {BehaviourType}\n";
            result += $"[BUILD ARGUMENT] Is Debuggable : {IsDebug}\n";
            result += $"[BUILD ARGUMENT] Build number : {BuildNumber}\n";
            result += $"[BUILD ARGUMENT] Backend type : {BackendType}\n";
            result += $"\n";
            result += $"[BUILD PROJECT] ------------------ Generated values -------------- \n";
            result += $"[BUILD PROJECT] Build version : {GetBuildVersion()}\n";
            return result;
        }

        public string GetBuildVersion()
        {
            if (!string.IsNullOrEmpty(_buildVersion))
            {
                return _buildVersion;
            }
            _buildVersion = DateTime.Now.ToString("dd.MM.y.HHmm");
            return _buildVersion;
        }
        
        private ScriptingImplementation GetBackendType()
        {
            var rawValue = GetCommandLineArg(BackendValue);
            var lowerCased = rawValue.ToLower();
            return lowerCased == "mono" ? ScriptingImplementation.Mono2x : ScriptingImplementation.IL2CPP;
        }
        
        private string GetPtrIOSBuildVersion(string parsedVersion)
        {
            return parsedVersion.Substring(0, parsedVersion.LastIndexOf('.'));
        }

        private string GetIOSBuildVersion(string parsedVersion, string serverId, string baseIosVersion)
        {
            Match slVersion = Regex.Match(parsedVersion, @"^(\d+.\d+).(\d+).(\d+)");

            if (!slVersion.Success)
            {
                throw new Exception("[BUILD PROJECT] | Exception: Can't determine iOS bundle version, shared logic version is invalid.");
            }

            string majorVersion = slVersion.Groups[1].Value.Replace(".", string.Empty);
            string midVersion = slVersion.Groups[2].Value;
            string minorVersion = slVersion.Groups[3].Value;

            int baseVersionValue = int.Parse(baseIosVersion);
            int majorVersionValue = int.Parse(majorVersion);

            string bundleMajorVersion = (baseVersionValue + majorVersionValue).ToString();
            string bundleMinorVersion;

            if (serverId.Equals(TargetServerType.Staging.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                bundleMinorVersion = "0";
            }
            else if (serverId.Equals(TargetServerType.Prod.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                bundleMinorVersion = "1";
            }
            else
            {
                bundleMinorVersion = "0";
                Debug.LogWarning("Server id is neither STAGING or PROD so giving it 0 minor version by default");   
            }

            var finalBundleVersion = $"{bundleMajorVersion}.{midVersion}{minorVersion}.{bundleMinorVersion}";
            return finalBundleVersion;
        }
        
        private BuildProcess.DistributionType GetDistributionType()
        {
            var rawValue = GetCommandLineArg(DistributionTypeValue);
            var lowerCased = rawValue.ToLower();
            return lowerCased == "production" ? BuildProcess.DistributionType.Production :  BuildProcess.DistributionType.Development;
        }

        private static string GetCommandLineArg(string name)
        {
            var args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == name && args.Length > i + 1)
                {
                    return args[i + 1];
                }
            }

            return string.Empty;
        }
    }
}