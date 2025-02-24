using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Barebone.Core;

public class GameAssets
{
    public List<Scene> Scenes;
}

public class Scene
{
    public string Name;
    public Texture Background;
    public List<Npc> Npcs;
    public List<Texture> Foregrounds;
}

public class Npc
{
    public string Name;
    public float LayerDepth;
    public List<AnimationSprite> AnimationSprites;
}

public class Texture
{
    public string Name;
    public string SpriteTexture;
    public Point Position;
    public float LayerDepth;
}