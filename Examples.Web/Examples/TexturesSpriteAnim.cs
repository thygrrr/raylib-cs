// Adapted for the browser from Examples/Textures/SpriteAnim.cs
using System;

namespace Examples.Web;

public class TexturesSpriteAnim : IWebExample
{
    public string Name => "Textures / Sprite Anim";

    private const int MaxFrameSpeed = 15;
    private const int MinFrameSpeed = 1;
    private const int ScreenWidth = 960;
    private const int ScreenHeight = 540;

    private Texture2D _scarfy;
    private Vector2 _position;
    private Rectangle _frameRec;
    private int _currentFrame;
    private int _framesCounter;
    private int _framesSpeed;

    public void Init()
    {
        // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
        _scarfy = LoadTexture("resources/scarfy.png");

        _position = new(350.0f, 280.0f);
        _frameRec = new(0.0f, 0.0f, (float)_scarfy.Width / 6, (float)_scarfy.Height);
        _currentFrame = 0;

        _framesCounter = 0;

        // Number of spritesheet frames shown by second
        _framesSpeed = 8;
    }

    public void Update()
    {
        _framesCounter++;

        if (_framesCounter >= (60 / _framesSpeed))
        {
            _framesCounter = 0;
            _currentFrame++;

            if (_currentFrame > 5)
            {
                _currentFrame = 0;
            }

            _frameRec.X = (float)_currentFrame * (float)_scarfy.Width / 6;
        }

        if (IsKeyPressed(KeyboardKey.Right))
        {
            _framesSpeed++;
        }
        else if (IsKeyPressed(KeyboardKey.Left))
        {
            _framesSpeed--;
        }

        _framesSpeed = Math.Clamp(_framesSpeed, MinFrameSpeed, MaxFrameSpeed);

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawTexture(_scarfy, 15, 40, Color.White);
        DrawRectangleLines(15, 40, _scarfy.Width, _scarfy.Height, Color.Lime);
        DrawRectangleLines(
            15 + (int)_frameRec.X,
            40 + (int)_frameRec.Y,
            (int)_frameRec.Width,
            (int)_frameRec.Height,
            Color.Red
        );

        DrawText("FRAME SPEED: ", 165, 210, 10, Color.DarkGray);
        DrawText($"{_framesSpeed:2F} FPS", 575, 210, 10, Color.DarkGray);
        DrawText("PRESS RIGHT/LEFT KEYS to CHANGE SPEED!", 290, 240, 10, Color.DarkGray);

        for (int i = 0; i < MaxFrameSpeed; i++)
        {
            if (i < _framesSpeed)
            {
                DrawRectangle(250 + 21 * i, 205, 20, 20, Color.Red);
            }
            DrawRectangleLines(250 + 21 * i, 205, 20, 20, Color.Maroon);
        }

        // Draw part of the texture
        DrawTextureRec(_scarfy, _frameRec, _position, Color.White);
        DrawText("(c) Scarfy sprite by Eiden Marsal", ScreenWidth - 200, ScreenHeight - 20, 10, Color.Gray);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadTexture(_scarfy);
    }
}
