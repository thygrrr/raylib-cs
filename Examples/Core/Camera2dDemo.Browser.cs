#if BROWSER
using Examples;
namespace Examples.Core;

public partial class Camera2dDemo : IExample
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
        public string Name => "Core / Camera 2D Demo";

        public const int MaxBuildings = 100;

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Rectangle _player;
        private Rectangle[] _buildings;
        private Color[] _buildColors;
        private Camera2D _camera;

        public void Init()
        {
            _player = new Rectangle(400, 280, 40, 40);
            _buildings = new Rectangle[MaxBuildings];
            _buildColors = new Color[MaxBuildings];

            int spacing = 0;

            for (int i = 0; i < MaxBuildings; i++)
            {
                _buildings[i].Width = GetRandomValue(50, 200);
                _buildings[i].Height = GetRandomValue(100, 800);
                _buildings[i].Y = screenHeight - 130 - _buildings[i].Height;
                _buildings[i].X = -6000 + spacing;

                spacing += (int)_buildings[i].Width;

                _buildColors[i] = new Color(
                    GetRandomValue(200, 240),
                    GetRandomValue(200, 240),
                    GetRandomValue(200, 250),
                    255
                );
            }

            _camera = new Camera2D();
            _camera.Target = new Vector2(_player.X + 20, _player.Y + 20);
            _camera.Offset = new Vector2(screenWidth / 2.0f, screenHeight / 2.0f);
            _camera.Rotation = 0.0f;
            _camera.Zoom = 1.0f;
        }

        public void Update()
        {
            // Player movement
            if (IsKeyDown(KeyboardKey.Right))
            {
                _player.X += 2;
            }
            else if (IsKeyDown(KeyboardKey.Left))
            {
                _player.X -= 2;
            }

            // Camera target follows player
            _camera.Target = new Vector2(_player.X + 20, _player.Y + 20);

            // Camera rotation controls
            if (IsKeyDown(KeyboardKey.A))
            {
                _camera.Rotation--;
            }
            else if (IsKeyDown(KeyboardKey.S))
            {
                _camera.Rotation++;
            }

            // Limit camera rotation to 80 degrees (-40 to 40)
            if (_camera.Rotation > 40)
            {
                _camera.Rotation = 40;
            }
            else if (_camera.Rotation < -40)
            {
                _camera.Rotation = -40;
            }

            // Camera zoom controls
            // Uses log scaling to provide consistent zoom speed
            _camera.Zoom = MathF.Exp(MathF.Log(_camera.Zoom) + ((float)GetMouseWheelMove() * 0.1f));

            if (_camera.Zoom > 3.0f)
            {
                _camera.Zoom = 3.0f;
            }
            else if (_camera.Zoom < 0.1f)
            {
                _camera.Zoom = 0.1f;
            }

            // Camera reset (zoom and rotation)
            if (IsKeyPressed(KeyboardKey.R))
            {
                _camera.Zoom = 1.0f;
                _camera.Rotation = 0.0f;
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode2D(_camera);

            DrawRectangle(-6000, 320, 13000, 8000, Color.DarkGray);

            for (int i = 0; i < MaxBuildings; i++)
            {
                DrawRectangleRec(_buildings[i], _buildColors[i]);
            }

            DrawRectangleRec(_player, Color.Red);

            DrawLine((int)_camera.Target.X, -screenHeight * 10, (int)_camera.Target.X, screenHeight * 10, Color.Green);
            DrawLine(-screenWidth * 10, (int)_camera.Target.Y, screenWidth * 10, (int)_camera.Target.Y, Color.Green);

            EndMode2D();

            DrawText("SCREEN AREA", 640, 10, 20, Color.Red);

            DrawRectangle(0, 0, screenWidth, 5, Color.Red);
            DrawRectangle(0, 5, 5, screenHeight - 10, Color.Red);
            DrawRectangle(screenWidth - 5, 5, 5, screenHeight - 10, Color.Red);
            DrawRectangle(0, screenHeight - 5, screenWidth, 5, Color.Red);

            DrawRectangle(10, 10, 250, 113, Fade(Color.SkyBlue, 0.5f));
            DrawRectangleLines(10, 10, 250, 113, Color.Blue);

            DrawText("Free 2D camera controls:", 20, 20, 10, Color.Black);
            DrawText("- Right/Left to move player", 40, 40, 10, Color.DarkGray);
            DrawText("- Mouse Wheel to Zoom in-out", 40, 60, 10, Color.DarkGray);
            DrawText("- A / S to Rotate", 40, 80, 10, Color.DarkGray);
            DrawText("- R to reset Zoom and Rotation", 40, 100, 10, Color.DarkGray);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
