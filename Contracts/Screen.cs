using Barebone.Core;
using Barebone.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Barebone.Contracts;

/// <summary>
/// A screen is a single layer that has update and draw logic, and which
/// can be combined with other layers to build up a complex menu system.
/// For instance the main menu, the options menu, the "are you sure you
/// want to quit" message box, and the main game itself are all implemented
/// as screens.
/// </summary>
public abstract class Screen(string name)
{
    #region Properties

    /// <summary>
    /// The name of the screen
    /// </summary>
    public string Name = name;
    /// <summary>
    /// There are two possible reasons why a screen might be transitioning
    /// off. It could be temporarily going away to make room for another
    /// screen that is on top of it, or it could be going away for good.
    /// This property indicates whether the screen is exiting for real:
    /// if set, the screen will automatically remove itself as soon as the
    /// transition finishes.
    /// </summary>
    public bool IsExiting { get; protected internal set; }
    /// <summary>
    /// Checks whether this screen is active and can respond to user input.
    /// </summary>
    public bool IsActive { get =>  ScreenState is ScreenState.TransitionOn or ScreenState.Active; }
    /// <summary>
    /// Normally when one screen is brought up over the top of another,
    /// the first screen will transition off to make room for the new
    /// one. This property indicates whether the screen is only a small
    /// popup, in which case screens underneath it do not need to bother
    /// transitioning off.
    /// </summary>
    public bool IsPopup { get; protected init; }
    /// <summary>
    /// Indicates how long the screen takes to
    /// transition on when it is activated.
    /// </summary>
    public TimeSpan TransitionOnTime { get; set; } = TimeSpan.Zero;
    /// <summary>
    /// Indicates how long the screen takes to
    /// transition off when it is deactivated.
    /// </summary>
    public TimeSpan TransitionOffTime { get; set; } = TimeSpan.Zero;
    /// <summary>
    /// Gets the current position of the screen transition, ranging
    /// from zero (fully active, no transition) to one (transitioned
    /// fully off to nothing).
    /// </summary>
    public float TransitionPosition { get; set; } = 1;
    /// <summary>
    /// Gets the current alpha of the screen transition, ranging
    /// from 1 (fully active, no transition) to 0 (transitioned
    /// fully off to nothing).
    /// </summary>
    public float TransitionAlpha { get => 1f - TransitionPosition; }
    /// <summary>
    /// Gets the current screen transition state.
    /// </summary>
    public ScreenState ScreenState { get; set; } = ScreenState.TransitionOn;
    public ScreenManager ScreenManager { get; internal set; }

    protected float _pauseAlpha;
    protected float _elapsedTime;

    #endregion

    #region Methods

    /// <summary>
    /// Load graphics content for the screen.
    /// </summary>
    public virtual void LoadContent(ContentManager content) { }
    /// <summary>
    /// Unload content for the screen.
    /// </summary>
    public virtual void UnloadContent() { }
    /// <summary>
    /// Allows the screen to handle user input. Unlike Update, this method
    /// is only called when the screen is active, and not when some other
    /// screen has taken the focus.
    /// </summary>
    public virtual void HandleInput(InputManager input) { }
    /// <summary>
    /// Allows the screen to run logic, such as updating the transition position.
    /// Unlike HandleInput, this method is called regardless of whether the screen
    /// is active, hidden, or in the middle of a transition.
    /// </summary>
    public virtual void Update(GameTime gameTime) { }
    /// <summary>
    /// This is called when the screen should draw itself.
    /// </summary>
    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

    #endregion
}
