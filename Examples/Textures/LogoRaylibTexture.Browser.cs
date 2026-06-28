#if BROWSER
using Examples;
namespace Examples.Textures;

public partial class LogoRaylibTexture : IExample
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
        public string Name => "Textures / Logo Raylib Texture";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Texture2D _texture;

        public void Init()
        {
            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
            _texture = LoadTexture("resources/raylib-cs_logo.png");
        }

        public void Update()
        {
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawTexture(
                _texture,
                screenWidth / 2 - _texture.Width / 2,
                screenHeight / 2 - _texture.Height / 2,
                Color.White
            );

            DrawText("this IS a texture!", 360, 370, 10, Color.Gray);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_texture);
        }
    }
}
#endif
