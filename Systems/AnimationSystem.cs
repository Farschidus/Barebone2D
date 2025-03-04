using Barebone.Components;
using Barebone.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Barebone.Systems;
/// <summary>
/// Controls playback of an Animation.
/// </summary>
public class AnimationSystem
{
    #region Properties

    /// <summary>
    /// Gets the Animation which is currently playing.
    /// </summary>
    private AnimationComponent _animationComponent;
    /// <summary>
    /// The zero base number of the current frame in the spritesheet row.
    /// </summary>
    private int _frameIndex;
    private Vector2 _origin;
    private bool _animationEnded;
    private TextureSpriteFile _textureSpriteFile;
    /// <summary>
    /// The amount of time in seconds that the current frame has been shown for.
    /// </summary>
    private float _timePerFrame;
    /// <summary>
    /// The amount of time in seconds that the current frame has been shown for.
    /// </summary>
    private float _totalElapsed;
    /// <summary>
    /// The amount of time in seconds that the player must wait for showing the next random Animation.
    /// </summary>
    private float _delayTime;
    /// <summary>
    /// The zero base random number of the rows in the current spritesheet.
    /// </summary>
    private int _randomRow;
    /// <summary>
    /// The zero base number of the current rows in the spritesheet.
    /// </summary>
    private int _rowIndex;
    /// <summary>
    /// Says that it time to move to the next row in spritesheet.
    /// </summary>
    private bool _isNewRow;
    /// <summary>
    /// Says that the new frame position set in Animation.
    /// </summary>
    private bool _isFrameSet;
    /// <summary>
    /// counting the number of loops that Animation is played
    /// </summary>
    private int _loopCounter;
    /// <summary>
    /// The agreed number to indicate infinit loop for the running animation
    /// </summary>
    private const int INFINIT_LOOP = -1;

    #endregion

    #region Methods

    /// <summary>
    /// Begins or continues playback of an Animation.
    /// </summary>
    public void Play(AnimationComponent animation)
    {
        // If this Animation is already running, do not restart it.
        if (_animationComponent == animation)
            return;

        // Reset for the new Animation.
        _animationComponent = animation;
        _timePerFrame = 1 / animation.AnimationSprite.Fps;
        _delayTime = animation.AnimationSprite.Delay;
        _textureSpriteFile = animation.TextureSpriteFiles[0];
        _origin = Vector2.Zero;
        _rowIndex = 0;
        _randomRow = 0;
        _frameIndex = 0;
        _loopCounter = 0;
        _totalElapsed = 0;
        _isFrameSet = false;
        _animationEnded = false;
    }
    public void SetFrame(int frameIndex)
    {
        _isFrameSet = true;
        _frameIndex = frameIndex;
    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects spriteEffects, Rectangle? destRectangle = null)
    {
        if (_animationComponent == null || _animationComponent.AnimationSprite.RowFrameCount == null)
            throw new NotSupportedException("No Animation is currently playing or Animation is not initialized properly");

        Rectangle destinationRectangle = destRectangle ?? _animationComponent.AnimationSprite.Rectangle;

        switch (_animationComponent.AnimationSprite.FileType)
        {
            case AnimationFileType.Joined:
            {
                if (_animationComponent.AnimationSprite.AnimType.Equals(AnimationType.Linear))
                    DrawSingleLinear(gameTime, spriteBatch, destinationRectangle, spriteEffects);
                else
                    DrawSingleRandom(gameTime, spriteBatch, destinationRectangle, spriteEffects);
                break;
            }
            case AnimationFileType.Seprate:
            {
                if (_animationComponent.AnimationSprite.AnimType.Equals(AnimationType.Linear))
                    DrawSeprateLinear(gameTime, spriteBatch, destinationRectangle, spriteEffects);
                else
                    DrawSeperateRandom(gameTime, spriteBatch, destinationRectangle, spriteEffects);
                return;
            }
        }
    }

