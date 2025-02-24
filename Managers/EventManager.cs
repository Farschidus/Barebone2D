using Barebone.Core;
using System;
using System.Collections.Generic;

namespace Barebone.Managers;

public class EventManager
{
    private Dictionary<string, string> _screenGameObjects = [];
    public static EventManager Current { get; } = new();

    private EventManager() { }

    public bool GetState(string objectName, out string state)
    {
        return _screenGameObjects.TryGetValue(objectName, out state);
    }

    public void SetState(string objectName, string state)
    {
        _screenGameObjects[objectName] = state;
    }

    public void Execute(string objectName, string state)
    {
        _screenGameObjects[objectName] = state;
    }

    public Action<CursorTextureType> OnCursorTextureChange;
    public void ChangeCursor(CursorTextureType type) => OnCursorTextureChange?.Invoke(type);

}