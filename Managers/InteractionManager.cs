using Barebone.Contracts;
using Barebone.Core;
using Microsoft.Xna.Framework;

namespace Barebone.Managers;

public static class InteractionManager
{
    public static void Interact(IInteractiveObject interactiveObject, Rectangle gameObjectRect, InputManager input, Screen screen)
    {
        if(input.IsHover(gameObjectRect))
        {
            if(input.IsMouseClicked)
            {

                ObjectClickIntraction(interactiveObject, screen);
            }
            else
            {
                ObjectHoverIntraction(interactiveObject);
            }
        }
        else
        {
            EventManager.Current.ChangeCursor(CursorTextureType.Pointer);
        }
    }

    private static void ObjectHoverIntraction(IInteractiveObject interactiveObject)
    {
        switch (interactiveObject.Type)
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
    private static void ObjectClickIntraction(IInteractiveObject interactiveObject, Screen screen)
    {
        switch (interactiveObject.Type)
        {
            case GameObjectType.Npc:
                EventManager.ExecuteDialogue(interactiveObject, screen);
                break;
            case GameObjectType.Item:
                EventManager.Current.Execute(interactiveObject.Name, CursorTextureType.Interact.ToString());
                break;
            case GameObjectType.Exit:
                EventManager.ExecuteExit(interactiveObject.Name, screen);
                break;
            default:
                break;
        }

        // revert to default cursor at the end of click interaction
        EventManager.Current.ChangeCursor(CursorTextureType.Pointer);
    }
}