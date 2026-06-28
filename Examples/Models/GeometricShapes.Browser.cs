#if BROWSER
using Examples;
namespace Examples.Models;

public partial class GeometricShapes : IExample
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
        public string Name => "Models / Geometric Shapes";

        private Camera3D _camera;

        public void Init()
        {
            // Define the camera to look into our 3d world
            _camera = new();
            _camera.Position = new Vector3(0.0f, 10.0f, 10.0f);
            _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 45.0f;
            _camera.Projection = CameraProjection.Perspective;
        }

        public void Update()
        {
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);

            DrawCube(new Vector3(-4.0f, 0.0f, 2.0f), 2.0f, 5.0f, 2.0f, Color.Red);
            DrawCubeWires(new Vector3(-4.0f, 0.0f, 2.0f), 2.0f, 5.0f, 2.0f, Color.Gold);
            DrawCubeWires(new Vector3(-4.0f, 0.0f, -2.0f), 3.0f, 6.0f, 2.0f, Color.Maroon);

            DrawSphere(new Vector3(-1.0f, 0.0f, -2.0f), 1.0f, Color.Green);
            DrawSphereWires(new Vector3(1.0f, 0.0f, 2.0f), 2.0f, 16, 16, Color.Lime);

            DrawCylinder(new Vector3(4.0f, 0.0f, -2.0f), 1.0f, 2.0f, 3.0f, 4, Color.SkyBlue);
            DrawCylinderWires(new Vector3(4.0f, 0.0f, -2.0f), 1.0f, 2.0f, 3.0f, 4, Color.DarkBlue);
            DrawCylinderWires(new Vector3(4.5f, -1.0f, 2.0f), 1.0f, 1.0f, 2.0f, 6, Color.Brown);

            DrawCylinder(new Vector3(1.0f, 0.0f, -4.0f), 0.0f, 1.5f, 3.0f, 8, Color.Gold);
            DrawCylinderWires(new Vector3(1.0f, 0.0f, -4.0f), 0.0f, 1.5f, 3.0f, 8, Color.Pink);

            DrawCapsule(new Vector3(-3.0f, 1.5f, -4.0f), new Vector3(-4.0f, -1.0f, -4.0f), 1.2f, 8, 8, Color.Violet);
            DrawCapsuleWires(new Vector3(-3.0f, 1.5f, -4.0f), new Vector3(-4.0f, -1.0f, -4.0f), 1.2f, 8, 8, Color.Purple);

            DrawGrid(10, 1.0f);

            EndMode3D();

            DrawFPS(10, 10);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
