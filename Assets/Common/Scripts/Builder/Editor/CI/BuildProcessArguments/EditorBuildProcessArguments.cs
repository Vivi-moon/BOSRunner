using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;

namespace BFB.Build
{
    public class EditorBuildProcessArguments
    {
        public BuildTarget BuildTarget;
        public string BuildSymbols;
        public string Version;
        public int BuildNumber = 0;
        public BuildProcess.DistributionType DistributionType;
        public BuildProcess.BehaviourType BehaviourType;
        public ScriptingImplementation BackendType = ScriptingImplementation.IL2CPP;
        public bool IsControlAppSimulate;
        public bool IsDebug;
        public bool ExportToAndroidProject;
        public string PackagePattern;
        public string CompanyName;
        public string BuildPath;
    }
}
