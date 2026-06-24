// Adapted for the browser from Examples/Shapes/EasingsBallAnim.cs
using System;

namespace Examples.Web;

public class ShapesEasingsBallAnim : IWebExample
{
    public string Name => "Shapes / Easings Ball Anim";

    private const int screenWidth = 960;
    private const int screenHeight = 540;

    // Ball variable value to be animated with easings
    private int _ballPositionX;
    private int _ballRadius;
    private float _ballAlpha;

    private int _state;
    private int _framesCounter;

    public void Init()
    {
        _ballPositionX = -100;
        _ballRadius = 20;
        _ballAlpha = 0.0f;

        _state = 0;
        _framesCounter = 0;
    }

    public void Update()
    {
        if (_state == 0)             // Move ball position X with easing
        {
            _framesCounter += 1;
            _ballPositionX = (int)EaseElasticOut(_framesCounter, -100, screenWidth / 2 + 100, 120);

            if (_framesCounter >= 120)
            {
                _framesCounter = 0;
                _state = 1;
            }
        }
        // Increase ball radius with easing
        else if (_state == 1)
        {
            _framesCounter += 1;
            _ballRadius = (int)EaseElasticIn(_framesCounter, 20, 500, 200);

            if (_framesCounter >= 200)
            {
                _framesCounter = 0;
                _state = 2;
            }
        }
        // Change ball alpha with easing (background color blending)
        else if (_state == 2)
        {
            _framesCounter += 1;
            _ballAlpha = EaseCubicOut(_framesCounter, 0.0f, 1.0f, 200);

            if (_framesCounter >= 200)
            {
                _framesCounter = 0;
                _state = 3;
            }
        }
        // Reset state to play again
        else if (_state == 3)
        {
            if (IsKeyPressed(KeyboardKey.Enter))
            {
                // Reset required variables to play again
                _ballPositionX = -100;
                _ballRadius = 20;
                _ballAlpha = 0.0f;
                _state = 0;
            }
        }

        if (IsKeyPressed(KeyboardKey.R))
        {
            _framesCounter = 0;
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        if (_state >= 2)
        {
            DrawRectangle(0, 0, screenWidth, screenHeight, Color.Green);
        }

        DrawCircle(_ballPositionX, 200, _ballRadius, ColorAlpha(Color.Red, 1.0f - _ballAlpha));

        if (_state == 3)
        {
            DrawText("PRESS [ENTER] TO PLAY AGAIN!", 240, 200, 20, Color.Black);
        }

        EndDrawing();
    }

    // Elastic Easing functions
    private static float EaseElasticOut(float t, float b, float c, float d)
    {
        if (t == 0)
        {
            return b;
        }
        if ((t /= d) == 1)
        {
            return (b + c);
        }

        float p = d * 0.3f;
        float a = c;
        float s = p / 4;

        return (a * MathF.Pow(2, -10 * t) * MathF.Sin((t * d - s) * (2 * MathF.PI) / p) + c + b);
    }

    private static float EaseElasticIn(float t, float b, float c, float d)
    {
        if (t == 0)
        {
            return b;
        }
        if ((t /= d) == 1)
        {
            return (b + c);
        }

        float p = d * 0.3f;
        float a = c;
        float s = p / 4;
        float postFix = a * MathF.Pow(2, 10 * (t -= 1));

        return (-(postFix * MathF.Sin((t * d - s) * (2 * MathF.PI) / p)) + b);
    }

    // Cubic Easing functions
    private static float EaseCubicOut(float t, float b, float c, float d)
    {
        return (c * ((t = t / d - 1) * t * t + 1) + b);
    }

    public void Unload()
    {
    }
}
