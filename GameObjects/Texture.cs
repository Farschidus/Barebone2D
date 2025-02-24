using Barebone.Components;
using Barebone.Contracts;
using Barebone.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Barebone.GameObjects;

public class Texture : GameObject
{
    private readonly TextureSystem _textureSystem;

    public Texture(TextureComponent component) : base(component.Name, component.LayerDepth)
    {
        _textureSystem = new TextureSystem(component);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _textureSystem.Draw(spriteBatch);
    }
}
