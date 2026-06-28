#if BROWSER
using Examples;
namespace Examples.Core;

public partial class WorldScreen : IExample
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
        public string Name => "Core / World to Screen";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Camera3D _camera;
        private Vector3 _cubePosition;
        private Vector2 _cubeScreenPosition;

        public void Init()
        {
            // Define the camera to look into our 3d world
            _camera = new Camera3D();
            _camera.Position = new Vector3(10.0f, 10.0f, 10.0f);
            _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 45.0f;
            _camera.Projection = CameraProjection.Perspective;

            _cubePosition = new Vector3(0.0f, 0.0f, 0.0f);
        }

        public void Update()
        {
            UpdateCamera(ref _camera, CameraMode.Free);

            // Calculate cube screen space position (with a little offset to be in top)
            _cubeScreenPosition = GetWorldToScreen(
                new Vector3(_cubePosition.X, _cubePosition.Y + 2.5f, _cubePosition.Z),
                _camera
            );

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);

            DrawCube(_cubePosition, 2.0f, 2.0f, 2.0f, Color.Red);
            DrawCubeWires(_cubePosition, 2.0f, 2.0f, 2.0f, Color.Maroon);

            DrawGrid(10, 1.0f);

            EndMode3D();

            DrawText(
                "Enemy: 100 / 100",
                (int)_cubeScreenPosition.X - MeasureText("Enemy: 100 / 100", 20) / 2,
                (int)_cubeScreenPosition.Y,
                20,
                Color.Black
            );
            DrawText(
                "Text is always on top of the cube",
                (screenWidth - MeasureText("Text is always on top of the cube", 20)) / 2,
                25,
                20,
                Color.Gray
            );

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
