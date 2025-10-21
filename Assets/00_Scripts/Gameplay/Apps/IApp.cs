
using System;
using static Unity.Collections.AllocatorManager;

public interface IApp
{
    virtual void Open() { }
    virtual void Close() { }

    public event Action<int> notificationEvent {
        add
        {
            notificationEvent += value;
        }
        remove
        {
            notificationEvent -= value;
        }
    }
}