    /// <summary>
    /// Advances the time position and draws the correct sprite file of the Animation.
    /// </summary>
    private void DrawSingleLinear(GameTime gameTime, SpriteBatch spriteBatch, Rectangle destRectangle, SpriteEffects spriteEffects)
    {
        if (!_animationEnded)
        {
            _delayTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_delayTime > _animationComponent.AnimationSprite.Delay)
            {
                _totalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_totalElapsed > _timePerFrame)
                {
                    _totalElapsed -= _timePerFrame;

                    if (_animationComponent.AnimationSprite.RowFrameCount[_rowIndex].Equals(_frameIndex + 1))
                        _isNewRow = true;
                    else
                        _isNewRow = false;

                    if (_animationComponent.IsLooping)
                    {                            
                        if (_animationComponent.AnimationSprite.LoopCount.Equals(INFINIT_LOOP))
                        {
                            _frameIndex = (_frameIndex + 1) % _animationComponent.AnimationSprite.RowFrameCount[_rowIndex];
                        }
                        else
                        {
                            if (_loopCounter.Equals(_animationComponent.AnimationSprite.LoopCount))
                            {
                                _animationEnded = true;
                            }
                            else
                            {
                                _frameIndex = (_frameIndex + 1) % _animationComponent.AnimationSprite.RowFrameCount[_rowIndex];
                                if (_animationComponent.AnimationSprite.RowFrameCount[_rowIndex].Equals(_frameIndex + 1) && _animationComponent.AnimationSprite.RowFrameCount.Length.Equals(_rowIndex + 1))
                                    _loopCounter++;
                            }
                        }
                    }
                    else
                    {
                        if (!_isFrameSet)
                        {
                            _frameIndex = Math.Min(_frameIndex + 1, _animationComponent.AnimationSprite.RowFrameCount[_rowIndex] - 1);
                            if (_frameIndex == _animationComponent.AnimationSprite.RowFrameCount[_rowIndex] - 1)
                                _animationEnded = true;
                        }
                    }
                    if (_isNewRow && !_animationEnded)
                    {
                        if (_animationComponent.AnimationSprite.RowFrameCount.Length.Equals(_rowIndex + 1))
                            _rowIndex = 0;
                        else
                            _rowIndex++;
                    }
                    if (_animationComponent.AnimationSprite.RowFrameCount[_rowIndex].Equals(_frameIndex + 1) && _animationComponent.AnimationSprite.RowFrameCount.Length.Equals(_rowIndex + 1))
                        _delayTime = 0;
                }
            }
        }

        // Calculate the source rectangle of the current frame.
        var source = new Rectangle(_frameIndex * _textureSpriteFile.Width, _rowIndex * _textureSpriteFile.Height, _textureSpriteFile.Width, _textureSpriteFile.Height);
        spriteBatch.Draw(_textureSpriteFile.Texture, destRectangle, source, Color.White, 0, _origin, spriteEffects, _animationComponent.LayerDepth);
    }
    private void DrawSeprateLinear(GameTime gameTime, SpriteBatch spriteBatch, Rectangle destRectangle, SpriteEffects spriteEffects)
    {
        if (!_animationEnded)
        {
            _delayTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_delayTime > _animationComponent.AnimationSprite.Delay)
            {
                _totalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_totalElapsed > _timePerFrame)
                {
                    _totalElapsed -= _timePerFrame;

                    if (_animationComponent.IsLooping)
                    {
                        if (_animationComponent.AnimationSprite.LoopCount.Equals(INFINIT_LOOP))
                        {
                            _frameIndex = (_frameIndex + 1) % _animationComponent.TextureSpriteFiles.Length;
                        }
                        else
                        {
                            if (_loopCounter.Equals(_animationComponent.AnimationSprite.LoopCount))
                            {
                                _animationEnded = true;
                            }
                            else
                            {
                                _frameIndex = (_frameIndex + 1) % _animationComponent.TextureSpriteFiles.Length;
                                if (_animationComponent.TextureSpriteFiles.Length.Equals(_frameIndex + 1))
                                    _loopCounter++;
                            }
                        }
                    }
                    else
                    {
                        _frameIndex = Math.Min(_frameIndex + 1, _animationComponent.TextureSpriteFiles.Length - 1);
                        if (_frameIndex == _animationComponent.TextureSpriteFiles.Length - 1)
                            _animationEnded = true;
                    }
                    _textureSpriteFile = _animationComponent.TextureSpriteFiles[_frameIndex];
                    if (_animationComponent.TextureSpriteFiles.Length.Equals(_frameIndex + 1))
                        _delayTime = 0;
                }
            }
        }
        spriteBatch.Draw(_textureSpriteFile.Texture, destRectangle, null, Color.White, 0, _origin, spriteEffects, _animationComponent.LayerDepth);
    }
    /// <summary>
    /// Advances the time position and draws the correct sprite file of the Animation.
    /// </summary>
    private void DrawSingleRandom(GameTime gameTime, SpriteBatch spriteBatch, Rectangle destRectangle, SpriteEffects spriteEffects)
    {
        _delayTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_delayTime > _animationComponent.AnimationSprite.Delay)
        {
            _totalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_totalElapsed > _timePerFrame)
            {
                _totalElapsed -= _timePerFrame;

                _frameIndex = Math.Min(_frameIndex + 1, _animationComponent.AnimationSprite.RowFrameCount[_randomRow] - 1);
                if (_frameIndex == _animationComponent.AnimationSprite.RowFrameCount[_randomRow] - 1)
                {
                    _delayTime = 0;
                    _animationEnded = true;
                    _frameIndex = 0;
                    if (_randomRow < _animationComponent.AnimationSprite.RowFrameCount.Length - 1)
                        _randomRow += 1;
                    else
                        _randomRow = 0;
                }
            }
        }
        // Calculate the source rectangle of the current frame.
        Rectangle source = new Rectangle(_frameIndex * _textureSpriteFile.Width, _randomRow * _textureSpriteFile.Height, _textureSpriteFile.Width, _textureSpriteFile.Height);
        spriteBatch.Draw(_textureSpriteFile.Texture, destRectangle, source, Color.White, 0, _origin, spriteEffects, _animationComponent.LayerDepth);
    }
    private void DrawSeperateRandom(GameTime gameTime, SpriteBatch spriteBatch, Rectangle destRectangle, SpriteEffects spriteEffects)
    {
        _delayTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_delayTime > _animationComponent.AnimationSprite.Delay)
        {
            _totalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_totalElapsed > _timePerFrame)
            {
                _totalElapsed -= _timePerFrame;

                _frameIndex = Math.Min(_frameIndex + 1, _animationComponent.AnimationSprite.RowFrameCount[_randomRow] - 1);
                if (_frameIndex == _animationComponent.AnimationSprite.RowFrameCount[_randomRow] - 1)
                {
                    _delayTime = 0;
                    _animationEnded = true;
                    _frameIndex = 0;
                    if (_randomRow < _animationComponent.AnimationSprite.RowFrameCount.Length - 1)
                        _randomRow += 1;
                    else
                        _randomRow = 0;
                }
                _textureSpriteFile = _animationComponent.TextureSpriteFiles[_frameIndex];
            }
        }
        spriteBatch.Draw(_textureSpriteFile.Texture, destRectangle, null, Color.White, 0, _origin, spriteEffects, _animationComponent.LayerDepth);
    }

    #endregion
}