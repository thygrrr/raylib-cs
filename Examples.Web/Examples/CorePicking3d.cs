// Adapted for the browser from Examples/Core/Picking3D.cs
namespace Examples.Web;

public class CorePicking3d : IWebExample
{
    public string Name => "Core / Picking 3D";

    private const int ScreenWidth = 800;
    private const int ScreenHeight = 450;

    private Camera3D _camera;
    private Vector3 _cubePosition;
    private Vector3 _cubeSize;
    private Ray _ray;
    private RayCollision _collision;

    public void Init()
    {
        // Define the camera to look into our 3d world
        _camera = new Camera3D();
        _camera.Position = new Vector3(10.0f, 10.0f, 10.0f);
        _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = 45.0f;
        _camera.Projection = CameraProjection.Perspective;

        _cubePosition = new Vector3(0.0f, 1.0f, 0.0f);
        _cubeSize = new Vector3(2.0f, 2.0f, 2.0f);

        // Picking line ray
        _ray = new Ray(new Vector3(0.0f, 0.0f, 0.0f), Vector3.Zero);
        _collision = new RayCollision();
    }

    public void Update()
    {
        UpdateCamera(ref _camera, CameraMode.Free);

        if (IsMouseButtonPressed(MouseButton.Left))
        {
            if (!_collision.Hit)
            {
                _ray = GetScreenToWorldRay(GetMousePosition(), _camera);

                // Check collision between ray and box
                BoundingBox box = new(
                    _cubePosition - _cubeSize / 2,
                    _cubePosition + _cubeSize / 2
                );
                _collision = GetRayCollisionBox(_ray, box);
            }
            else
            {
                _collision.Hit = false;
            }

            _ray = GetScreenToWorldRay(GetMousePosition(), _camera);
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        BeginMode3D(_camera);

        if (_collision.Hit)
        {
            DrawCube(_cubePosition, _cubeSize.X, _cubeSize.Y, _cubeSize.Z, Color.Red);
            DrawCubeWires(_cubePosition, _cubeSize.X, _cubeSize.Y, _cubeSize.Z, Color.Maroon);

            DrawCubeWires(_cubePosition, _cubeSize.X + 0.2f, _cubeSize.Y + 0.2f, _cubeSize.Z + 0.2f, Color.Green);
        }
        else
        {
            DrawCube(_cubePosition, _cubeSize.X, _cubeSize.Y, _cubeSize.Z, Color.Gray);
            DrawCubeWires(_cubePosition, _cubeSize.X, _cubeSize.Y, _cubeSize.Z, Color.DarkGray);
        }

        DrawRay(_ray, Color.Maroon);
        DrawGrid(10, 1.0f);

        EndMode3D();

        DrawText("Try selecting the box with mouse!", 240, 10, 20, Color.DarkGray);

        if (_collision.Hit)
        {
            int posX = (ScreenWidth - MeasureText("BOX SELECTED", 30)) / 2;
            DrawText("BOX SELECTED", posX, (int)(ScreenHeight * 0.1f), 30, Color.Green);
        }

        DrawFPS(10, 10);

        EndDrawing();
    }

    public void Unload()
    {
    }
}
