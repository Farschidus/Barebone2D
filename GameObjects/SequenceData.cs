using Barebone.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Barebone.GameObjects;

public class SequenceData
{
    public List<SequenceNode> Nodes { get; set; }
}

public class SequenceNode
{
    public int Id { get; set; }
    public string Actor { get; set; }
    public string State { get; set; }
    public Vector2? Destination { get; set; }
    public Dialogue Dialogue { get; set; }
}

public class Dialogue
{
    public string VoiceOver { get; set; }
    public string Text { get; set; }
    public List<DialogueOption> Choices { get; set; }
}


public class DialogueOption
{
    public string Text { get; set; }
    public int NextNodeId { get; set; }

    private Vector2 _position = new(Constants.DialogueBgOffset * 2, Constants.DialogueBgOffset * 2);

    public int GetWidth(SpriteFont font) => (int)font.MeasureString(Text).X;
    public Rectangle TextRectangle(SpriteFont _spriteFont)
        => new((int)_position.X, (int)_position.Y, GetWidth(_spriteFont), _spriteFont.LineSpacing);
}
