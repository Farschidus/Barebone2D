using Barebone.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Barebone.Contracts;

public abstract class GameObject(string name, float layerDepth)
{
    public string Name { get; private set; } = name;
    public float LayerDepth { get; private set; } = layerDepth;

    public virtual void HandleInput(InputManager input) { }
    public virtual void Update(GameTime gameTime) { }
    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
}