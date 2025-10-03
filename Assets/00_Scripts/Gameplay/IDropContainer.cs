using System;
using UnityEngine;
public interface IDropContainer
{
    public bool Drop<T>(T dropped) where T : Draggable;
}

