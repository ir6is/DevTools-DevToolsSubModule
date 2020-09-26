using System.IO;
using UnityDevTools.Common;
using UnityEditor;
using UnityEngine;

public class DataService : MonoBehaviour
{
    [SerializeField]
    private ExampleScriptableData _scriptableData;
    private const string _path = "examplePath";
    public ExampleScriptableData ExampleData
    {
        get
        {
            return _scriptableData;
        }
    }

    private void Awake()
    {
        if (Application.isEditor)
        {
            if (_scriptableData == null)
            {
                _scriptableData = ScriptableDataUtils.TryLoadScriptableFromJSON<ExampleScriptableData>(_path);
            }
        }
        else
        {
            try
            {
                _scriptableData = ScriptableDataUtils.LoadScriptableFromJSON<ExampleScriptableData>(_path);
            }
            catch (FileNotFoundException e)
            {
                Debug.Log($"Saved data for {_scriptableData} not found. {e}");
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        SaveData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void SaveData()
    {
        ScriptableDataUtils.SaveScriptableToJSON(_path, _scriptableData);
    }
}
