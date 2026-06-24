// Adapted for the browser from Examples/Textures/ImageGeneration.cs
namespace Examples.Web;

public class TexturesImageGeneration : IWebExample
{
    public string Name => "Textures / Image Generation";

    private const int NumTextures = 6;

    private const int screenWidth = 800;
    private const int screenHeight = 450;

    private Texture2D[] _textures;
    private int _currentTexture;

    public void Init()
    {
        Image verticalGradient = GenImageGradientLinear(screenWidth, screenHeight, 0, Color.Red, Color.Blue);
        Image horizontalGradient = GenImageGradientLinear(screenWidth, screenHeight, 90, Color.Red, Color.Blue);
        Image radialGradient = GenImageGradientRadial(screenWidth, screenHeight, 0.0f, Color.White, Color.Black);
        Image isChecked = GenImageChecked(screenWidth, screenHeight, 32, 32, Color.Red, Color.Blue);
        Image whiteNoise = GenImageWhiteNoise(screenWidth, screenHeight, 0.5f);
        Image cellular = GenImageCellular(screenWidth, screenHeight, 32);

        _textures = new Texture2D[NumTextures];
        _textures[0] = LoadTextureFromImage(verticalGradient);
        _textures[1] = LoadTextureFromImage(horizontalGradient);
        _textures[2] = LoadTextureFromImage(radialGradient);
        _textures[3] = LoadTextureFromImage(isChecked);
        _textures[4] = LoadTextureFromImage(whiteNoise);
        _textures[5] = LoadTextureFromImage(cellular);

        UnloadImage(verticalGradient);
        UnloadImage(horizontalGradient);
        UnloadImage(radialGradient);
        UnloadImage(isChecked);
        UnloadImage(whiteNoise);
        UnloadImage(cellular);

        _currentTexture = 0;
    }

    public void Update()
    {
        if (IsMouseButtonPressed(MouseButton.Left) || IsKeyPressed(KeyboardKey.Right))
        {
            // Cycle between the textures
            _currentTexture = (_currentTexture + 1) % NumTextures;
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawTexture(_textures[_currentTexture], 0, 0, Color.White);

        DrawRectangle(30, 400, 325, 30, ColorAlpha(Color.SkyBlue, 0.5f));
        DrawRectangleLines(30, 400, 325, 30, ColorAlpha(Color.White, 0.5f));
        DrawText("MOUSE LEFT BUTTON to CYCLE PROCEDURAL TEXTURES", 40, 410, 10, Color.White);

        switch (_currentTexture)
        {
            case 0:
                DrawText("VERTICAL GRADIENT", 560, 10, 20, Color.RayWhite);
                break;
            case 1:
                DrawText("HORIZONTAL GRADIENT", 540, 10, 20, Color.RayWhite);
                break;
            case 2:
                DrawText("RADIAL GRADIENT", 580, 10, 20, Color.LightGray);
                break;
            case 3:
                DrawText("CHECKED", 680, 10, 20, Color.RayWhite);
                break;
            case 4:
                DrawText("Color.WHITE NOISE", 640, 10, 20, Color.Red);
                break;
            case 5:
                DrawText("CELLULAR", 670, 10, 20, Color.RayWhite);
                break;
            default:
                break;
        }

        EndDrawing();
    }

    public void Unload()
    {
        for (int i = 0; i < _textures.Length; i++)
        {
            UnloadTexture(_textures[i]);
        }
    }
}
