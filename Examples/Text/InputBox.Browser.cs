#if BROWSER
using Examples;
namespace Examples.Text;

public partial class InputBox : IExample
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
        public string Name => "Text / Input Box";

        private const int MaxInputChars = 9;

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        // NOTE: One extra space required for line ending char '\0'
        private char[] _name;
        private int _letterCount;

        private Rectangle _textBox;
        private bool _mouseOnText;

        private int _framesCounter;

        public void Init()
        {
            _name = new char[MaxInputChars];
            _letterCount = 0;

            _textBox = new(screenWidth / 2 - 100, 180, 225, 50);
            _mouseOnText = false;

            _framesCounter = 0;
        }

        public void Update()
        {
            if (CheckCollisionPointRec(GetMousePosition(), _textBox))
            {
                _mouseOnText = true;
            }
            else
            {
                _mouseOnText = false;
            }

            if (_mouseOnText)
            {
                // Set the window's cursor to the I-Beam
                SetMouseCursor(MouseCursor.IBeam);

                // Check if more characters have been pressed on the same frame
                int key = GetCharPressed();

                while (key > 0)
                {
                    // NOTE: Only allow keys in range [32..125]
                    if ((key >= 32) && (key <= 125) && (_letterCount < MaxInputChars))
                    {
                        _name[_letterCount] = (char)key;
                        _letterCount++;
                    }

                    // Check next character in the queue
                    key = GetCharPressed();
                }

                if (IsKeyPressed(KeyboardKey.Backspace))
                {
                    _letterCount -= 1;
                    if (_letterCount < 0)
                    {
                        _letterCount = 0;
                    }
                    _name[_letterCount] = '\0';
                }
            }
            else
            {
                SetMouseCursor(MouseCursor.Default);
            }

            if (_mouseOnText)
            {
                _framesCounter += 1;
            }
            else
            {
                _framesCounter = 0;
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("PLACE MOUSE OVER INPUT BOX!", 240, 140, 20, Color.Gray);
            DrawRectangleRec(_textBox, Color.LightGray);

            if (_mouseOnText)
            {
                DrawRectangleLines(
                    (int)_textBox.X,
                    (int)_textBox.Y,
                    (int)_textBox.Width,
                    (int)_textBox.Height,
                    Color.Red
                );
            }
            else
            {
                DrawRectangleLines(
                    (int)_textBox.X,
                    (int)_textBox.Y,
                    (int)_textBox.Width,
                    (int)_textBox.Height,
                    Color.DarkGray
                );
            }

            DrawText(new string(_name), (int)_textBox.X + 5, (int)_textBox.Y + 8, 40, Color.Maroon);
            DrawText($"INPUT CHARS: {_letterCount}/{MaxInputChars}", 315, 250, 20, Color.DarkGray);

            if (_mouseOnText)
            {
                if (_letterCount < MaxInputChars)
                {
                    // Draw blinking underscore char
                    if ((_framesCounter / 20 % 2) == 0)
                    {
                        DrawText(
                            "_",
                            (int)_textBox.X + 8 + MeasureText(new string(_name), 40),
                            (int)_textBox.Y + 12,
                            40,
                            Color.Maroon
                        );
                    }
                }
                else
                {
                    DrawText("Press BACKSPACE to delete chars...", 230, 300, 20, Color.Gray);
                }
            }

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
