using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeComponent : SerializedMonoBehaviour, ISomeInterface
{
    public ISomeInterface A;

    public ISomeInterface B;

    public ISomeInterface[] C;

    public ISomeInterface D;
}

public interface ISomeInterface { }

public interface ISomeGenericInterface : ISomeInterface { }

public class SomeClass1 : ISomeInterface
{
    public int Test;
}

public class SomeClass2 : ISomeGenericInterface
{
    public GameObject Test1;
}

public class SomeClass3 : SomeClass2
{
    public GameObject Test2;
}