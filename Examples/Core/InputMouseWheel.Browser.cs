#if BROWSER
using Examples;
namespace Examples.Core;

public partial class InputMouseWheel : IExample
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
        public string Name => "Core / Mouse Wheel";

        private const int ScrollSpeed = 4;
        private int _boxPositionY;

        public void Init()
        {
            _boxPositionY = GetScreenHeight() / 2 - 40;
        }

        public void Update()
        {
            _boxPositionY -= (int)(GetMouseWheelMove() * ScrollSpeed);

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawRectangle(GetScreenWidth() / 2 - 40, _boxPositionY, 80, 80, Color.Maroon);

            DrawText("Use mouse wheel to move the cube up and down!", 10, 10, 20, Color.Gray);
            DrawText($"Box position Y: {_boxPositionY}", 10, 40, 20, Color.LightGray);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
