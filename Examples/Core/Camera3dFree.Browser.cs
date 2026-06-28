#if BROWSER
using Examples;
namespace Examples.Core;

public partial class Camera3dFree : IExample
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
        public string Name => "Core / Camera 3D Free";

        private Camera3D _camera;
        private Vector3 _cubePosition;

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

            DisableCursor();
        }

        public void Update()
        {
            UpdateCamera(ref _camera, CameraMode.Free);

            if (IsKeyPressed(KeyboardKey.Z))
            {
                _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);

            DrawCube(_cubePosition, 2.0f, 2.0f, 2.0f, Color.Red);
            DrawCubeWires(_cubePosition, 2.0f, 2.0f, 2.0f, Color.Maroon);

            DrawGrid(10, 1.0f);

            EndMode3D();

            DrawRectangle(10, 10, 320, 93, Fade(Color.SkyBlue, 0.5f));
            DrawRectangleLines(10, 10, 320, 93, Color.Blue);

            DrawText("Free camera default controls:", 20, 20, 10, Color.Black);
            DrawText("- Mouse Wheel to Zoom in-out", 40, 40, 10, Color.DarkGray);
            DrawText("- Mouse Wheel Pressed to Pan", 40, 60, 10, Color.DarkGray);
            DrawText("- Z to zoom to (0, 0, 0)", 40, 80, 10, Color.DarkGray);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
