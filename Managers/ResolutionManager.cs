using Barebone.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Barebone.Managers;

public class ResolutionManager
{
    private static GraphicsDevice graphics;
    private static int vWidth;
    private static int vHeight;
    
    public static Matrix ScreenScaleMatrix { get; private set; }
    public static Viewport Viewport { get; private set; }

    //new Rectangle(0, 0, Constants.ScreenWidth, Constants.ScreenHeight)
    public static Rectangle ScreenRectangle { get => new(0, 0, Constants.ScreenWidth, Constants.ScreenHeight); }


    public static void Init(GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight)
    {
        graphics = graphicsDevice;
        vWidth = virtualWidth;
        vHeight = virtualHeight;
        UpdateScreenScaleMatrix();
    }

    public static void UpdateScreenScaleMatrix()
    {
        float screenWidth = graphics.PresentationParameters.BackBufferWidth;
        float screenHeight = graphics.PresentationParameters.BackBufferHeight;

        if(screenWidth / Constants.ScreenWidth > screenHeight / Constants.ScreenHeight)
        {
            float ratio = screenHeight / Constants.ScreenHeight;
            vWidth = (int)(ratio * Constants.ScreenWidth);
            vHeight = (int)screenHeight;
        }
        else
        {
            float ratio = screenWidth / Constants.ScreenWidth;
            vWidth = (int)screenWidth;
            vHeight = (int)(ratio * Constants.ScreenHeight);
        }

        float aspectRatio = Math.Min(graphics.PresentationParameters.BackBufferWidth / (float)Constants.ScreenWidth,
                                     graphics.PresentationParameters.BackBufferHeight / (float)Constants.ScreenHeight);
        ScreenScaleMatrix = Matrix.CreateScale(aspectRatio);

        Viewport = new Viewport
        {
            X = (int)(screenWidth / 2 - vWidth / 2),
            Y = (int)(screenHeight / 2 - vHeight / 2),
            Width = vWidth,
            Height = vHeight,
            MinDepth = 0,
            MaxDepth = 1
        };
    }
}
