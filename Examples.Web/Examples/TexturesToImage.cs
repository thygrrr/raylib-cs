// Adapted for the browser from Examples/Textures/ToImage.cs
namespace Examples.Web;

public class TexturesToImage : IWebExample
{
    public string Name => "Textures / Texture to Image";

    private const int ScreenWidth = 960;
    private const int ScreenHeight = 540;

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

        int x = ScreenWidth / 2 - _texture.Width / 2;
        int y = ScreenHeight / 2 - _texture.Height / 2;
        DrawTexture(_texture, x, y, Color.White);

        DrawText("this IS a texture loaded from an image!", 300, 370, 10, Color.Gray);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadTexture(_texture);
    }
}
