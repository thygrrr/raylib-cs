#if BROWSER
using Examples;
using System;

namespace Examples.Shapes;

public partial class EasingsBoxAnim : IExample
{
    private readonly BrowserAdapter _browserAdapter = new();

    public string Name => _browserAdapter.Name;

    public void Init()
    {
        _browserAdapter.Init();
    }

    public void Update()
    {
        _browserAdapter.Update();
    }

    public void Unload()
    {
        _browserAdapter.Unload();
    }

    private sealed class BrowserAdapter : IExample
    {
        public string Name => "Shapes / Easings Box Anim";

        // Box variables to be animated with easings
        private Rectangle _rec;
        private float _rotation;
        private float _alpha;

        private int _state;
        private int _framesCounter;

        public void Init()
        {
            _rec = new Rectangle(GetScreenWidth() / 2.0f, -100, 100, 100);
            _rotation = 0.0f;
            _alpha = 1.0f;

            _state = 0;
            _framesCounter = 0;
        }

        public void Update()
        {
            switch (_state)
            {
                // Move box down to center of screen
                case 0:
                    _framesCounter += 1;

                    // NOTE: Remember that 3rd parameter of easing function refers to
                    // desired value variation, do not confuse it with expected final value!
                    _rec.Y = EaseElasticOut(_framesCounter, -100, GetScreenHeight() / 2.0f + 100, 120);

                    if (_framesCounter >= 120)
                    {
                        _framesCounter = 0;
                        _state = 1;
                    }
                    break;
                // Scale box to an horizontal bar
                case 1:
                    _framesCounter += 1;
                    _rec.Height = EaseBounceOut(_framesCounter, 100, -90, 120);
                    _rec.Width = EaseBounceOut(_framesCounter, 100, GetScreenWidth(), 120);

                    if (_framesCounter >= 120)
                    {
                        _framesCounter = 0;
                        _state = 2;
                    }
                    break;
                // Rotate horizontal bar rectangle
                case 2:
                    _framesCounter += 1;
                    _rotation = EaseQuadOut(_framesCounter, 0.0f, 270.0f, 240);

                    if (_framesCounter >= 240)
                    {
                        _framesCounter = 0;
                        _state = 3;
                    }
                    break;
                // Increase bar size to fill all screen
                case 3:
                    _framesCounter += 1;
                    _rec.Height = EaseCircOut(_framesCounter, 10, GetScreenWidth(), 120);

                    if (_framesCounter >= 120)
                    {
                        _framesCounter = 0;
                        _state = 4;
                    }
                    break;
                // Fade out animation
                case 4:
                    _framesCounter++;
                    _alpha = EaseSineOut(_framesCounter, 1.0f, -1.0f, 160);

                    if (_framesCounter >= 160)
                    {
                        _framesCounter = 0;
                        _state = 5;
                    }
                    break;
                default:
                    break;
            }

            // Reset animation at any moment
            if (IsKeyPressed(KeyboardKey.Space))
            {
                _rec = new Rectangle(GetScreenWidth() / 2.0f, -100, 100, 100);
                _rotation = 0.0f;
                _alpha = 1.0f;
                _state = 0;
                _framesCounter = 0;
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawRectanglePro(
                _rec,
                new Vector2(_rec.Width / 2, _rec.Height / 2),
                _rotation,
                Fade(Color.Black, _alpha)
            );
            DrawText("PRESS [SPACE] TO RESET BOX ANIMATION!", 10, GetScreenHeight() - 25, 20, Color.LightGray);

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

        // Bounce Easing functions
        private static float EaseBounceOut(float t, float b, float c, float d)
        {
            if ((t /= d) < (1 / 2.75f))
            {
                return (c * (7.5625f * t * t) + b);
            }
            else if (t < (2 / 2.75f))
            {
                float postFix = t -= (1.5f / 2.75f);
                return (c * (7.5625f * (postFix) * t + 0.75f) + b);
            }
            else if (t < (2.5 / 2.75))
            {
                float postFix = t -= (2.25f / 2.75f);
                return (c * (7.5625f * (postFix) * t + 0.9375f) + b);
            }
            else
            {
                float postFix = t -= (2.625f / 2.75f);
                return (c * (7.5625f * (postFix) * t + 0.984375f) + b);
            }
        }

        // Quadratic Easing functions
        private static float EaseQuadOut(float t, float b, float c, float d)
        {
            return (-c * (t /= d) * (t - 2) + b);
        }

        // Circular Easing functions
        private static float EaseCircOut(float t, float b, float c, float d)
        {
            return (c * MathF.Sqrt(1 - (t = t / d - 1) * t) + b);
        }

        // Sine Easing functions
        private static float EaseSineOut(float t, float b, float c, float d)
        {
            return (c * MathF.Sin(t / d * (MathF.PI / 2)) + b);
        }

        public void Unload()
        {
        }
    }
}
#endif
