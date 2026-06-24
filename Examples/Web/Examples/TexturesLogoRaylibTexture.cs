// Adapted for the browser from Examples/Textures/LogoRaylibTexture.cs
namespace Examples.Web;

public class TexturesLogoRaylibTexture : IWebExample
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
