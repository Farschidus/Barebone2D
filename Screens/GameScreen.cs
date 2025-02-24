using System;
using System.Collections.Generic;
using Barebone.Components;
using Barebone.Contracts;
using Barebone.Core;
using Barebone.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Barebone.Screens;

public class GameScreen : Screen
{
    private Scene _scene;
    private GameObjects.Texture _background;
    private readonly List<GameObjects.Npc> _npcs = [];
    private readonly List<GameObjects.Texture> _foregrounds = [];
    
    private GameScreen(Scene scene) : base(scene.Name)
    {
        _scene = scene;
        //TODO: could be move to the XML and defined by user at dev time under scene props, ex. <TransitionTime>0.5</TransitionTime>
        TransitionOnTime = TimeSpan.FromSeconds(0.5);
        TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    public override void LoadContent(ContentManager content)
    {
      _background = new GameObjects.Texture(new TextureComponent(
            _scene.Background.Name,
            _scene.Background.SpriteTexture,
            _scene.Background.Position,  
            _scene.Background.LayerDepth,
            content)
        );

        foreach (var npc in _scene.Npcs)
        {
            var character = new GameObjects.Npc(npc.Name, npc.LayerDepth);
            foreach (var animationSprite in npc.AnimationSprites)
            {
                character.AddAnimation(new AnimationComponent(animationSprite, npc.LayerDepth, content));
            }
            _npcs.Add(character);
        }

        foreach (var foreground in _scene.Foregrounds)
        {
            _foregrounds.Add(new GameObjects.Texture(new TextureComponent(
                foreground.Name, 
                foreground.SpriteTexture, 
                foreground.Position, 
                foreground.LayerDepth, 
                content)));
        }

        _scene = null;
    }
    public override void UnloadContent()
    {
        _background = null;
        _npcs.Clear();
        _foregrounds.Clear();
    }
    public override void HandleInput(InputManager input)
    {
        if (input.IsGamePaused)
        {
            ScreenManager.AddScreen(new PromptScreen("Game paused"));
        }

        foreach (var npc in _npcs)
        {
            npc.HandleInput(input);
        }
    }
    public override void Update(GameTime gameTime)
    {
        foreach (var npc in _npcs)
        {
            npc.Update(gameTime);
        }
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _background.Draw(gameTime, spriteBatch);

        foreach (var npc in _npcs)
        {
            npc.Draw(gameTime, spriteBatch);
        }

        foreach (var foreground in _foregrounds)
        {
            foreground.Draw(gameTime, spriteBatch);
        }
    }

    public static GameScreen InitNextScene(string sceneName, ContentManager content)
    {
        var gameAssets = content.Load<GameAssets>(Constants.GameAssetsFile);
        var scene = gameAssets.Scenes.Find(x => x.Name.Equals(sceneName, StringComparison.Ordinal));
        return new GameScreen(scene);
    }
}