using Barebone.Core;
using Barebone.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Barebone.Contracts;

public interface INonInteractiveObject
{
    string Name { get; }
    float LayerDepth { get; }

    void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}