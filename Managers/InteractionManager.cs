using Barebone.Contracts;
using Barebone.Core;
using Microsoft.Xna.Framework;

namespace Barebone.Managers;

public static class InteractionManager
{
    public static void Interact(GameObjectType type, string name, Rectangle gameObjectRect, InputManager input, Screen screen)
    {
        if(input.IsHover(gameObjectRect))
        {
            if(input.IsMouseClicked)
            {

                ObjectClickIntraction(type, name, screen);
            }
            else
            {
                ObjectHoverIntraction(type);
            }
        }
        else
        {
            EventManager.Current.ChangeCursor(CursorTextureType.Pointer);
        }
    }

    private static void ObjectHoverIntraction(GameObjectType type)
    {
        switch (type)
        {
            case GameObjectType.Npc:
                EventManager.Current.ChangeCursor(CursorTextureType.Talk);
                break;
            case GameObjectType.Item:
                EventManager.Current.ChangeCursor(CursorTextureType.Interact);
                break;
            case GameObjectType.Exit:
                EventManager.Current.ChangeCursor(CursorTextureType.Exit);
                break;
            default:
                EventManager.Current.ChangeCursor(CursorTextureType.Pointer);
                break;
        }
    }
    private static void ObjectClickIntraction(GameObjectType type, string objectName, Screen screen)
    {
        switch (type)
        {
            case GameObjectType.Npc:
                EventManager.Current.ExecuteSequence(objectName, screen);
                break;
            case GameObjectType.Item:
                EventManager.Current.Execute(objectName, CursorTextureType.Interact.ToString());
                break;
            case GameObjectType.Exit:
                EventManager.ExecuteExit(objectName, screen);
                break;
            default:
                break;
        }

        // revert to default cursor at the end of click interaction
        EventManager.Current.ChangeCursor(CursorTextureType.Pointer);
    }
}