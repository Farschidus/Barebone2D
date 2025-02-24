using Microsoft.Xna.Framework;

namespace Barebone.Managers;

public static class InteractionManager
{
    // public static InteractionManager Instance { get; } = new();
    // private InteractionManager() { }
    public static void Interact(string objectName, Rectangle gameObjectRect, InputManager input)
    {
        if(input.IsHover(gameObjectRect))
        {
            if(input.IsMouseClicked)
            {
                EventManager.Current.Execute(objectName, "Talk");
            }

            EventManager.Current.ChangeCursor(Core.CursorTextureType.Talk);
        }
        else
        {
            EventManager.Current.ChangeCursor(Core.CursorTextureType.Pointer);
        }
    }
}