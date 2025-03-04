using Barebone.Contracts;
using Barebone.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Barebone.Managers;

public class ScreenManager : DrawableGameComponent
{
    private float _pauseAlpha;
    private bool _isInitialized;
    private bool _gameWindowHasFocus;
    private bool _popupScreenIsActive;
    private Texture2D _backBufferTexture;
    private readonly CursorManager _cursorManager;
    private readonly SpriteBatch _spriteBatch;
    private readonly InputManager _inputManager;
    private readonly List<Screen> _screens = [];
    private readonly List<Screen> _screensToUpdate = [];


    public ScreenManager(Game game) : base(game)
    {
        _gameWindowHasFocus = game.IsActive;
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _inputManager = new InputManager();
        _cursorManager = new CursorManager();
    }
    /// <summary>
    /// Initializes the screen manager component.
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        _isInitialized = true;
    }
    protected override void LoadContent()
    {
        _backBufferTexture = new(GraphicsDevice, 1, 1);
        _backBufferTexture.SetData([Color.White]);

        foreach (var screen in _screens) screen.LoadContent(Game.Content);
        _cursorManager.LoadContent(Game.Content);
    }
    protected override void UnloadContent()
    {
        foreach (var screen in _screens) screen.UnloadContent();
        _cursorManager.UnloadContent();
    }
    private void HandleGameGeneralInput(GameTime gameTime)
    {
        _inputManager.Update(gameTime);
    }
    public override void Update(GameTime gameTime)
    {
        // Read the keyboard, gamepad and Mouse.
        HandleGameGeneralInput(gameTime);

        // Make a copy of the master screen list, to avoid confusion if
        // the process of updating one screen adds or removes others.
        _screensToUpdate.Clear();
        _screensToUpdate.AddRange(_screens);

        _gameWindowHasFocus = Game.IsActive;
        _popupScreenIsActive = false;

        // Loop as long as there are screens waiting to be updated.
        while (_screensToUpdate.Count > 0)
        {
            // Pop the topmost screen off the waiting list.
            var screen = _screensToUpdate[^1];
            _screensToUpdate.RemoveAt(_screensToUpdate.Count - 1);

            // Update the screen.
            UpdateScreenState(screen, gameTime, false);
            UpdateTransitionAlpha();

            // If this is the first active screen we came across,
            // give it a chance to handle InputManager.
            if (!_popupScreenIsActive && _gameWindowHasFocus && screen.IsActive)
            {
                screen.HandleInput(_inputManager);
            }

            _cursorManager.Update(_inputManager);
            screen.Update(gameTime);

            if (screen.ScreenState is not (ScreenState.TransitionOn or ScreenState.Active))
            {
                continue;
            }

            // If this is an active non-popup, inform any subsequent screens that they are covered by it.
            if (screen.IsPopup)
            {
                _popupScreenIsActive = true;
            }
        }
    }
    /// <summary>
    /// Tells each screen to draw itself.
    /// </summary>
    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(SpriteSortMode.FrontToBack,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.AnisotropicWrap,
            transformMatrix: ResolutionManager.ScreenScaleMatrix);

        foreach (var screen in _screens)
        {
            if (screen.ScreenState == ScreenState.Hidden) continue;
          
            if(screen.IsPopup && !screen.NoFadedBackBufferBlack)
            {
                FadeBackBufferToBlack(screen.TransitionAlpha * 2 / 3);
            }
            
            screen.Draw(gameTime, _spriteBatch);

            // if (!screen.IsPopup && screen.TransitionPosition > 0 || _pauseAlpha > 0)
            // {
            //     FadeBackBufferToBlack(MathHelper.Lerp(1f - screen.TransitionAlpha, 1f, _pauseAlpha / 2));
            // }
        }

        _cursorManager.Draw(_spriteBatch);

        _spriteBatch.End();
    }
    /// <summary>
    /// Adds a new screen to the screen manager.
    /// </summary>
    public void AddScreen(Screen screen)
    {
        screen.ScreenManager = this;
        screen.IsExiting = false;

        // If we have a graphics device, tell the screen to load content.
        if (_isInitialized)
        {
            screen.LoadContent(Game.Content);
        }

        _screens.Add(screen);
    }
    /// <summary>
    /// Removes a screen from the screen manager. You should normally
    /// use GameScreen.ExitScreen instead of calling this directly, so
    /// the screen can gradually transition off rather than just being
    /// instantly removed.
    /// </summary>
    public void RemoveScreen(Screen screen)
    {
        // If we have a graphics device, tell the screen to unload content.
        if (_isInitialized)
        {
            screen.UnloadContent();
        }

        _screens.Remove(screen);
        _screensToUpdate.Remove(screen);
    }
    /// <summary>
    /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
    /// instantly kills the screen, this method respects the transition timings
    /// and will give the screen a chance to gradually transition off.
    /// </summary>
    public void ExitScreen(Screen screen)
    {
        if (screen.TransitionOffTime == TimeSpan.Zero)
        {
            // If the screen has a zero transition time, remove it immediately.
            RemoveScreen(screen);
        }
        else
        {
            // Otherwise flag that it should transition off and then exit.
            screen.IsExiting = true;
        }
    }


    /// <summary>
    /// Will update ScreenState during the screen transition
    /// </summary>
    /// <param name="screen"></param>
    /// <param name="gameTime"></param>
    /// <param name="shouldTransitionOff"></param>
    private void UpdateScreenState(Screen screen, GameTime gameTime, bool shouldTransitionOff)
    {
        if (screen.IsExiting)
        {
            // If the screen is going away to die, it should transition off.
            screen.ScreenState = ScreenState.TransitionOff;

            if (!UpdateTransition(gameTime, screen.TransitionOffTime, 1, screen))
            {
                // When the transition finishes, remove the screen.
                RemoveScreen(screen);
            }
        } 
        else if (shouldTransitionOff)
        {
            // If the screen is covered by another, it should transition off.
            if (UpdateTransition(gameTime, screen.TransitionOffTime, 1, screen))
            {
                // Still busy transitioning.
                screen.ScreenState = ScreenState.TransitionOff;
            }
            else
            {
                // Transition finished!
                screen.ScreenState = ScreenState.Hidden;
            }
        }
        else
        {
            // Otherwise the screen should transition on and become active.
            if (UpdateTransition(gameTime, screen.TransitionOnTime, -1, screen))
            {
                // Still busy transitioning.
                screen.ScreenState = ScreenState.TransitionOn;
            }
            else
            {
                // Transition finished!
                screen.ScreenState = ScreenState.Active;
            }
        }
    }
    /// <summary>
    /// Update screen position at the screen transitioning time
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="time"></param>
    /// <param name="direction"></param>
    /// <param name="screen"></param>
    /// <returns></returns>
    private static bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction, Screen screen)
    {
        // How much should we move by?
        float transitionDelta;

        if (time == TimeSpan.Zero)
            transitionDelta = 1;
        else
            transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);

        // Update the transition position.
        screen.TransitionPosition += transitionDelta * direction;

        // Did we reach the end of the transition?
        if (((direction < 0) && (screen.TransitionPosition <= 0)) ||
            ((direction > 0) && (screen.TransitionPosition >= 1)))
        {
            screen.TransitionPosition = MathHelper.Clamp(screen.TransitionPosition, 0, 1);
            return false;
        }

        // Otherwise we are still busy transitioning.
        return true;
    }
    private void UpdateTransitionAlpha()
    {
        // Gradually fade in or out depending on whether we are covered by the pause screen.
        _pauseAlpha = _popupScreenIsActive 
            ? Math.Min(_pauseAlpha + 1f / 32, 1) 
            : Math.Max(_pauseAlpha - 1f / 32, 0);
    }
    private void FadeBackBufferToBlack(float alpha)
    {
        _spriteBatch.Draw(
            _backBufferTexture, 
            ResolutionManager.ScreenRectangle,
            null,
            Color.Black * alpha,
            0,
            Vector2.Zero,
            SpriteEffects.None,
            0.95f
        );
    }
}