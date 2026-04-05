using UnityEngine;
using System;

public interface IGridHexObject : IHideable
{
    public GridHex CurrentHex { get; }
    public event Action OnDataChanged;
}
