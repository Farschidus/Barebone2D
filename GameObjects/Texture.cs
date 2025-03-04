using Barebone.Components;
using Barebone.Contracts;
using Barebone.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Barebone.GameObjects;

public class Texture : INonInteractiveObject
{
    public string Name { get; }
    public float LayerDepth { get; }

    private readonly TextureSystem _textureSystem;

    public Texture(TextureComponent component)
    {
        _textureSystem = new TextureSystem(component);
        Name = component.Name;
        LayerDepth = component.LayerDepth;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _textureSystem.Draw(spriteBatch);
    }
}
