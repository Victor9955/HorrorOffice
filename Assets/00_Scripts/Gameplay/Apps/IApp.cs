
using System;
using static Unity.Collections.AllocatorManager;

public interface IApp
{
    public abstract void Open();
    public abstract void Close();
    public virtual void Notification() { }
}