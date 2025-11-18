using System;
using UnityEngine;
public interface IDropContainer
{
    public bool IsUnlocked();
    public void UpdateOpenState(bool isOpen, bool isHovered = false);
    public bool Drop<T>(T dropped) where T : Draggable;
}

