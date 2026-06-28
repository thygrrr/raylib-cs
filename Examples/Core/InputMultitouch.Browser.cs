#if BROWSER
using Examples;
namespace Examples.Core;

public partial class InputMultitouch : IExample
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
        public string Name => "Core / Input Multitouch";

        private const int MaxTouchPoints = 10;

        private Vector2[] _touchPositions;

        public void Init()
        {
            _touchPositions = new Vector2[MaxTouchPoints];
        }

        public void Update()
        {
            // Get the touch point count ( how many fingers are touching the screen )
            int tCount = GetTouchPointCount();

            // Clamp touch points available ( set the maximum touch points allowed )
            if (tCount > MaxTouchPoints)
            {
                tCount = MaxTouchPoints;
            }

            // Get touch points positions
            for (int i = 0; i < tCount; i++)
            {
                _touchPositions[i] = GetTouchPosition(i);
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            for (int i = 0; i < tCount; i++)
            {
                // Make sure point is not (0, 0) as this means there is no touch for it
                if ((_touchPositions[i].X > 0) && (_touchPositions[i].Y > 0))
                {
                    // Draw circle and touch index number
                    DrawCircleV(_touchPositions[i], 34, Color.Orange);
                    DrawText(i.ToString(),
                        (int)_touchPositions[i].X - 10,
                        (int)_touchPositions[i].Y - 70,
                        40,
                        Color.Black
                    );
                }
            }

            DrawText("touch the screen at multiple locations to get multiple balls", 10, 10, 20, Color.DarkGray);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
