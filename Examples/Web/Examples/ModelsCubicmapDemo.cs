// Adapted for the browser from Examples/Models/CubicmapDemo.cs
namespace Examples.Web;

public class ModelsCubicmapDemo : IWebExample
{
    public string Name => "Models / Cubicmap Demo";

    private const int screenWidth = 800;
    private const int screenHeight = 450;

    private Camera3D _camera;

    private Texture2D _cubicmap;
    private Texture2D _texture;
    private Model _model;
    private Vector3 _mapPosition;

    public void Init()
    {
        // Define the camera to look into our 3d world
        _camera = new();
        _camera.Position = new Vector3(16.0f, 14.0f, 16.0f);
        _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = 45.0f;
        _camera.Projection = CameraProjection.Perspective;

        Image image = LoadImage("resources/cubicmap.png");
        _cubicmap = LoadTextureFromImage(image);

        Mesh mesh = GenMeshCubicmap(image, new Vector3(1.0f, 1.0f, 1.0f));
        _model = LoadModelFromMesh(mesh);

        // NOTE: By default each cube is mapped to one part of texture atlas
        _texture = LoadTexture("resources/cubicmap_atlas.png");

        // Set map diffuse texture
        Raylib.SetMaterialTexture(ref _model, 0, MaterialMapIndex.Albedo, ref _texture);

        _mapPosition = new(-16.0f, 0.0f, -8.0f);
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

        DrawModel(_model, _mapPosition, 1.0f, Color.White);

        EndMode3D();

        Vector2 position = new(screenWidth - _cubicmap.Width * 4 - 20, 20);
        DrawTextureEx(_cubicmap, position, 0.0f, 4.0f, Color.White);
        DrawRectangleLines(
            screenWidth - _cubicmap.Width * 4 - 20,
            20,
            _cubicmap.Width * 4,
            _cubicmap.Height * 4,
            Color.Green
        );

        DrawText("cubicmap image used to", 658, 90, 10, Color.Gray);
        DrawText("generate map 3d model", 658, 104, 10, Color.Gray);

        DrawFPS(10, 10);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadTexture(_cubicmap);
        UnloadTexture(_texture);
        UnloadModel(_model);
    }
}
