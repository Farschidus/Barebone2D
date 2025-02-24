using Barebone.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Barebone.Systems;

public class TextureSystem(TextureComponent textureComponent)
{
    private readonly TextureComponent _texture = textureComponent;
    private readonly Rectangle _textureRectangle = new(textureComponent.Position.X, textureComponent.Position.Y, textureComponent.Texture.Width, textureComponent.Texture.Height);

    public void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(_texture.Texture, _textureRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, _texture.LayerDepth);
}
