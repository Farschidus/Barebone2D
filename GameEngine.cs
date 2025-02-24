using System;
using Barebone.Core;
using Barebone.Managers;
using Barebone.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Barebone;

public class GameEngine : Game
{
    private readonly ScreenManager _screenManager;
    private bool _isResizing;

    public GameEngine()
    {
        var graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = Constants.ScreenWidth,
            PreferredBackBufferHeight = Constants.ScreenHeight,
        };  
        graphics.ApplyChanges();

        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnClientSizeChanged;

        _screenManager = new ScreenManager(this);
    }
    protected override void Initialize()
    {
        ResolutionManager.Init(GraphicsDevice, Constants.ScreenWidth, Constants.ScreenHeight);
        Components.Add(_screenManager);
        
        base.Initialize();
    }
    protected override void LoadContent()
    {
        _screenManager.AddScreen(GameScreen.InitNextScene("Bakery", _screenManager.Game.Content));
    }
    protected override void UnloadContent()
    {
        Components.Remove(_screenManager);
    }
    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Viewport = ResolutionManager.Viewport;
        base.Draw(gameTime);
    }

    private void OnClientSizeChanged(object sender, EventArgs e)
    {
        if (_isResizing || Window.ClientBounds.Width <= 0 || Window.ClientBounds.Height <= 0) return;
        
        _isResizing = true;
        ResolutionManager.UpdateScreenScaleMatrix();
        _isResizing = false;
    }
}
