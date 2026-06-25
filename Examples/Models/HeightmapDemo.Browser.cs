#if BROWSER
using Examples;
namespace Examples.Models;

public partial class HeightmapDemo : IExample
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
        public string Name => "Models / Heightmap Demo";

        private const int screenWidth = 800;

        private Camera3D _camera;

        private Texture2D _texture;
        private Model _model;
        private Vector3 _mapPosition;

        public void Init()
        {
            // Define our custom camera to look into our 3d world
            _camera = new();
            _camera.Position = new Vector3(18.0f, 16.0f, 18.0f);
            _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 45.0f;
            _camera.Projection = CameraProjection.Perspective;

            Image image = LoadImage("resources/heightmap.png");
            _texture = LoadTextureFromImage(image);

            Mesh mesh = GenMeshHeightmap(image, new Vector3(16, 8, 16));
            _model = LoadModelFromMesh(mesh);

            // Set map diffuse texture
            Raylib.SetMaterialTexture(ref _model, 0, MaterialMapIndex.Albedo, ref _texture);

            _mapPosition = new(-8.0f, 0.0f, -8.0f);

            UnloadImage(image);
        }

        public void Update()
        {
            // Update
            UpdateCamera(ref _camera, CameraMode.Orbital);

            // Draw
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);

            DrawModel(_model, _mapPosition, 1.0f, Color.Red);

            DrawGrid(20, 1.0f);

            EndMode3D();

            DrawTexture(_texture, screenWidth - _texture.Width - 20, 20, Color.White);
            DrawRectangleLines(screenWidth - _texture.Width - 20, 20, _texture.Width, _texture.Height, Color.Green);

            DrawFPS(10, 10);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_texture);
            UnloadModel(_model);
        }
    }
}
#endif
