using System;
using UnityEngine;
public interface IDropContainer
{
    bool CanReceive();
    public bool Drop<T>(T dropped) where T : Draggable;
}

