using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace BFB.Build
{
    public abstract class BuildProcess
    {
        public enum DistributionType
        {
            Production,
            Development
        }

        public enum BehaviourType
        {
            Development,
            Production
        }
        
        protected BuildProcessArguments BuildProcessArguments { get; private set; }
        protected BuildTarget BuildTarget { get; set; }
        
        protected ApplicationPlayerSettings ApplicationPlayerSettings => GetApplicationPlayerSettings();

        public void SetEditorBuildProcessArguments(EditorBuildProcessArguments editorBuildArguments)
        {
            if (editorBuildArguments == null)
            {
                return;
            }
            BuildProcessArguments = BuildProcessArguments.CreateFromEditor(editorBuildArguments);
        }

        protected abstract void SetupSymbols();

        protected abstract string GetProductName();
        
        protected abstract string GetFileName();
        
        protected abstract ApplicationPlayerSettings GetApplicationPlayerSettings();

        protected abstract string GetPackageName();
    
        protected virtual void Setup()
        {
            CreateBuildProcessArguments();
            SetupSymbols();
        }

        public void PerformSettings()
        {
            Setup();
            SetupPlayerSettings();
            SetupLogTypes();
            SetupUnityDebugSettings();
            Refresh();
        }

        public virtual void Build()
        {
            Debug.Log($"[BUILD PROJECT]  {BuildTarget} build process started");
            
            Refresh();
            
            try
            {
                string[] scenes = GetScenePaths();

                if (scenes.Length == 0)
                {
                    throw new Exception("[BUILD PROJECT] | Exception: No scenes found for build.");
                }

                var buildOptions = BuildProcessArguments.IsDebug ? BuildOptions.Development : BuildOptions.None;
                
                var locationPath = GetLocationPath() + GetFileName();
                var result = BuildPipeline.BuildPlayer(scenes, locationPath, BuildTarget, buildOptions);

                if (result.summary.result == BuildResult.Failed)
                {
                    throw new Exception("[BUILD PROJECT] | Exception: [*********] BuildPipeline.BuildPlayer has failed:\n" + result.summary + "\n [*********]");
                }

                if (result.summary.result == BuildResult.Succeeded)
                {
                    PostBuild();
                }

                Debug.Log("[*********] Build successfull [*********]");
            }
            finally
            {
                OnBuildFailedHandler();
            }
        }

        protected virtual void PostBuild()
        {
            
        }

        protected virtual string GetLocationPath()
        {
            return BuildProcessArguments.BuildPath;
        }

        protected virtual void OnBuildFailedHandler()
        {
            //Override it if you need it
        }
        
        protected  BuildTargetGroup GetBuildTargetGroup()
        {
            switch (BuildTarget)
            {
                case BuildTarget.iOS:
                    return BuildTargetGroup.iOS;
                case BuildTarget.Android:
                    return BuildTargetGroup.Android;
                default:
                    return BuildTargetGroup.Standalone;
            }
        }

        protected virtual void SetupPlayerSettings()
        {
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
            PlayerSettings.SetScriptingBackend(GetBuildTargetGroup(), BuildProcessArguments.BackendType);
            
            PlayerSettings.SplashScreen.show = false;
            PlayerSettings.SplashScreen.showUnityLogo = false;
            PlayerSettings.bundleVersion = BuildProcessArguments.GetBuildVersion();
            PlayerSettings.SetApplicationIdentifier(GetBuildTargetGroup(), ApplicationPlayerSettings.Identifier);
            PlayerSettings.companyName = ApplicationPlayerSettings.CompanyName;
            PlayerSettings.productName = GetProductName();
        }

        protected void Refresh()
        {
            AssetDatabase.SaveAssets();
            Debug.Log("[BUILD PROJECT] Assets saved.");
        }
    
        protected void SetupLogTypes()
        {
            switch (BuildProcessArguments.BehaviourType)
            {
                case BehaviourType.Production:
                    PlayerSettings.usePlayerLog = false;
                    PlayerSettings.logObjCUncaughtExceptions = true;
                    PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
                    PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.Full);
                    PlayerSettings.SetStackTraceLogType(LogType.Error, StackTraceLogType.None);
                    PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.None);
                    PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.None);
                    break;
                case BehaviourType.Development:
                    PlayerSettings.usePlayerLog = true;
                    PlayerSettings.logObjCUncaughtExceptions = true;
                    PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
                    PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.Full);
                    PlayerSettings.SetStackTraceLogType(LogType.Error, StackTraceLogType.Full);
                    PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);
                    PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.Full);
                    break;
            }
        }
        
        protected string[] GetScenePaths()
        {
            return (from currentScene in EditorBuildSettings.scenes where currentScene != null && currentScene.enabled select currentScene.path).ToArray();
        }
        
        protected void SetupUnityDebugSettings()
        {
            EditorUserBuildSettings.development = BuildProcessArguments.IsDebug;
        }

        protected string GetShortBehaviorType()
        {
            switch (BuildProcessArguments.BehaviourType)
            {
                case BehaviourType.Production:
                    return "Prod";
                case BehaviourType.Development:
                    return "Dev";
                default:
                    return "None";
            }
        }

        private void CreateBuildProcessArguments()
        {
            if (BuildProcessArguments == null)
            {
                BuildProcessArguments = new BuildProcessArguments(BuildTarget);
            }
            Debug.Log(BuildProcessArguments.ToString());
        }
    }

    public class ApplicationPlayerSettings
    {
        public string ProductName { get; }
        public string Identifier { get; }
        public string CompanyName { get; }
        public SigningSettings Signing { get; }
        
        public ApplicationPlayerSettings(string productName, string identifier, string companyName, SigningSettings signing)
        {
            ProductName = productName;
            Identifier = identifier;
            CompanyName = companyName;
            Signing = signing;
        }
    }

    public struct SigningSettings
    {
        public string KeystoreName { get; }
        public string KeystorePass { get; }
        public string KeyaliasName { get; }
        public string KeyaliasPass { get; }

        public SigningSettings(string keystoreName, string keystorePass, string keyaliasName, string keyaliasPass) : this()
        {
            KeystoreName = $"Signing/{keystoreName}.keystore";
            KeystorePass = keystorePass;
            KeyaliasName = keyaliasName;
            KeyaliasPass = keyaliasPass;
        }
    }
}