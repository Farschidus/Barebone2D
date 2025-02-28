using System;
using Barebone.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Barebone.Managers;

public class InputManager
{
    public bool IsMouseClicked
    {
        get
        {
            if (_currentMouseStates.LeftButton == ButtonState.Pressed && _lastMouseStates.LeftButton == ButtonState.Released)
            {
                mouseClickedPoint = CurrentMousePoint;
                return true;
            }

            return false;
        }
    }
    public bool IsDoubleClicked { get; private set; }
    public Point CurrentMousePoint { get; private set; }
    public Point MouseClickedPoint { get => mouseClickedPoint; }
    /// <summary>
    /// Checks if the Escape key was newly pressed during this update.
    /// This method returns true only if the Escape key was pressed in the current frame
    /// and was not pressed in the previous frame.
    /// </summary>
    /// <returns></returns>
    public bool IsGamePaused { get => IsNewKeyPress(Keys.Escape); }
    public bool IsMoveUpPressed { get => IsNewKeyPress(Keys.Up) || IsNewKeyPress(Keys.W); }
    public bool IsMoveDownPressed { get => IsNewKeyPress(Keys.Down) || IsNewKeyPress(Keys.S); }
    public bool IsSelectPressed { get => IsNewKeyPress(Keys.Space) || IsNewKeyPress(Keys.Enter); }

    private double timePassed;
    private double previousGameTime;
    private const double TimerDelay = Constants.DoubleClickSpeed; 
    private MouseState _lastMouseStates;
    private MouseState _currentMouseStates;
    private Point mouseClickedPoint = Point.Zero;
    private Vector2 mouseStateVector;
    private KeyboardState _currentKeyboardStates;
    private KeyboardState _lastKeyboardStates;

    public void Update(GameTime gameTime)
    {
        _lastKeyboardStates = _currentKeyboardStates;
        _currentKeyboardStates = Keyboard.GetState();

        _lastMouseStates = _currentMouseStates;
        _currentMouseStates = Mouse.GetState();

        CurrentMousePoint = GetCurrentMousePoint();

        CheckMouseClick(gameTime);
    }
    /// <summary>
    /// Helper for checking if a key was newly pressed during this update.
    /// it will accept input from player one.
    /// </summary>
    public bool IsHover(Rectangle rect) => rect.Contains(CurrentMousePoint);

    private bool IsNewKeyPress(Keys key) => _currentKeyboardStates.IsKeyDown(key) && _lastKeyboardStates.IsKeyUp(key);
    private Point GetCurrentMousePoint()
    {
        mouseStateVector = new Vector2(_currentMouseStates.X - ResolutionManager.Viewport.X, _currentMouseStates.Y - ResolutionManager.Viewport.Y);
        mouseStateVector = Vector2.Transform(mouseStateVector, Matrix.Invert(ResolutionManager.ScreenScaleMatrix));
        return new Point((int)Math.Round(mouseStateVector.X), (int)Math.Round(mouseStateVector.Y));
    }
    private void CheckMouseClick(GameTime gameTime)
    {
        timePassed = gameTime.TotalGameTime.Milliseconds - previousGameTime;
        IsDoubleClicked = timePassed > 0 && timePassed < TimerDelay &&
            _currentMouseStates.LeftButton == ButtonState.Released && _lastMouseStates.LeftButton == ButtonState.Pressed;
        previousGameTime = gameTime.TotalGameTime.Milliseconds;
    }
}