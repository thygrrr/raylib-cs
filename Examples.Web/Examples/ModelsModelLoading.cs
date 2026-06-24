// Adapted for the browser from Examples/Models/ModelLoading.cs
namespace Examples.Web;

public unsafe class ModelsModelLoading : IWebExample
{
    public string Name => "Models / Model Loading";

    private const int screenWidth = 960;
    private const int screenHeight = 540;

    private Camera3D _camera;

    private Model _model;
    private Texture2D _texture;
    private Vector3 _position;
    private BoundingBox _bounds;
    private bool _selected;

    public void Init()
    {
        // Define the camera to look into our 3d world
        _camera = new();
        _camera.Position = new Vector3(50.0f, 50.0f, 50.0f);
        _camera.Target = new Vector3(0.0f, 10.0f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = 45.0f;
        _camera.Projection = CameraProjection.Perspective;

        _model = LoadModel("resources/models/obj/castle.obj");
        _texture = LoadTexture("resources/models/obj/castle_diffuse.png");

        // Set map diffuse texture
        Raylib.SetMaterialTexture(ref _model, 0, MaterialMapIndex.Albedo, ref _texture);

        _position = new(0.0f, 0.0f, 0.0f);
        _bounds = GetMeshBoundingBox(_model.Meshes[0]);

        // NOTE: bounds are calculated from the original size of the model,
        // if model is scaled on drawing, bounds must be also scaled

        _selected = false;
    }

    public void Update()
    {
        // Update
        UpdateCamera(ref _camera, CameraMode.Orbital);

        // NOTE: Drag & drop file loading (IsFileDropped) is not available in the browser
        // host, so it is skipped here. The default model stays loaded.

        // Select model on mouse click
        if (IsMouseButtonPressed(MouseButton.Left))
        {
            // Check collision between ray and box
            if (GetRayCollisionBox(GetScreenToWorldRay(GetMousePosition(), _camera), _bounds).Hit)
            {
                _selected = !_selected;
            }
            else
            {
                _selected = false;
            }
        }

        // Draw
        BeginDrawing();
        ClearBackground(Color.RayWhite);

        BeginMode3D(_camera);

        DrawModel(_model, _position, 1.0f, Color.White);

        DrawGrid(20, 10.0f);

        if (_selected)
        {
            DrawBoundingBox(_bounds, Color.Green);
        }

        EndMode3D();

        DrawText("Drag & drop model to load mesh/texture.", 10, GetScreenHeight() - 20, 10, Color.DarkGray);
        if (_selected)
        {
            DrawText("MODEL SELECTED", GetScreenWidth() - 110, 10, 10, Color.Green);
        }

        DrawText("(c) Castle 3D model by Alberto Cano", screenWidth - 200, screenHeight - 20, 10, Color.Gray);

        DrawFPS(10, 10);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadTexture(_texture);
        UnloadModel(_model);
    }
}
