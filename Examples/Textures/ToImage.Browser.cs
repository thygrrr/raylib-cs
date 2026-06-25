#if BROWSER
using Examples;
namespace Examples.Textures;

public partial class ToImage : IExample
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
        public string Name => "Textures / Texture to Image";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Texture2D _texture;

        public void Init()
        {
            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)

            Image image = LoadImage("resources/raylib-cs_logo.png");
            _texture = LoadTextureFromImage(image);
            UnloadImage(image);

            image = LoadImageFromTexture(_texture);
            UnloadTexture(_texture);

            _texture = LoadTextureFromImage(image);
            UnloadImage(image);
        }

        public void Update()
        {
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            int x = screenWidth / 2 - _texture.Width / 2;
            int y = screenHeight / 2 - _texture.Height / 2;
            DrawTexture(_texture, x, y, Color.White);

            DrawText("this IS a texture loaded from an image!", 300, 370, 10, Color.Gray);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_texture);
        }
    }
}
#endif
