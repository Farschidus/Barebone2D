using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Barebone.Core;

public class AnimationSprite
{
    public NpcState Name;
    public float Fps;
    public float Delay;
    public int LoopCount;
    public int[] RowFrameCount;
    public bool IsMoving;
    public float MoveSpeed;
    public Vector2 Destination;
    public Rectangle Rectangle;
    public SpriteFile SpriteFile;
    public FileRange FileRange;
    public AnimationFileType FileType;
    public AnimationType AnimType;
}

public enum AnimationType
{
    Linear,
    Random
}

public enum AnimationFileType
{
    Joined,
    Seprate
}

public struct FileRange
{
    public int StartNumber { get; set; }
    public int EndNumber { get; set; }
}

public struct SpriteFile
{
    public string Texture { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class TextureSpriteFile
{
    public Texture2D Texture { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}