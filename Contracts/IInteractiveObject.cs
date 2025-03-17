using Barebone.Core;
using Barebone.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Barebone.Contracts;

public interface IInteractiveObject
{
    string Name { get; }
    float LayerDepth { get; }
    GameObjectType Type { get; }

    void UpdateState(string state);
    void HandleInput(InputManager input);
    void Update(GameTime gameTime);
    void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}