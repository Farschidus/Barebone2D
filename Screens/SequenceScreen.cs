using Barebone.Contracts;
using Barebone.Core;
using Barebone.GameObjects;
using Barebone.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Barebone.Screens;

public class SequenceScreen : Screen
{
    private SpriteFont _spriteFont;
    private Texture2D _dialogueBgTexture;
    private int _dialogueHeight;
    private int _selectedChoice;
    private Dictionary<int, SequenceNode> _dialogues = [];
    private List<DialogueOption> _choices = [];
    private SequenceNode _currentNode;
    //TODO: implement hover on choice
    //private bool _isHoverOnChoice;

    public SequenceScreen(string objectName) : base($"Dialogues/{objectName}")
    {
        IsPopup = true;
        NoFadedBackBufferBlack = true;
    }

    public override void LoadContent(ContentManager content)
    {
        _spriteFont = content.Load<SpriteFont>("Fonts/Tahoma");
        _dialogueBgTexture = new Texture2D(ScreenManager.GraphicsDevice, 1, 1);
        _dialogueBgTexture.SetData([Color.White]);

        _dialogues = LoadDialogue(Name);
        NextDialogue(1);
    }
    public override void UnloadContent()
    {
        _dialogues.Clear();
        _currentNode = null;
        _spriteFont = null;
        _dialogueBgTexture = null;
        //EventManager.Current.OnStartDialogue -= StartDialogue;
    }
    public override void HandleInput(InputManager input)
    {
        if (input.IsMoveDownPressed)
        {
            _selectedChoice++;
            if (_selectedChoice >= _choices.Count) _selectedChoice--;
        }
        if (input.IsMoveUpPressed)
        {
            _selectedChoice--;
            if (_selectedChoice < 0) _selectedChoice = 0;
        }
        if (input.IsSelectPressed)
        {
            if (_choices.Count > 0)
                NextDialogue(_choices[_selectedChoice].NextNodeId);
            else
                ScreenManager.ExitScreen(this);
        }

        // for (int i = 0; i < _choices.Count; i++)
        // {
        //     if (_choices[i].TextRectangle(_spriteFont).Contains(input.CurrentMousePoint))
        //     {
        //         _selectedChoice = i;
        //         _isHoverOnChoice = true;
        //     }
        //     else
        //     {
        //         _isHoverOnChoice = false;
        //     }
        // }

        // if(_isHoverOnChoice && input.IsMouseClicked)
        // {
        //     _choices[_selectedChoice].OnSelectChoice();
        // }
    }
    public override void Update(GameTime gameTime)
    {
        if (_currentNode.Dialogue is null || _spriteFont is null) return;

        _dialogueHeight = (_choices.Count == 0 ? 1 : _choices.Count) * _spriteFont.LineSpacing;
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {   
        if(_currentNode.Dialogue is null) return;

        spriteBatch.Draw(
                _dialogueBgTexture,
                new Rectangle(0, 0, ResolutionManager.ScreenRectangle.Width, _dialogueHeight),
                null,
                Color.Black * 0.7f,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                0.98f
            );

            spriteBatch.DrawString(
                _spriteFont,
                _currentNode.Dialogue.Text,
                new Vector2(Constants.DialogueBgOffset*2, Constants.DialogueBgOffset*2),
                Color.White,
                0,
                Vector2.Zero,
                .5f,
                SpriteEffects.None,
                0.99f
            );

            for(int i = 0; i < _choices.Count; i++)
            {
                spriteBatch.DrawString(
                    _spriteFont,
                    _choices[i].Text,
                    Vector2.Add(new Vector2(Constants.DialogueBgOffset, Constants.DialogueBgOffset), new Vector2(30, _spriteFont.LineSpacing/2 * (i+2))),
                    i == _selectedChoice ? Color.Yellow : Color.White,
                    0,
                    new Vector2(0, _spriteFont.LineSpacing / 2),
                    .5f,
                    SpriteEffects.None, 
                    0.99f
                );
            }
    }
    
    private void NextDialogue(int nextNodeId)
    {
        _currentNode = _dialogues[nextNodeId];
        _choices = _currentNode.Dialogue?.Choices ?? [];
        _selectedChoice = 0;
    }
    private static Dictionary<int, SequenceNode> LoadDialogue(string objectName)
    {
        var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", objectName + ".json");
        var dialogueJson = File.ReadAllText(jsonFilePath);
        var dialogueData = JsonSerializer.Deserialize<SequenceData>(dialogueJson, jsonSerializerOptions);
        return dialogueData?.Nodes.ToDictionary(node => node.Id) ?? [];
    }
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
}