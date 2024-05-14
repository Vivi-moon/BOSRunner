using System.IO;
using UnityEditor;
using UnityEngine;

namespace BFB.Build
{
    public class BuildProjectEditorWindow : EditorWindow
    {

        public enum BuildPlatform
        {
            Windows,
            // Android
        }
        
        private const string PackagePattern = "Software.Neurotech.{0}";
        private const string CompanyName = "Neurotech";
        private const string BUILD_FOLDER_NAME = "Build";
        
        private BuildPlatform _buildTarget = BuildPlatform.Windows;
        private BuildProcess.DistributionType _distributionType = BuildProcess.DistributionType.Development;
        private BuildProcess.BehaviourType _behaviourType = BuildProcess.BehaviourType.Development;
        private ScriptingImplementation _spriptingBackend = ScriptingImplementation.Mono2x;
        private bool _controlAppSimulateEnabled;
        private BuildProcessArguments.TargetServerType _targetServer = BuildProcessArguments.TargetServerType.Prod;
        private string _buildSymbols;
        private int _buildNumber = 0;
        private bool _debugEnabled = true;
        private bool _exportAndroid = false;
        private bool _cleanLastBuild = true;


        [MenuItem("CI/Build Window %u", false, 400)]
        static void Init()
        {
            var window = (BuildProjectEditorWindow) GetWindow(typeof (BuildProjectEditorWindow), false, "Build Project");
            window.Show();
        }

        public BuildProjectEditorWindow()
        {
            
        }

        private void DrawToggleButton(ref bool val, string name)
        {
            var oldBackgorundColor = GUI.backgroundColor;
            GUI.backgroundColor = val ? Color.green : oldBackgorundColor;
            val = GUILayout.Toggle(val, name, "Button");
            GUI.backgroundColor = oldBackgorundColor;
        }

        private void OnGUI()
        {
            GUILayout.Space(15);

            _buildTarget = (BuildPlatform)EditorGUILayout.EnumPopup("Target platform", _buildTarget);
    //        GUILayout.Space(5);
    //        _distributionType = (BuildProcess.DistributionType)EditorGUILayout.EnumPopup("Distribution type", _distributionType);
            GUILayout.Space(5);
            _behaviourType = (BuildProcess.BehaviourType)EditorGUILayout.EnumPopup("Behaviour type", _behaviourType);
            GUILayout.Space(5);
            _spriptingBackend = (ScriptingImplementation)EditorGUILayout.EnumPopup("Scripting implementation", _spriptingBackend);
            GUILayout.Space(15);

            GUILayout.Label("BUILD SYMBOLS", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
                //DrawToggleButton(ref _helpshiftDisabled, "Disable Helpshift");
                //DrawToggleButton(ref _cheatsEnabled, "Cheats");
                //DrawToggleButton(ref _debugPanelEnabled, "Debug Panel");
                //DrawToggleButton(ref _realInApps, "Real In Apps");
                //DrawToggleButton(ref _qaBuild, "QA");
                //DrawToggleButton(ref _isPtr, "PTR");
                //DrawToggleButton(ref _noGpgs, "No GPGS");
                SetProductionSettings();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
                GUILayout.Label( ConstructBuildSymbols(), EditorStyles.label);
            GUILayout.EndHorizontal();

            //_prodGroup = _behaviourType == BuildProcess.BehaviourType.Production || _behaviourType == BuildProcess.BehaviourType.PreProduction;

            GUILayout.Space(15);
            GUILayout.Label("PRODUCTION SETTINGS", EditorStyles.boldLabel);
            /*GUI.enabled = _prodGroup;
                _targetServer = (BuildProcessArguments.TargetServerType)EditorGUILayout.EnumPopup("Target server", _targetServer);
                GUILayout.Space(5);
                _gitCommit = EditorGUILayout.TextField("Commit Hash:", _gitCommit);
                GUILayout.Space(5);

                var branch = $"{defaultBranch}{GetBranch()}";
                _gitBranch = EditorGUILayout.TextField($"Branch: {branch}", _gitBranch);
                GUILayout.Space(5);
                _buildNumber = EditorGUILayout.IntField("Build number (optional):", _buildNumber);
                GUILayout.Space(5);
                _sharedLogicVersion = EditorGUILayout.TextField("Shared Logic Version:", _sharedLogicVersion);
                GUILayout.Space(5);
                _statProject = EditorGUILayout.TextField("Stat Project:", _statProject);
                GUILayout.Space(5);
            
            GUI.enabled = true;*/

            GUILayout.Space(15);
            GUILayout.Label("SETTINGS", EditorStyles.boldLabel);

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
                DrawToggleButton(ref _debugEnabled, "Is Debuggable");
                // GUI.enabled = _buildTarget == BuildPlatform.Android;
                // DrawToggleButton(ref _exportAndroid, "Export Android Project");
                // GUI.enabled = true;
                DrawToggleButton(ref _cleanLastBuild, "Remove last build");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
                DrawToggleButton(ref _controlAppSimulateEnabled, "Is Control App Simulate");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

    //        GUILayout.Label("Custom connections", EditorStyles.miniLabel);
            //_deviceId = EditorGUILayout.TextField("Device ID", _deviceId);

            GUILayout.Space(20);

            if (GUILayout.Button("SAVE SETTINGS"))
            {
                SaveSettings();
                EditorGUIUtility.ExitGUI();
            }
            if (GUILayout.Button("BUILD"))
            {
                Build();
                EditorGUIUtility.ExitGUI();
            }
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("OPEN BUILD PATH", GUILayout.MinWidth(200)))
            {
                RevealBuildFolder();
                EditorGUIUtility.ExitGUI();
            }
            GUILayout.EndHorizontal();
        }

