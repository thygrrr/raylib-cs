// Adapted for the browser from Examples/Shapes/EasingsRectangleArray.cs
using System;

namespace Examples.Web;

public class ShapesEasingsRectangleArray : IWebExample
{
    public string Name => "Shapes / Easings Rectangle Array";

    private const int RecsWidth = 50;
    private const int RecsHeight = 50;
    private const int MaxRecsX = 960 / RecsWidth;
    private const int MaxRecsY = 540 / RecsHeight;

    // At 60 fps = 4 seconds
    private const int PlayTimeInFrames = 240;

    private Rectangle[] _recs;
    private float _rotation;
    private int _framesCounter;

    // Rectangles animation state: 0-Playing, 1-Finished
    private int _state;

    public void Init()
    {
        _recs = new Rectangle[MaxRecsX * MaxRecsY];

        for (int y = 0; y < MaxRecsY; y++)
        {
            for (int x = 0; x < MaxRecsX; x++)
            {
                _recs[y * MaxRecsX + x].X = RecsWidth / 2 + RecsWidth * x;
                _recs[y * MaxRecsX + x].Y = RecsHeight / 2 + RecsHeight * y;
                _recs[y * MaxRecsX + x].Width = RecsWidth;
                _recs[y * MaxRecsX + x].Height = RecsHeight;
            }
        }

        _rotation = 0.0f;
        _framesCounter = 0;
        _state = 0;
    }

    public void Update()
    {
        if (_state == 0)
        {
            _framesCounter++;

            for (int i = 0; i < MaxRecsX * MaxRecsY; i++)
            {
                _recs[i].Height = EaseCircOut(_framesCounter, RecsHeight, -RecsHeight, PlayTimeInFrames);
                _recs[i].Width = EaseCircOut(_framesCounter, RecsWidth, -RecsWidth, PlayTimeInFrames);

                if (_recs[i].Height < 0)
                {
                    _recs[i].Height = 0;
                }
                if (_recs[i].Width < 0)
                {
                    _recs[i].Width = 0;
                }

                // Finish playing
                if ((_recs[i].Height == 0) && (_recs[i].Width == 0))
                {
                    _state = 1;
                }
                _rotation = EaseLinearIn(_framesCounter, 0.0f, 360.0f, PlayTimeInFrames);
            }
        }
        else if ((_state == 1) && IsKeyPressed(KeyboardKey.Space))
        {
            // When animation has finished, press space to restart
            _framesCounter = 0;

            for (int i = 0; i < MaxRecsX * MaxRecsY; i++)
            {
                _recs[i].Height = RecsHeight;
                _recs[i].Width = RecsWidth;
            }

            _state = 0;
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        if (_state == 0)
        {
            for (int i = 0; i < MaxRecsX * MaxRecsY; i++)
            {
                DrawRectanglePro(
                    _recs[i],
                    new Vector2(_recs[i].Width / 2, _recs[i].Height / 2),
                    _rotation,
                    Color.Red
                );
            }
        }
        else if (_state == 1)
        {
            DrawText("PRESS [SPACE] TO PLAY AGAIN!", 240, 200, 20, Color.Gray);
        }

        EndDrawing();
    }

    // Circular Easing functions
    private static float EaseCircOut(float t, float b, float c, float d)
    {
        return (c * MathF.Sqrt(1 - (t = t / d - 1) * t) + b);
    }

    // Linear Easing functions
    private static float EaseLinearIn(float t, float b, float c, float d)
    {
        return (c * t / d + b);
    }

    public void Unload()
    {
    }
}
