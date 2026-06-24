// Adapted for the browser from Examples/Core/InputMouseWheel.cs
namespace Examples.Web;

public class CoreInputMouseWheel : IWebExample
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
