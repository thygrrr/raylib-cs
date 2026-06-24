// Adapted for the browser from Examples/Models/LoadingGltf.cs
namespace Examples.Web;

public unsafe class ModelsLoadingGltf : IWebExample
{
    public string Name => "Models / Loading GLTF";

    private Camera3D _camera;

    private Model _model;
    private Vector3 _position;

    // Load animation data
    private ModelAnimation* _anims;
    private int _animCount;

    // Animation playing variables
    private int _animIndex;
    private float _animCurrentFrame;

    public void Init()
    {
        // Define the camera to look into our 3d world
        _camera = new();
        _camera.Position = new Vector3(6.0f, 6.0f, 6.0f);
        _camera.Target = new Vector3(0.0f, 2.0f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = 45.0f;
        _camera.Projection = CameraProjection.Perspective;

        _model = LoadModel("resources/models/gltf/robot.glb");
        _position = new(0.0f, 0.0f, 0.0f);

        // Load animation data
        int count = 0;
        _anims = LoadModelAnimations("resources/models/gltf/robot.glb", ref count);
        _animCount = count;

        // Animation playing variables
        _animIndex = 0;
        _animCurrentFrame = 0.0f;
    }

    public void Update()
    {
        // Update
        UpdateCamera(ref _camera, CameraMode.Orbital);

        if (IsKeyPressed(KeyboardKey.Right))
        {
            _animIndex = (_animIndex + 1) % _animCount;
        }
        else if (IsKeyPressed(KeyboardKey.Left))
        {
            _animIndex = (_animIndex + _animCount - 1) % _animCount;
        }

        // Update model animation
        _animCurrentFrame = (_animCurrentFrame + 1) % _anims[_animIndex].KeyFrameCount;
        UpdateModelAnimation(_model, _anims[_animIndex], (float)_animCurrentFrame);

        // Draw
        BeginDrawing();
        ClearBackground(Color.RayWhite);

        BeginMode3D(_camera);

        DrawModel(
            _model,
            _position,
            1f,
            Color.White
        );

        DrawGrid(10, 1.0f);

        EndMode3D();
        DrawText($"Current animation: {_anims[_animIndex].NameToString()}", 10, 40, 20, Color.Maroon);
        DrawText("Use the LEFT/RIGHT keys to switch animation", 10, 10, 20, Color.Gray);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadModelAnimations(new Span<ModelAnimation>(_anims, _animCount));
        UnloadModel(_model);
    }
}
