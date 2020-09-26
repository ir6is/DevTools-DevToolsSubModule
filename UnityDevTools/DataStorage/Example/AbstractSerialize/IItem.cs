using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    string Id { get; set; }
    double Value { get; set; }
    bool IsEquipable { get; }
}