        private void SetProductionSettings()
        {
            if (_behaviourType != BuildProcess.BehaviourType.Production) return;
            _debugEnabled = false;
        }

        private string ConstructBuildSymbols()
        {
            var symbols = string.Empty;

            if (_behaviourType == BuildProcess.BehaviourType.Development)
            {
                symbols += "DEVELOPMENT;";
            }
            
            if (_behaviourType == BuildProcess.BehaviourType.Production)
            {
                symbols += "PRODUCTION;";
            }

            if (_controlAppSimulateEnabled)
            {
                if (_behaviourType == BuildProcess.BehaviourType.Production)
                {
                    Debug.LogWarning("[BUILD PROJECT] Simulate enabled for production, maybe need to disable?");
                }
                symbols += "SIMULATE;";
            }

            symbols = symbols.TrimEnd(';');
            
            return symbols;
        }

        private BuildTarget GetBuildTarget()
        {
            // return _buildTarget == BuildPlatform.Android
            //     ? BuildTarget.Android
            //     : BuildTarget.StandaloneWindows64;
            return BuildTarget.StandaloneWindows64;
        }

        private bool SwitchAndValidateBuildTarget(BuildTarget buildTarget)
        {
            if (EditorUserBuildSettings.activeBuildTarget != buildTarget)
            {
                if (!EditorUtility.DisplayDialog("Switch platform?", "You need to switch to " + _buildTarget + ". Start switching now?", "Ok, start reimport", "No"))
                {
                    return false;
                }
        
    #pragma warning disable 618
                EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);
    #pragma warning restore 618
            }

            return true;
        }

        private EditorBuildProcessArguments GetBuildArgs(BuildTarget buildTarget)
        {
            return new EditorBuildProcessArguments
            {
                BuildTarget = buildTarget,
                IsDebug = _debugEnabled,
                IsControlAppSimulate = _controlAppSimulateEnabled,
                DistributionType = _distributionType,
                BuildSymbols = _buildSymbols,
                BuildNumber = _buildNumber,
                BackendType = _spriptingBackend,
                BehaviourType = _behaviourType,
                CompanyName = CompanyName,
                PackagePattern = PackagePattern,
                ExportToAndroidProject = _exportAndroid,
                BuildPath = GetBuildPath()
            };
        } 

        private string GetBuildPath()
        {
            string directoryPath = Application.dataPath.Replace("/Assets", string.Empty);
            return Path.Combine(directoryPath
                            , BUILD_FOLDER_NAME
                            , GetBuildTarget().ToString()
            );
        }

        private void RevealBuildFolder()
        {
            string path = GetBuildPath();

            if (!Directory.Exists(path))
            {
                path = Directory.GetParent(path).ToString();
            }

            EditorUtility.RevealInFinder(path);
        }

        private void SaveSettings()
        {
            var systemBuildTarget = GetBuildTarget();

            if (!SwitchAndValidateBuildTarget(systemBuildTarget)) { return; }
            _buildSymbols = ConstructBuildSymbols();

            var preparedArguments = GetBuildArgs(systemBuildTarget);
            
            EditorCI.GetBuildProcess(preparedArguments).PerformSettings();
        }

        private void Build()
        {
            if (_cleanLastBuild)
            {
                var buildPath = GetBuildPath();
                
                Debug.Log(buildPath);
                
                if (Directory.Exists(buildPath))
                {
                    Directory.Delete(buildPath, true);
                    Debug.Log("Build directory removed!");
                }
            }

            var systemBuildTarget = GetBuildTarget();

            if (!SwitchAndValidateBuildTarget(systemBuildTarget)) { return; }
            SaveSettings();
            
            var preparedArguments = GetBuildArgs(systemBuildTarget);
            
            EditorCI.BuildFromEditor(preparedArguments);
        }
    }
}