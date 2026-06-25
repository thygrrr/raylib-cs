// Adapted for the browser from Examples/Models/LoadingIqm.cs
namespace Examples.Web;

public unsafe class ModelsLoadingIqm : IWebExample
{
    public string Name => "Models / Loading IQM";

    private const int screenWidth = 800;
    private const int screenHeight = 450;

    private Camera3D _camera;

    private Model _model;
    private Texture2D _texture;
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
        _camera.Position = new Vector3(10.0f, 10.0f, 10.0f);
        _camera.Target = new Vector3(0.0f, 4.0f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = 45.0f;
        _camera.Projection = CameraProjection.Perspective;

        _model = LoadModel("resources/models/iqm/guy.iqm");
        _texture = LoadTexture("resources/models/iqm/guytex.png");
        Raylib.SetMaterialTexture(ref _model, 0, MaterialMapIndex.Diffuse, ref _texture);
        _position = new(0.0f, 0.0f, 0.0f);

        // Load animation data
        int count = 0;
        _anims = LoadModelAnimations("resources/models/iqm/guyanim.iqm", ref count);
        _animCount = count;

        // Animation playing variables
        _animIndex = 0;
        _animCurrentFrame = 0.0f;
    }

    public void Update()
    {
        // Update
        UpdateCamera(ref _camera, CameraMode.Orbital);

        // Play animation when spacebar is held down
        _animCurrentFrame += 1.0f;
        UpdateModelAnimation(_model, _anims[0], _animCurrentFrame);

        if (_animCurrentFrame >= _anims[0].KeyFrameCount)
        {
            _animCurrentFrame = 0;
        }

        // Draw
        BeginDrawing();
        ClearBackground(Color.RayWhite);

        BeginMode3D(_camera);

        DrawModelEx(
            _model,
            _position,
            new Vector3(1.0f, 0.0f, 0.0f),
            -90.0f,
            new Vector3(1.0f, 1.0f, 1.0f),
            Color.White
        );

        DrawGrid(10, 1.0f);

        EndMode3D();
        DrawText($"Current animation: {_anims[_animIndex].NameToString()}", 10, 10, 20, Color.Maroon);
        DrawText("(c) Guy IQM 3D model by @culacant", screenWidth - 200, screenHeight - 20, 10, Color.Gray);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadTexture(_texture);
        UnloadModelAnimations(new Span<ModelAnimation>(_anims, _animCount));
        UnloadModel(_model);
    }
}
