using Barebone.Core;
using Barebone.Contracts;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Barebone.Components;

public class AnimationComponent : BaseComponent
{
    public bool IsLooping { get; private set; }
    public AnimationSprite AnimationSprite { get; private set; }
    public TextureSpriteFile[] TextureSpriteFiles { get; private set; }

    public AnimationComponent(AnimationSprite animationSprite, float layerDepth, ContentManager content) 
        : base(animationSprite.Name.ToString(), layerDepth)
    {
        IsLooping = !animationSprite.LoopCount.Equals(0);
        AnimationSprite = animationSprite;
        TextureSpriteFiles = Enumerable.Range(animationSprite.FileRange.StartNumber, animationSprite.FileRange.EndNumber - animationSprite.FileRange.StartNumber + 1)
            .Select(i => new TextureSpriteFile
            {
                Texture = content.Load<Texture2D>($"{animationSprite.SpriteFile.Texture}{i:00}"),
                Width = animationSprite.SpriteFile.Width,
                Height = animationSprite.SpriteFile.Height
            })
            .ToArray();
    }
}