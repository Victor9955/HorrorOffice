using System;
using UnityEngine;
public interface IDropContainer
{
    bool IsOpen();
    public bool Drop<T>(T dropped) where T : Draggable;
}

