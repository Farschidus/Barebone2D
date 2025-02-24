using Microsoft.Xna.Framework;

namespace Barebone.Components;

public class InteractiveComponent(Rectangle interactiveArea)
{
    public Rectangle InteractiveArea { get; private set; } = interactiveArea;
}