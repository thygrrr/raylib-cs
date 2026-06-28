#if BROWSER
using Examples;
namespace Examples.Shapes;

public partial class BasicShapes : IExample
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
        public string Name => "Shapes / Basic Shapes";

        private float _rotation;

        public void Init()
        {
            _rotation = 0.0f;
        }

        public void Update()
        {
            int screenWidth = GetScreenWidth();

            _rotation += 0.2f;

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("some basic shapes available on raylib", 20, 20, 20, Color.DarkGray);

            // Circle shapes and lines
            DrawCircle(screenWidth / 5, 120, 35, Color.DarkBlue);
            DrawCircleGradient(new Vector2(screenWidth / 5.0f, 220.0f), 60, Color.Green, Color.SkyBlue);
            DrawCircleLines(screenWidth / 5, 340, 80, Color.DarkBlue);
            DrawEllipse(screenWidth / 5, 120, 25, 20, Color.Yellow);
            DrawEllipseLines(screenWidth / 5, 120, 30, 25, Color.Yellow);

            // Rectangle shapes and lines
            DrawRectangle(screenWidth / 4 * 2 - 60, 100, 120, 60, Color.Red);
            DrawRectangleGradientH(screenWidth / 4 * 2 - 90, 170, 180, 130, Color.Maroon, Color.Gold);
            DrawRectangleLines(screenWidth / 4 * 2 - 40, 320, 80, 60, Color.Orange);  // NOTE: Uses QUADS internally, not lines

            // Triangle shapes and lines
            DrawTriangle(
                new Vector2(screenWidth / 4.0f * 3.0f, 80.0f),
                new Vector2(screenWidth / 4.0f * 3.0f - 60.0f, 150.0f),
                new Vector2(screenWidth / 4.0f * 3.0f + 60.0f, 150.0f), Color.Violet
            );

            DrawTriangleLines(
                new Vector2(screenWidth / 4.0f * 3.0f, 160.0f),
                new Vector2(screenWidth / 4.0f * 3.0f - 20.0f, 230.0f),
                new Vector2(screenWidth / 4.0f * 3.0f + 20.0f, 230.0f), Color.DarkBlue
            );

            // Polygon shapes and lines
            DrawPoly(new Vector2(screenWidth / 4.0f * 3, 330), 6, 80, _rotation, Color.Brown);
            DrawPolyLines(new Vector2(screenWidth / 4.0f * 3, 330), 6, 90, _rotation, Color.Brown);
            DrawPolyLinesEx(new Vector2(screenWidth / 4.0f * 3, 330), 6, 85, _rotation, 6, Color.Beige);

            // NOTE: We draw all LINES based shapes together to optimize internal drawing,
            // this way, all LINES are rendered in a single draw pass
            DrawLine(18, 42, screenWidth - 18, 42, Color.Black);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
