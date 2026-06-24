// Adapted for the browser from Examples/Textures/ImageLoading.cs
namespace Examples.Web;

public class TexturesImageLoading : IWebExample
{
    public string Name => "Textures / Image Loading";

    private const int screenWidth = 800;
    private const int screenHeight = 450;

    private Texture2D _texture;

    public void Init()
    {
        // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
        Image image = LoadImage("resources/raylib-cs_logo.png");
        _texture = LoadTextureFromImage(image);

        UnloadImage(image);
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

        DrawText("this IS a texture loaded from an image!", 300, 370, 10, Color.Gray);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadTexture(_texture);
    }
}
