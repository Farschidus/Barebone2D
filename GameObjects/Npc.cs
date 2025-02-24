using Barebone.Components;
using Barebone.Core;
using Barebone.Contracts;
using Barebone.Managers;
using Barebone.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace Barebone.GameObjects;

public class Npc(string name, float layerDepth) : GameObject(name, layerDepth)
{
    private readonly Dictionary<string, AnimationComponent> _animations = [];
    private readonly AnimationSystem _animationSystem = new();
    private AnimationComponent _currentAnimation;
    private string _state;

    public void AddAnimation(AnimationComponent animation)
    {
        if (!_animations.TryAdd(animation.Name, animation) || _currentAnimation is not null) return;

        _state = animation.Name;
        _currentAnimation = animation;
        EventManager.Current.SetState(Name, animation.Name);
    }

    public override void HandleInput(InputManager input)
    {
        InteractionManager.Interact(Name, _currentAnimation.AnimationSprite.Rectangle, input);
    }
    public override void Update(GameTime gameTime)
    {
        if(EventManager.Current.GetState(Name, out _state))
        {
            _animations.TryGetValue(_state, out _currentAnimation);
            _animationSystem.Play(_currentAnimation);
        }
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _animationSystem.Draw(gameTime, spriteBatch, null, SpriteEffects.None);
    }
}
