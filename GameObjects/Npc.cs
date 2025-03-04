using Barebone.Components;
using Barebone.Contracts;
using Barebone.Core;
using Barebone.Managers;
using Barebone.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Barebone.GameObjects;

public class Npc(string name, float layerDepth, Screen screen) : IInteractiveObject
{
    public string Name => name;
    public float LayerDepth => layerDepth;
    public GameObjectType Type => GameObjectType.Npc;


    private string _state;
    private readonly Screen _screen = screen;
    private AnimationComponent _currentAnimation;
    private readonly AnimationSystem _animationSystem = new();
    private readonly Dictionary<string, AnimationComponent> _animations = [];

    public void AddAnimation(AnimationComponent animation)
    {
        if (!_animations.TryAdd(animation.Name, animation) || _currentAnimation is not null) return;

        _state = animation.Name;
        _currentAnimation = animation;
        EventManager.Current.SetState(Name, animation.Name);
    }

    public void HandleInput(InputManager input)
    {
        InteractionManager.Interact(Type, Name, _currentAnimation.AnimationSprite.Rectangle, input, _screen);
    }
    public void Update(GameTime gameTime)
    {
        if(EventManager.Current.GetState(Name, out _state))
        {
            _animations.TryGetValue(_state, out _currentAnimation);
            _animationSystem.Play(_currentAnimation);
        }
    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _animationSystem.Draw(gameTime, spriteBatch, SpriteEffects.None);
    }
}