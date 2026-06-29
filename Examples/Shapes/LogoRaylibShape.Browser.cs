#if BROWSER
using Examples;
namespace Examples.Shapes;

public partial class LogoRaylibShape : IExample
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
        public string Name => "Shapes / Logo Raylib Shape";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        public void Init()
        {
        }

        public void Update()
        {

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawRectangle(screenWidth / 2 - 128, screenHeight / 2 - 128, 256, 256, new Color(139, 71, 135, 255));
            DrawRectangle(screenWidth / 2 - 112, screenHeight / 2 - 112, 224, 224, Color.RayWhite);
            DrawText("raylib", screenWidth / 2 - 44, screenHeight / 2 + 28, 50, new Color(155, 79, 151, 255));
            DrawText("cs", screenWidth / 2 - 44, screenHeight / 2 + 58, 50, new Color(155, 79, 151, 255));

            DrawText("this is NOT a texture!", 350, 370, 10, Color.Gray);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
