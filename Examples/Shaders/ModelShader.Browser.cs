#if BROWSER
using Examples;
namespace Examples.Shaders;

public partial class ModelShader : IExample
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
        public string Name => "Shaders / Model Shader";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Camera3D _camera;
        private Model _model;
        private Texture2D _texture;
        private Shader _shader;
        private Vector3 _position;

        public void Init()
        {
            // Define the camera to look into our 3d world
            _camera = new();
            _camera.Position = new Vector3(4.0f, 4.0f, 4.0f);
            _camera.Target = new Vector3(0.0f, 1.0f, -1.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 45.0f;
            _camera.Projection = CameraProjection.Perspective;

            _model = LoadModel("resources/models/obj/watermill.obj");
            _texture = LoadTexture("resources/models/obj/watermill_diffuse.png");

            // Load shader for model
            // NOTE: Defining 0 (NULL) for vertex shader forces usage of internal default vertex shader
            _shader = LoadShader(null, "resources/shaders/glsl100/grayscale.fs");

            SetMaterialShader(ref _model, 0, ref _shader);
            SetMaterialTexture(ref _model, 0, MaterialMapIndex.Albedo, ref _texture);

            _position = new(0.0f, 0.0f, 0.0f);
        }

        public void Update()
        {
            UpdateCamera(ref _camera, CameraMode.Free);

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);

            DrawModel(_model, _position, 0.2f, Color.White);

            DrawGrid(10, 1.0f);

            EndMode3D();

            DrawText(
                "(c) Watermill 3D model by Alberto Cano",
                screenWidth - 210,
                screenHeight - 20,
                10,
                Color.Gray
            );

            DrawFPS(10, 10);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadShader(_shader);
            UnloadTexture(_texture);
            UnloadModel(_model);
        }
    }
}
#endif
