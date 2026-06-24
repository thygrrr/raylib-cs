// Adapted for the browser from Examples/Shapes/LogoRaylibAnim.cs
namespace Examples.Web;

public class ShapesLogoRaylibAnim : IWebExample
{
    public string Name => "Shapes / Logo Raylib Anim";

    private const int screenWidth = 960;
    private const int screenHeight = 540;

    private int _logoPositionX;
    private int _logoPositionY;

    private int _framesCounter;
    private int _lettersCount;

    private int _topSideRecWidth;
    private int _leftSideRecHeight;

    private int _bottomSideRecWidth;
    private int _rightSideRecHeight;

    // Tracking animation states (State Machine)
    private int _state;

    // Useful for fading
    private float _alpha;

    private Color _outline;

    public void Init()
    {
        _logoPositionX = screenWidth / 2 - 128;
        _logoPositionY = screenHeight / 2 - 128;

        _framesCounter = 0;
        _lettersCount = 0;

        _topSideRecWidth = 16;
        _leftSideRecHeight = 16;

        _bottomSideRecWidth = 16;
        _rightSideRecHeight = 16;

        _state = 0;
        _alpha = 1.0f;

        _outline = new Color(139, 71, 135, 255);
    }

    public void Update()
    {
        // State 0: Small box blinking
        if (_state == 0)
        {
            _framesCounter++;

            // Reset counter... will be used later...
            if (_framesCounter == 120)
            {
                _state = 1;
                _framesCounter = 0;
            }
        }
        // State 1: Top and left bars growing
        else if (_state == 1)
        {
            _topSideRecWidth += 4;
            _leftSideRecHeight += 4;

            if (_topSideRecWidth == 256)
            {
                _state = 2;
            }
        }
        // State 2: Bottom and right bars growing
        else if (_state == 2)
        {
            _bottomSideRecWidth += 4;
            _rightSideRecHeight += 4;

            if (_bottomSideRecWidth == 256)
            {
                _state = 3;
            }
        }
        // State 3: Letters appearing (one by one)
        else if (_state == 3)
        {
            _framesCounter++;

            // Every 12 frames, one more letter!
            if (_framesCounter / 12 != 0)
            {
                _lettersCount++;
                _framesCounter = 0;
            }

            // When all letters have appeared, just fade out everything
            if (_lettersCount >= 10)
            {
                _alpha -= 0.02f;

                if (_alpha <= 0.0f)
                {
                    _alpha = 0.0f;
                    _state = 4;
                }
            }
        }
        // State 4: Reset and Replay
        else if (_state == 4)
        {
            if (IsKeyPressed(KeyboardKey.R))
            {
                _framesCounter = 0;
                _lettersCount = 0;

                _topSideRecWidth = 16;
                _leftSideRecHeight = 16;

                _bottomSideRecWidth = 16;
                _rightSideRecHeight = 16;

                // Return to State 0
                _alpha = 1.0f;
                _state = 0;
            }
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        if (_state == 0)
        {
            if ((_framesCounter / 15) % 2 != 0)
            {
                DrawRectangle(_logoPositionX, _logoPositionY, 16, 16, _outline);
            }
        }
        else if (_state == 1)
        {
            DrawRectangle(_logoPositionX, _logoPositionY, _topSideRecWidth, 16, _outline);
            DrawRectangle(_logoPositionX, _logoPositionY, 16, _leftSideRecHeight, _outline);
        }
        else if (_state == 2)
        {
            DrawRectangle(_logoPositionX, _logoPositionY, _topSideRecWidth, 16, _outline);
            DrawRectangle(_logoPositionX, _logoPositionY, 16, _leftSideRecHeight, _outline);

            DrawRectangle(_logoPositionX + 240, _logoPositionY, 16, _rightSideRecHeight, _outline);
            DrawRectangle(_logoPositionX, _logoPositionY + 240, _bottomSideRecWidth, 16, _outline);
        }
        else if (_state == 3)
        {
            Color outlineFade = ColorAlpha(_outline, _alpha);
            DrawRectangle(_logoPositionX, _logoPositionY, _topSideRecWidth, 16, outlineFade);
            DrawRectangle(_logoPositionX, _logoPositionY + 16, 16, _leftSideRecHeight - 32, outlineFade);

            DrawRectangle(_logoPositionX + 240, _logoPositionY + 16, 16, _rightSideRecHeight - 32, outlineFade);
            DrawRectangle(_logoPositionX, _logoPositionY + 240, _bottomSideRecWidth, 16, outlineFade);

            Color whiteFade = ColorAlpha(Color.RayWhite, _alpha);
            DrawRectangle(screenWidth / 2 - 112, screenHeight / 2 - 112, 224, 224, whiteFade);

            Color label = ColorAlpha(new Color(155, 79, 151, 255), _alpha);
            string text = "raylib".SubText(0, _lettersCount);
            DrawText(text, screenWidth / 2 - 44, screenHeight / 2 + 28, 50, label);

            DrawText("cs".SubText(0, _lettersCount), screenWidth / 2 - 44, screenHeight / 2 + 58, 50, label);
        }
        else if (_state == 4)
        {
            DrawText("[R] REPLAY", 340, 200, 20, Color.Gray);
        }

        EndDrawing();
    }

    public void Unload()
    {
    }
}
