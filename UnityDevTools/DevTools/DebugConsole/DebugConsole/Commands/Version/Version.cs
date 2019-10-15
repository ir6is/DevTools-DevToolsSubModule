using UnityEditor;
using UnityEngine;

namespace UnityDevTools.Console
{
    internal static class Version
    {
        public const string VersionCommand = "version";

        public static void PrintVersion(object sender,string command)
        {
            if (command == VersionCommand)
            {
                Debug.Log("PlayerSettings.bundleVersion:= " + PlayerSettings.bundleVersion);
            }
        }
    }
}