#if BROWSER
using Examples;
namespace Examples.Textures;

public partial class ImageGeneration : IExample
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
        public string Name => "Textures / Image Generation";

        private const int NumTextures = 9;

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Texture2D[] _textures;
        private int _currentTexture;

        public void Init()
        {
            Image verticalGradient = GenImageGradientLinear(screenWidth, screenHeight, 0, Color.Red, Color.Blue);
            Image horizontalGradient = GenImageGradientLinear(screenWidth, screenHeight, 90, Color.Red, Color.Blue);
            Image diagonalGradient = GenImageGradientLinear(screenWidth, screenHeight, 45, Color.Red, Color.Blue);
            Image radialGradient = GenImageGradientRadial(screenWidth, screenHeight, 0.0f, Color.White, Color.Black);
            Image squareGradient = GenImageGradientSquare(screenWidth, screenHeight, 0.0f, Color.White, Color.Black);
            Image isChecked = GenImageChecked(screenWidth, screenHeight, 32, 32, Color.Red, Color.Blue);
            Image whiteNoise = GenImageWhiteNoise(screenWidth, screenHeight, 0.5f);
            Image perlinNoise = GenImagePerlinNoise(screenWidth, screenHeight, 50, 50, 4.0f);
            Image cellular = GenImageCellular(screenWidth, screenHeight, 32);

            _textures = new Texture2D[NumTextures];
            _textures[0] = LoadTextureFromImage(verticalGradient);
            _textures[1] = LoadTextureFromImage(horizontalGradient);
            _textures[2] = LoadTextureFromImage(diagonalGradient);
            _textures[3] = LoadTextureFromImage(radialGradient);
            _textures[4] = LoadTextureFromImage(squareGradient);
            _textures[5] = LoadTextureFromImage(isChecked);
            _textures[6] = LoadTextureFromImage(whiteNoise);
            _textures[7] = LoadTextureFromImage(perlinNoise);
            _textures[8] = LoadTextureFromImage(cellular);

            UnloadImage(verticalGradient);
            UnloadImage(horizontalGradient);
            UnloadImage(diagonalGradient);
            UnloadImage(radialGradient);
            UnloadImage(squareGradient);
            UnloadImage(isChecked);
            UnloadImage(whiteNoise);
            UnloadImage(perlinNoise);
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

            DrawRectangle(30, 400, 325, 30, Fade(Color.SkyBlue, 0.5f));
            DrawRectangleLines(30, 400, 325, 30, Fade(Color.White, 0.5f));
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
                    DrawText("DIAGONAL GRADIENT", 540, 10, 20, Color.RayWhite);
                    break;
                case 3:
                    DrawText("RADIAL GRADIENT", 580, 10, 20, Color.LightGray);
                    break;
                case 4:
                    DrawText("SQUARE GRADIENT", 580, 10, 20, Color.LightGray);
                    break;
                case 5:
                    DrawText("CHECKED", 680, 10, 20, Color.RayWhite);
                    break;
                case 6:
                    DrawText("WHITE NOISE", 640, 10, 20, Color.Red);
                    break;
                case 7:
                    DrawText("PERLIN NOISE", 640, 10, 20, Color.Red);
                    break;
                case 8:
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
}
#endif
