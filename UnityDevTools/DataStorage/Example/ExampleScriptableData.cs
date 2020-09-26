using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ExampleScriptableData : SerializedScriptableObject // if dont need serialize Interface use ScriptableObject
{
    public List<IItem>  SomeComponents;
}