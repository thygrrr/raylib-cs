#if BROWSER
using Examples;
namespace Examples.Core;

public partial class BasicWindow : IExample
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
        public string Name => "Core / Basic Window";

        public void Init()
        {
        }

        public void Update()
        {
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("Congrats! You created your first window!", 190, 200, 20, Color.LightGray);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
