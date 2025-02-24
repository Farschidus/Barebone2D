using Barebone.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Barebone.Managers;

public class CursorManager
{
    private Vector2 _cursorPosition;
    private readonly Dictionary<CursorTextureType, Texture2D> _cursorTextures = [];
    private Texture2D _cursorTexture;
    // TODO: need a way to show the inventory item beside the cursor for "Use" cases
    // public object ActiveInventoryItem { get; set; }

    public CursorManager()
    {
        EventManager.Current.OnCursorTextureChange += CursorTextureChange;
    }
    public void LoadContent(ContentManager content)
    {
        _cursorTextures.Add(CursorTextureType.Pointer, content.Load<Texture2D>(Constants.Cursor_Pointer));
        _cursorTextures.Add(CursorTextureType.Interact, content.Load<Texture2D>(Constants.Cursor_Interact));
        _cursorTextures.Add(CursorTextureType.Talk, content.Load<Texture2D>(Constants.Cursor_Talk));
        _cursorTextures.Add(CursorTextureType.Walk, content.Load<Texture2D>(Constants.Cursor_Walk));
        _cursorTextures.Add(CursorTextureType.Exit, content.Load<Texture2D>(Constants.Cursor_Exit));
        
        _cursorTexture = _cursorTextures[CursorTextureType.Pointer];
    }
    public void UnloadContent()
    {
        EventManager.Current.OnCursorTextureChange -= CursorTextureChange;
        _cursorTextures.Clear();
    }
    public void Update(InputManager input)
    {
        _cursorPosition.Y = input.CurrentMousePoint.Y;
        _cursorPosition.X = input.CurrentMousePoint.X;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_cursorTexture, _cursorPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
    }
    
    private void CursorTextureChange(CursorTextureType type)
    {
        _cursorTextures.TryGetValue(type, out _cursorTexture);
    }
}
