// Adapted for the browser from Examples/Core/Camera3dMode.cs
namespace Examples.Web;

public class CoreCamera3dMode : IWebExample
{
    public string Name => "Core / Camera 3D Mode";

    private Camera3D _camera;
    private Vector3 _cubePosition;

    public void Init()
    {
        // Define the camera to look into our 3d world
        _camera = new Camera3D();
        _camera.Position = new Vector3(0.0f, 10.0f, 10.0f);
        _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = 45.0f;
        _camera.Projection = CameraProjection.Perspective;

        _cubePosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void Update()
    {
        // TODO: Update your variables here

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        BeginMode3D(_camera);

        DrawCube(_cubePosition, 2.0f, 2.0f, 2.0f, Color.Red);
        DrawCubeWires(_cubePosition, 2.0f, 2.0f, 2.0f, Color.Maroon);

        DrawGrid(10, 1.0f);

        EndMode3D();

        DrawText("Welcome to the third dimension!", 10, 40, 20, Color.DarkGray);

        DrawFPS(10, 10);

        EndDrawing();
    }

    public void Unload()
    {
    }
}
