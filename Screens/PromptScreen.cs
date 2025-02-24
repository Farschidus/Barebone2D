using System;
using Barebone.Components;
using Barebone.Contracts;
using Barebone.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Barebone.Screens;

public class PromptScreen : Screen
{
    public PromptScreen(string name) : base(name)
    {
        IsPopup = true;
        TransitionOnTime = TimeSpan.FromSeconds(0.2);
        TransitionOffTime = TimeSpan.FromSeconds(0.2);
    }
    private GameObjects.Texture _background;

    public override void LoadContent(ContentManager content)
    {
        _background = new GameObjects.Texture(
            new TextureComponent($"{Name} Bg", "Foregrounds/Bakery-CashRegister", new Point(0, 0), 0.99f, content)
        );
    }

    public override void HandleInput(InputManager input)
    {
        if (input.IsGamePaused)
        {
            ScreenManager.ExitScreen(this);
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _background.Draw(gameTime, spriteBatch);
    }
}