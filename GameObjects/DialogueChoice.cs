using Barebone.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Barebone.GameObjects;

public class DialogueChoice
{
    public string Text { get; set; }
    public int NextNodeId { get; set; }

    private Vector2 _position = new(Constants.DialogueBgOffset * 2, Constants.DialogueBgOffset * 2);

    public int GetWidth(SpriteFont font) => (int)font.MeasureString(Text).X;
    public Rectangle TextRectangle(SpriteFont _spriteFont)
        => new((int)_position.X, (int)_position.Y, GetWidth(_spriteFont), _spriteFont.LineSpacing);
}

public class DialogueData
{
    public List<DialogueNode> Nodes { get; set; }
}

public class DialogueNode
{
    public int Id { get; set; }
    public string Text { get; set; }
    public List<DialogueChoice> Choices { get; set; }
}
