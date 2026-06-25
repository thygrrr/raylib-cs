// Adapted for the browser from Examples/Shapes/LogoRaylibShape.cs
namespace Examples.Web;

public class ShapesLogoRaylibShape : IWebExample
{
    public string Name => "Shapes / Logo Raylib Shape";

    private const int screenWidth = 800;
    private const int screenHeight = 450;

    public void Init()
    {
    }

    public void Update()
    {
        // TODO: Update your variables here

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
