#if BROWSER
using Examples;
namespace Examples.Core;

public partial class SplitScreen : IExample
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
        public string Name => "Core / Split Screen";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Texture2D _textureGrid;
        private Camera3D _cameraPlayer1;
        private Camera3D _cameraPlayer2;

        private RenderTexture2D _screenPlayer1;
        private RenderTexture2D _screenPlayer2;
        private Rectangle _splitScreenRect;

        // Scene drawing
        private void DrawScene()
        {
            int count = 5;
            float spacing = 4;

            // Grid of cube trees on a plane to make a "world"
            // Simple world plane
            DrawPlane(new Vector3(0, 0, 0), new Vector2(50, 50), Color.Beige);

            for (float x = -count * spacing; x <= count * spacing; x += spacing)
            {
                for (float z = -count * spacing; z <= count * spacing; z += spacing)
                {
                    DrawCube(new Vector3(x, 1.5f, z), 1, 1, 1, Color.Lime);
                    DrawCube(new Vector3(x, 0.5f, z), 0.25f, 1, 0.25f, Color.Brown);
                }
            }

            // Draw a cube at each player's position
            DrawCube(_cameraPlayer1.Position, 1, 1, 1, Color.Red);
            DrawCube(_cameraPlayer2.Position, 1, 1, 1, Color.Blue);
        }

        public void Init()
        {
            // Generate a simple texture to use for trees
            Image img = GenImageChecked(256, 256, 32, 32, Color.DarkGray, Color.White);
            _textureGrid = LoadTextureFromImage(img);
            UnloadImage(img);
            SetTextureFilter(_textureGrid, TextureFilter.Anisotropic16X);
            SetTextureWrap(_textureGrid, TextureWrap.Clamp);

            // Setup player 1 camera and screen
            _cameraPlayer1.FovY = 45.0f;
            _cameraPlayer1.Up.Y = 1.0f;
            _cameraPlayer1.Target.Y = 1.0f;
            _cameraPlayer1.Position.Z = -3.0f;
            _cameraPlayer1.Position.Y = 1.0f;

            _screenPlayer1 = LoadRenderTexture(screenWidth / 2, screenHeight);

            // Setup player two camera and screen
            _cameraPlayer2.FovY = 45.0f;
            _cameraPlayer2.Up.Y = 1.0f;
            _cameraPlayer2.Target.Y = 3.0f;
            _cameraPlayer2.Position.X = -3.0f;
            _cameraPlayer2.Position.Y = 3.0f;

            _screenPlayer2 = LoadRenderTexture(screenWidth / 2, screenHeight);

            // Build a flipped rectangle the size of the split view to use for drawing later
            _splitScreenRect = new(
                0.0f,
                0.0f,
                (float)_screenPlayer1.Texture.Width,
                (float)-_screenPlayer1.Texture.Height
            );
        }

        public void Update()
        {
            // If anyone moves this frame, how far will they move based on the time since the last frame
            // this moves things at 10 world units per second, regardless of the actual FPS
            float offsetThisFrame = 10.0f * GetFrameTime();

            // Move Player1 forward and backwards (no turning)
            if (IsKeyDown(KeyboardKey.W))
            {
                _cameraPlayer1.Position.Z += offsetThisFrame;
                _cameraPlayer1.Target.Z += offsetThisFrame;
            }
            else if (IsKeyDown(KeyboardKey.S))
            {
                _cameraPlayer1.Position.Z -= offsetThisFrame;
                _cameraPlayer1.Target.Z -= offsetThisFrame;
            }

            // Move Player2 forward and backwards (no turning)
            if (IsKeyDown(KeyboardKey.Up))
            {
                _cameraPlayer2.Position.X += offsetThisFrame;
                _cameraPlayer2.Target.X += offsetThisFrame;
            }
            else if (IsKeyDown(KeyboardKey.Down))
            {
                _cameraPlayer2.Position.X -= offsetThisFrame;
                _cameraPlayer2.Target.X -= offsetThisFrame;
            }

            // Draw Player1 view to the render texture
            BeginTextureMode(_screenPlayer1);
            ClearBackground(Color.SkyBlue);

            BeginMode3D(_cameraPlayer1);
            DrawScene();
            EndMode3D();

            DrawText("PLAYER 1 W/S to move", 10, 10, 20, Color.Red);
            EndTextureMode();

            // Draw Player2 view to the render texture
            BeginTextureMode(_screenPlayer2);
            ClearBackground(Color.SkyBlue);

            BeginMode3D(_cameraPlayer2);
            DrawScene();
            EndMode3D();

            DrawText("PLAYER 2 UP/DOWN to move", 10, 10, 20, Color.Blue);
            EndTextureMode();

            // Draw both views render textures to the screen side by side
            BeginDrawing();
            ClearBackground(Color.Black);

            DrawTextureRec(_screenPlayer1.Texture, _splitScreenRect, new Vector2(0, 0), Color.White);
            DrawTextureRec(_screenPlayer2.Texture, _splitScreenRect, new Vector2(screenWidth / 2.0f, 0), Color.White);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadRenderTexture(_screenPlayer1);
            UnloadRenderTexture(_screenPlayer2);
            UnloadTexture(_textureGrid);
        }
    }
}
#endif
