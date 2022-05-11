using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MultiplayerBuild
{
#if UNITY_EDITOR

    [MenuItem("Tools/Run Multiplayer/2 Players")]
    static void PerformSiliconBuild2()
    {
        PerformSiliconBuild(2);
    }

    [MenuItem("Tools/Run Multiplayer/3 Players")]
    static void PerformSiliconBuild3()
    {
        PerformSiliconBuild(3);
    }

    [MenuItem("Tools/Run Multiplayer/4 Players")]
    static void PerformSiliconBuild4()
    {
        PerformSiliconBuild(4);
    }

    static void PerformSiliconBuild(int count)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);

        for (int i = 0; i < count; i++)
        {
            BuildPipeline.BuildPlayer(
                GetScenePaths(),
                "Builds/OSX/" + GetProjectName() + i + "/" + GetProjectName() + i,
                BuildTarget.StandaloneOSX,
                BuildOptions.AutoRunPlayer
            );
        }
    }

    static string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        return s[s.Length - 2];
    }

    static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        return scenes;
    }

#endif
}