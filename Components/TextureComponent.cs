using Barebone.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Barebone.Components;

public class TextureComponent(string name, string texture, Point position, float layerDepth, ContentManager content)
    : BaseComponent(name, layerDepth)
{
    public Texture2D Texture { get; } = content.Load<Texture2D>(texture);
    public Point Position { get; } = position;
}
