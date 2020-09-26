using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConcreateSimple : IItem
{
    [SerializeField]
    private string m_id;
    [SerializeField]
    private double m_value;
    [SerializeField]
    private bool m_isEquipable;

    public int OwnValue;
    public string Id { get => m_id; set => m_id=value; }
    public double Value { get => m_value; set => m_value=value; }
    public bool IsEquipable => m_isEquipable;

    public override string ToString()
    {
        return $"Id {Id} . Value {Value} . IsEquipable {IsEquipable} . OwnValue {OwnValue} . hash {GetHashCode()} . ";
    }
}
