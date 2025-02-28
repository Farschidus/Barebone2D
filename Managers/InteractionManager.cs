using Barebone.Contracts;
using Barebone.Screens;
using Microsoft.Xna.Framework;

namespace Barebone.Managers;

public static class InteractionManager
{
    public static void Interact(string objectName, Rectangle gameObjectRect, InputManager input, Screen screen)
    {
        if(input.IsHover(gameObjectRect))
        {
            if(input.IsMouseClicked)
            {
                EventManager.Current.Execute(objectName, "Talk");
                screen.ScreenManager.AddScreen(new DialogueScreen(screen.Name));
                EventManager.Current.ChangeCursor(Core.CursorTextureType.Pointer);
            }
            else
            {
                EventManager.Current.ChangeCursor(Core.CursorTextureType.Talk);
            }
        }
        else
        {
            EventManager.Current.ChangeCursor(Core.CursorTextureType.Pointer);
        }
    }
}