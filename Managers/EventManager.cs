using Barebone.Contracts;
using Barebone.Core;
using Barebone.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Barebone.Managers;

public class EventManager
{
    private readonly ScreenManager _screenManager;
    private readonly Dictionary<string, string> _screenGameObjects = [];
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

    public void ExecuteDialogue(string objectName, Screen screen)
    {
        _screenGameObjects[objectName] = NpcState.Talk.ToString();
        screen.ScreenManager.AddScreen(new DialogueScreen(screen.Name));
    }

    public static void ExecuteExit(string nextScene, Screen screen)
    {
        screen.ScreenManager.ExitScreen(screen);
        screen.ScreenManager.AddScreen(GameScreen.InitNextScene(nextScene, screen.ScreenManager.Game.Content));
    }

    public void ExecuteSequence(string objectName, Screen screen)
    {
        _screenGameObjects[objectName] = NpcState.Talk.ToString();
        screen.ScreenManager.AddScreen(new SequenceScreen(objectName));
    }
    

    public Action<CursorTextureType> OnCursorTextureChange;
    public void ChangeCursor(CursorTextureType type) => OnCursorTextureChange?.Invoke(type);

    public Action<int> OnStartDialogue;
    public void StartDialogue(int param) => OnStartDialogue?.Invoke(param);

}