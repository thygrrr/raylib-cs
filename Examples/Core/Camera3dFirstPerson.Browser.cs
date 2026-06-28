#if BROWSER
using Examples;
namespace Examples.Core;

public partial class Camera3dFirstPerson : IExample
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
        public string Name => "Core / Camera 3D First Person";

        public const int MaxColumns = 20;

        private Camera3D _camera;
        private float[] _heights;
        private Vector3[] _positions;
        private Color[] _colors;
        private CameraMode _cameraMode;

        public void Init()
        {
            // Define the camera to look into our 3d world (position, target, up vector)
            _camera = new Camera3D();
            _camera.Position = new Vector3(0.0f, 2.0f, 4.0f);
            _camera.Target = new Vector3(0.0f, 2.0f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 60.0f;
            _camera.Projection = CameraProjection.Perspective;

            _cameraMode = CameraMode.FirstPerson;

            // Generates some random columns
            _heights = new float[MaxColumns];
            _positions = new Vector3[MaxColumns];
            _colors = new Color[MaxColumns];

            for (int i = 0; i < MaxColumns; i++)
            {
                _heights[i] = (float)GetRandomValue(1, 12);
                _positions[i] = new Vector3(GetRandomValue(-15, 15), _heights[i] / 2.0f, GetRandomValue(-15, 15));
                _colors[i] = new Color(GetRandomValue(20, 255), GetRandomValue(10, 55), 30, 255);
            }

            DisableCursor();
        }

        public void Update()
        {
            // Switch camera mode
            if (IsKeyPressed(KeyboardKey.One))
            {
                _cameraMode = CameraMode.Free;
                _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            }

            if (IsKeyPressed(KeyboardKey.Two))
            {
                _cameraMode = CameraMode.FirstPerson;
                _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            }

            if (IsKeyPressed(KeyboardKey.Three))
            {
                _cameraMode = CameraMode.ThirdPerson;
                _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            }

            if (IsKeyPressed(KeyboardKey.Four))
            {
                _cameraMode = CameraMode.Orbital;
                _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            }

            // Switch camera projection
            if (IsKeyPressed(KeyboardKey.P))
            {
                if (_camera.Projection == CameraProjection.Perspective)
                {
                    // Create isometric view
                    _cameraMode = CameraMode.ThirdPerson;
                    // Note: The target distance is related to the render distance in the orthographic projection
                    _camera.Position = new Vector3(0.0f, 2.0f, -100.0f);
                    _camera.Target = new Vector3(0.0f, 2.0f, 0.0f);
                    _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
                    _camera.Projection = CameraProjection.Orthographic;
                    _camera.FovY = 20.0f; // near plane width in CAMERA_ORTHOGRAPHIC
                    // CameraYaw(&camera, -135 * DEG2RAD, true);
                    // CameraPitch(&camera, -45 * DEG2RAD, true, true, false);
                }
                else if (_camera.Projection == CameraProjection.Orthographic)
                {
                    // Reset to default view
                    _cameraMode = CameraMode.ThirdPerson;
                    _camera.Position = new Vector3(0.0f, 2.0f, 10.0f);
                    _camera.Target = new Vector3(0.0f, 2.0f, 0.0f);
                    _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
                    _camera.Projection = CameraProjection.Perspective;
                    _camera.FovY = 60.0f;
                }
            }

            // Update camera computes movement internally depending on the camera mode
            // Some default standard keyboard/mouse inputs are hardcoded to simplify use
            // For advanced camera controls, it's recommended to compute camera movement manually
            UpdateCamera(ref _camera, _cameraMode);

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);

            // Draw ground
            DrawPlane(new Vector3(0.0f, 0.0f, 0.0f), new Vector2(32.0f, 32.0f), Color.LightGray);

            // Draw a blue wall
            DrawCube(new Vector3(-16.0f, 2.5f, 0.0f), 1.0f, 5.0f, 32.0f, Color.Blue);

            // Draw a green wall
            DrawCube(new Vector3(16.0f, 2.5f, 0.0f), 1.0f, 5.0f, 32.0f, Color.Lime);

            // Draw a yellow wall
            DrawCube(new Vector3(0.0f, 2.5f, 16.0f), 32.0f, 5.0f, 1.0f, Color.Gold);

            // Draw some cubes around
            for (int i = 0; i < MaxColumns; i++)
            {
                DrawCube(_positions[i], 2.0f, _heights[i], 2.0f, _colors[i]);
                DrawCubeWires(_positions[i], 2.0f, _heights[i], 2.0f, Color.Maroon);
            }

            // Draw player cube
            if (_cameraMode == CameraMode.ThirdPerson)
            {
                DrawCube(_camera.Target, 0.5f, 0.5f, 0.5f, Color.Purple);
                DrawCubeWires(_camera.Target, 0.5f, 0.5f, 0.5f, Color.DarkPurple);
            }

            EndMode3D();

            // Draw info boxes
            DrawRectangle(5, 5, 330, 100, Fade(Color.SkyBlue, 0.5f));
            DrawRectangleLines(5, 5, 330, 100, Color.Blue);

            DrawText("Camera controls:", 15, 15, 10, Color.Black);
            DrawText("- Move keys: W, A, S, D, Space, Left-Ctrl", 15, 30, 10, Color.Black);
            DrawText("- Look around: arrow keys or mouse", 15, 45, 10, Color.Black);
            DrawText("- Camera mode keys: 1, 2, 3, 4", 15, 60, 10, Color.Black);
            DrawText("- Zoom keys: num-plus, num-minus or mouse scroll", 15, 75, 10, Color.Black);
            DrawText("- Camera projection key: P", 15, 90, 10, Color.Black);

            DrawRectangle(600, 5, 195, 100, Fade(Color.SkyBlue, 0.5f));
            DrawRectangleLines(600, 5, 195, 100, Color.Blue);

            DrawText("Camera status:", 610, 15, 10, Color.Black);
            DrawText($"- Mode: {_cameraMode}", 610, 30, 10, Color.Black);
            DrawText($"- Projection: {_camera.Projection}", 610, 45, 10, Color.Black);
            DrawText($"- Position: {_camera.Position}", 610, 60, 10, Color.Black);
            DrawText($"- Target: {_camera.Target}", 610, 75, 10, Color.Black);
            DrawText($"- Up: {_camera.Up}", 610, 90, 10, Color.Black);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
