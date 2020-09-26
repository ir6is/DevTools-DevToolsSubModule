using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UnityDevTools.Common
{
    public static class ScriptableDataUtils
    {
        public static void SaveScriptableToJSON(string localPath, Object data)
        {
            localPath = Path.Combine(Application.persistentDataPath, localPath);
            Debug.LogFormat("Saving game settings to {0}", data);
            File.WriteAllText(localPath, JsonUtility.ToJson(data, true));

#if UNITY_EDITOR
            EditorUtility.SetDirty(data);
#endif
        }

        public static T TryLoadScriptableFromJSON<T>(string localPath) where T : ScriptableObject
        {
            localPath = Path.Combine(Application.persistentDataPath, localPath);

            if (File.Exists(localPath))
            {
                return LoadScriptableFromJSON<T>(localPath);
            }
            else
            {
                return ScriptableObject.CreateInstance<T>();
            }
        }

        public static T LoadScriptableFromJSON<T>(string localPath) where T : ScriptableObject
        {
            localPath = Path.Combine(Application.persistentDataPath, localPath);

            if (File.Exists(localPath))
            {
                var instance = ScriptableObject.CreateInstance<T>();
                JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(localPath), instance);
                return instance;
            }
            else
            {
                throw new FileNotFoundException($"Can't laod file by path {localPath}");
            }
        }
    }
}
