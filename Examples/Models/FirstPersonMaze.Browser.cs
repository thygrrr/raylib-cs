#if BROWSER
using Examples;
namespace Examples.Models;

public partial class FirstPersonMaze : IExample
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

    private unsafe sealed class BrowserAdapter : IExample
    {
        public string Name => "Models / First Person Maze";

        private Camera3D _camera;

        private Texture2D _cubicmap;
        private Texture2D _texture;
        private Model _model;
        private Color* _mapPixels;
        private Vector3 _mapPosition;

        public void Init()
        {
            // Define the camera to look into our 3d world
            _camera = new();
            _camera.Position = new Vector3(0.2f, 0.4f, 0.2f);
            _camera.Target = new Vector3(0.185f, 0.4f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 45.0f;
            _camera.Projection = CameraProjection.Perspective;

            Image imMap = LoadImage("resources/cubicmap.png");
            _cubicmap = LoadTextureFromImage(imMap);
            Mesh mesh = GenMeshCubicmap(imMap, new Vector3(1.0f, 1.0f, 1.0f));
            _model = LoadModelFromMesh(mesh);

            // NOTE: By default each cube is mapped to one part of texture atlas
            _texture = LoadTexture("resources/cubicmap_atlas.png");

            // Set map diffuse texture
            Raylib.SetMaterialTexture(ref _model, 0, MaterialMapIndex.Albedo, ref _texture);

            // Get map image data to be used for collision detection
            _mapPixels = LoadImageColors(imMap);
            UnloadImage(imMap);

            _mapPosition = new(-16.0f, 0.0f, -8.0f);
        }

        public void Update()
        {
            // Update
            Vector3 oldCamPos = _camera.Position;

            UpdateCamera(ref _camera, CameraMode.FirstPerson);

            // Check player collision (we simplify to 2D collision detection)
            Vector2 playerPos = new(_camera.Position.X, _camera.Position.Z);
            float playerRadius = 0.1f;  // Collision radius (player is modelled as a cilinder for collision)

            int playerCellX = (int)(playerPos.X - _mapPosition.X + 0.5f);
            int playerCellY = (int)(playerPos.Y - _mapPosition.Z + 0.5f);

            // Out-of-limits security check
            if (playerCellX < 0)
            {
                playerCellX = 0;
            }
            else if (playerCellX >= _cubicmap.Width)
            {
                playerCellX = _cubicmap.Width - 1;
            }

            if (playerCellY < 0)
            {
                playerCellY = 0;
            }
            else if (playerCellY >= _cubicmap.Height)
            {
                playerCellY = _cubicmap.Height - 1;
            }

            // Check map collisions using image data and player position against surrounding cells only
            for (int y = playerCellY - 1; y <= playerCellY + 1; y++)
            {
                // Avoid map accessing out of bounds
                if ((y >= 0) && (y < _cubicmap.Height))
                {
                    for (int x = playerCellX - 1; x <= playerCellX + 1; x++)
                    {
                        // NOTE: Collision: Only checking R channel for white pixel
                        if (((x >= 0) && (x < _cubicmap.Width)) &&
                            (_mapPixels[y * _cubicmap.Width + x].R == 255) &&
                            (CheckCollisionCircleRec(playerPos, playerRadius,
                            new Rectangle(_mapPosition.X - 0.5f + x * 1.0f, _mapPosition.Z - 0.5f + y * 1.0f, 1.0f, 1.0f))))
                        {
                            // Collision detected, reset camera position
                            _camera.Position = oldCamPos;
                        }
                    }
                }
            }

            // Draw
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // Draw maze map
            BeginMode3D(_camera);
            DrawModel(_model, _mapPosition, 1.0f, Color.White);
            EndMode3D();

            DrawTextureEx(_cubicmap, new Vector2(GetScreenWidth() - _cubicmap.Width * 4 - 20, 20), 0.0f, 4.0f, Color.White);
            DrawRectangleLines(GetScreenWidth() - _cubicmap.Width * 4 - 20, 20, _cubicmap.Width * 4, _cubicmap.Height * 4, Color.Green);

            // Draw player position radar
            DrawRectangle(GetScreenWidth() - _cubicmap.Width * 4 - 20 + playerCellX * 4, 20 + playerCellY * 4, 4, 4, Color.Red);

            DrawFPS(10, 10);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadImageColors(_mapPixels);

            UnloadTexture(_cubicmap);
            UnloadTexture(_texture);
            UnloadModel(_model);
        }
    }
}
#endif
