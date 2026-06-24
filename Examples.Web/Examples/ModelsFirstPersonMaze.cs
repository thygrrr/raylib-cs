// Adapted for the browser from Examples/Models/FirstPersonMaze.cs
namespace Examples.Web;

public unsafe class ModelsFirstPersonMaze : IWebExample
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
        _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
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

        // Collision radius (player is modelled as a cilinder for collision)
        float playerRadius = 0.1f;

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

        // Check map collisions using image data and player position
        // TODO: Improvement: Just check player surrounding cells for collision
        for (int y = 0; y < _cubicmap.Height; y++)
        {
            for (int x = 0; x < _cubicmap.Width; x++)
            {
                Color* mapPixelsData = _mapPixels;

                // Collision: Color.white pixel, only check R channel
                Rectangle rec = new(
                    _mapPosition.X - 0.5f + x * 1.0f,
                    _mapPosition.Z - 0.5f + y * 1.0f,
                    1.0f,
                    1.0f
                );

                bool collision = CheckCollisionCircleRec(playerPos, playerRadius, rec);
                if ((mapPixelsData[y * _cubicmap.Width + x].R == 255) && collision)
                {
                    // Collision detected, reset camera position
                    _camera.Position = oldCamPos;
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
