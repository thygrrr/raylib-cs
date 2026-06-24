// Adapted for the browser from Examples/Shaders/ShapesTextures.cs
namespace Examples.Web;

public class ShadersShapesTextures : IWebExample
{
    public string Name => "Shaders / Shapes Textures";

    private const int screenHeight = 540;

    private Texture2D _fudesumi;
    private Shader _shader;

    public void Init()
    {
        _fudesumi = LoadTexture("resources/fudesumi.png");

        // NOTE: Using GLSL 100 shader version for WebGL1 (OpenGL ES 2.0)
        _shader = LoadShader(
            "resources/shaders/glsl100/base.vs",
            "resources/shaders/glsl100/grayscale.fs"
        );
    }

    public void Update()
    {
        BeginDrawing();
        ClearBackground(Color.RayWhite);

        // Start drawing with default shader
        DrawText("USING DEFAULT SHADER", 20, 40, 10, Color.Red);

        DrawCircle(80, 120, 35, Color.DarkBlue);
        DrawCircleGradient(new Vector2(80, 220), 60, Color.Green, Color.SkyBlue);
        DrawCircleLines(80, 340, 80, Color.DarkBlue);

        // Activate our custom shader to be applied on next shapes/textures drawings
        BeginShaderMode(_shader);

        DrawText("USING CUSTOM SHADER", 190, 40, 10, Color.Red);

        DrawRectangle(250 - 60, 90, 120, 60, Color.Red);
        DrawRectangleGradientH(250 - 90, 170, 180, 130, Color.Maroon, Color.Gold);
        DrawRectangleLines(250 - 40, 320, 80, 60, Color.Orange);

        // Activate our default shader for next drawings
        EndShaderMode();

        DrawText("USING DEFAULT SHADER", 370, 40, 10, Color.Red);

        DrawTriangle(
            new Vector2(430, 80),
            new Vector2(430 - 60, 150),
            new Vector2(430 + 60, 150), Color.Violet
        );

        DrawTriangleLines(
            new Vector2(430, 160),
            new Vector2(430 - 20, 230),
            new Vector2(430 + 20, 230), Color.DarkBlue
        );

        DrawPoly(new Vector2(430, 320), 6, 80, 0, Color.Brown);

        // Activate our custom shader to be applied on next shapes/textures drawings
        BeginShaderMode(_shader);

        // Using custom shader
        DrawTexture(_fudesumi, 500, -30, Color.White);

        // Activate our default shader for next drawings
        EndShaderMode();

        DrawText("(c) Fudesumi sprite by Eiden Marsal", 380, screenHeight - 20, 10, Color.Gray);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadShader(_shader);
        UnloadTexture(_fudesumi);
    }
}
