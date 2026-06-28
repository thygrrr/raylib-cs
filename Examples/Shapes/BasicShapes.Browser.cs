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

        public void Init()
        {
        }

        public void Update()
        {
            int screenWidth = GetScreenWidth();

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("some basic shapes available on raylib", 20, 20, 20, Color.DarkGray);

            DrawLine(18, 42, screenWidth - 18, 42, Color.Black);

            DrawCircle(screenWidth / 4, 120, 35, Color.DarkBlue);
            DrawCircleGradient(new Vector2(screenWidth / 4, 220), 60, Color.Green, Color.SkyBlue);
            DrawCircleLines(screenWidth / 4, 340, 80, Color.DarkBlue);

            DrawRectangle(screenWidth / 4 * 2 - 60, 100, 120, 60, Color.Red);
            DrawRectangleGradientH(screenWidth / 4 * 2 - 90, 170, 180, 130, Color.Maroon, Color.Gold);
            DrawRectangleLines(screenWidth / 4 * 2 - 40, 320, 80, 60, Color.Orange);

            DrawTriangle(
                new Vector2(screenWidth / 4 * 3, 80),
                new Vector2(screenWidth / 4 * 3 - 60, 150),
                new Vector2(screenWidth / 4 * 3 + 60, 150), Color.Violet
            );

            DrawTriangleLines(
                new Vector2(screenWidth / 4 * 3, 160),
                new Vector2(screenWidth / 4 * 3 - 20, 230),
                new Vector2(screenWidth / 4 * 3 + 20, 230), Color.DarkBlue
            );

            DrawPoly(new Vector2(screenWidth / 4 * 3, 320), 6, 80, 0, Color.Brown);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
