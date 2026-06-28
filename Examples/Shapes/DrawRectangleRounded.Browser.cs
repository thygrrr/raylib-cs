#if BROWSER
using Examples;
namespace Examples.Shapes;

public partial class DrawRectangleRounded : IExample
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
        public string Name => "Shapes / Draw Rectangle Rounded";

        private float _roundness;
        private float _width;
        private float _height;
        private float _segments;
        private float _lineThick;

        private bool _drawRect;
        private bool _drawRoundedRect;
        private bool _drawRoundedLines;

        public void Init()
        {
            _roundness = 0.2f;
            _width = 200.0f;
            _height = 100.0f;
            _segments = 0.0f;
            _lineThick = 1.0f;

            _drawRect = false;
            _drawRoundedRect = true;
            _drawRoundedLines = false;
        }

        public void Update()
        {
            Rectangle rec = new(
                ((float)GetScreenWidth() - _width - 250) / 2,
                (GetScreenHeight() - _height) / 2.0f,
                (float)_width,
                (float)_height
            );

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawLine(560, 0, 560, GetScreenHeight(), Fade(Color.LightGray, 0.6f));
            DrawRectangle(560, 0, GetScreenWidth() - 500, GetScreenHeight(), Fade(Color.LightGray, 0.3f));

            if (_drawRect)
            {
                DrawRectangleRec(rec, Fade(Color.Gold, 0.6f));
            }
            if (_drawRoundedRect)
            {
                DrawRectangleRounded(rec, _roundness, (int)_segments, Fade(Color.Maroon, 0.2f));
            }
            if (_drawRoundedLines)
            {
                DrawRectangleRoundedLinesEx(rec, _roundness, (int)_segments, _lineThick, Fade(Color.Maroon, 0.4f));
            }

            string text = $"MODE: {((_segments >= 4) ? "MANUAL" : "AUTO")}";
            DrawText(text, 640, 280, 10, (_segments >= 4) ? Color.Maroon : Color.DarkGray);
            DrawFPS(10, 10);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
