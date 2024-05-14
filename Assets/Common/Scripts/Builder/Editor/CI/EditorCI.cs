using System;
using UnityEditor;

namespace BFB.Build
{
    public class EditorCI 
    {
        public static void BuildFromEditor(EditorBuildProcessArguments args)
        {
            var process = GetBuildProcess(args);
            process.Build();
        }

        public static BuildProcess GetBuildProcess(EditorBuildProcessArguments args)
        {
            BuildProcess process;
            switch (args.BuildTarget)
            {
                // case BuildTarget.Android:
                //     process = new AndroidBuildProcess();
                //     break;
                // case BuildTarget.iOS:
                //     process = new IosBuildProcess();
                //     break;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    process = new WindowsBuildProcess();
                    break;
                case BuildTarget.StandaloneOSX:
                    process = new MacBuildProcess();
                    break;
                default:
                    throw new TypeLoadException($"Invalid build target: {args.BuildTarget}");
            }

            process.SetEditorBuildProcessArguments(args);
            return process;
        }
    }
}