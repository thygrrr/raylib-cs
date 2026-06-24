// Adapted for the browser from Examples/Models/OrthographicProjection.cs
namespace Examples.Web;

public class ModelsOrthographicProjection : IWebExample
{
    public string Name => "Models / Orthographic Projection";

    private const float FovyPerspective = 45.0f;
    private const float WidthOrthographic = 10.0f;

    private Camera3D _camera;

    public void Init()
    {
        // Define the camera to look into our 3d world
        _camera = new();
        _camera.Position = new Vector3(0.0f, 10.0f, 10.0f);
        _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = FovyPerspective;
        _camera.Projection = CameraProjection.Perspective;
    }

    public void Update()
    {
        if (IsKeyPressed(KeyboardKey.Space))
        {
            if (_camera.Projection == CameraProjection.Perspective)
            {
                _camera.FovY = WidthOrthographic;
                _camera.Projection = CameraProjection.Orthographic;
            }
            else
            {
                _camera.FovY = FovyPerspective;
                _camera.Projection = CameraProjection.Perspective;
            }
        }

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

        DrawGrid(10, 1.0f);

        EndMode3D();

        DrawText("Press Spacebar to switch camera type", 10, GetScreenHeight() - 30, 20, Color.DarkGray);

        if (_camera.Projection == CameraProjection.Orthographic)
        {
            DrawText("ORTHOGRAPHIC", 10, 40, 20, Color.Black);
        }
        else if (_camera.Projection == CameraProjection.Perspective)
        {
            DrawText("PERSPECTIVE", 10, 40, 20, Color.Black);
        }

        DrawFPS(10, 10);

        EndDrawing();
    }

    public void Unload()
    {
    }
}
