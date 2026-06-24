// Adapted for the browser from Examples/Models/SolarSystem.cs
using System;

namespace Examples.Web;

public class ModelsSolarSystem : IWebExample
{
    public string Name => "Models / Solar System";

    private const float SunRadius = 4.0f;
    private const float EarthRadius = 0.6f;
    private const float EarthOrbitRadius = 8.0f;
    private const float MoonRadius = 0.16f;
    private const float MoonOrbitRadius = 1.5f;

    private Camera3D _camera;

    // General system rotation speed
    private float _rotationSpeed;
    // Rotation of earth around itself (days) in degrees
    private float _earthRotation;
    // Rotation of earth around the Sun (years) in degrees
    private float _earthOrbitRotation;
    // Rotation of moon around itself
    private float _moonRotation;
    // Rotation of moon around earth in degrees
    private float _moonOrbitRotation;

    public void Init()
    {
        // Define the camera to look into our 3d world
        _camera = new();
        _camera.Position = new Vector3(16.0f, 16.0f, 16.0f);
        _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = 45.0f;
        _camera.Projection = CameraProjection.Perspective;

        _rotationSpeed = 0.2f;
        _earthRotation = 0.0f;
        _earthOrbitRotation = 0.0f;
        _moonRotation = 0.0f;
        _moonOrbitRotation = 0.0f;
    }

    public void Update()
    {
        UpdateCamera(ref _camera, CameraMode.Free);

        _earthRotation += (5.0f * _rotationSpeed);
        _earthOrbitRotation += (365 / 360.0f * (5.0f * _rotationSpeed) * _rotationSpeed);
        _moonRotation += (2.0f * _rotationSpeed);
        _moonOrbitRotation += (8.0f * _rotationSpeed);

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        BeginMode3D(_camera);

        Rlgl.PushMatrix();
        // Scale Sun
        Rlgl.Scalef(SunRadius, SunRadius, SunRadius);
        // Draw the Sun
        DrawSphereBasic(Color.Gold);
        Rlgl.PopMatrix();

        Rlgl.PushMatrix();
        // Rotation for Earth orbit around Sun
        Rlgl.Rotatef(_earthOrbitRotation, 0.0f, 1.0f, 0.0f);
        // Translation for Earth orbit
        Rlgl.Translatef(EarthOrbitRadius, 0.0f, 0.0f);
        // Rotation for Earth orbit around Sun inverted
        Rlgl.Rotatef(-_earthOrbitRotation, 0.0f, 1.0f, 0.0f);

        Rlgl.PushMatrix();
        // Rotation for Earth itself
        Rlgl.Rotatef(_earthRotation, 0.25f, 1.0f, 0.0f);
        // Scale Earth
        Rlgl.Scalef(EarthRadius, EarthRadius, EarthRadius);

        // Draw the Earth
        DrawSphereBasic(Color.Blue);
        Rlgl.PopMatrix();

        // Rotation for Moon orbit around Earth
        Rlgl.Rotatef(_moonOrbitRotation, 0.0f, 1.0f, 0.0f);
        // Translation for Moon orbit
        Rlgl.Translatef(MoonOrbitRadius, 0.0f, 0.0f);
        // Rotation for Moon orbit around Earth inverted
        Rlgl.Rotatef(-_moonOrbitRotation, 0.0f, 1.0f, 0.0f);
        // Rotation for Moon itself
        Rlgl.Rotatef(_moonRotation, 0.0f, 1.0f, 0.0f);
        // Scale Moon
        Rlgl.Scalef(MoonRadius, MoonRadius, MoonRadius);

        // Draw the Moon
        DrawSphereBasic(Color.LightGray);
        Rlgl.PopMatrix();

        // Some reference elements (not affected by previous matrix transformations)
        DrawCircle3D(
            new Vector3(0.0f, 0.0f, 0.0f),
            EarthOrbitRadius,
            new Vector3(1, 0, 0),
            90.0f,
            ColorAlpha(Color.Red, 0.5f)
        );
        DrawGrid(20, 1.0f);

        EndMode3D();

        DrawText("EARTH ORBITING AROUND THE SUN!", 400, 10, 20, Color.Maroon);
        DrawFPS(10, 10);

        EndDrawing();
    }

    public void Unload()
    {
    }

    // Draw sphere without any matrix transformation
    // NOTE: Sphere is drawn in world position ( 0, 0, 0 ) with radius 1.0f
    private static void DrawSphereBasic(Color color)
    {
        int rings = 16;
        int slices = 16;

        Rlgl.Begin(DrawMode.Triangles);
        Rlgl.Color4ub(color.R, color.G, color.B, color.A);

        for (int i = 0; i < (rings + 2); i++)
        {
            for (int j = 0; j < slices; j++)
            {
                Rlgl.Vertex3f(
                    MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * i)) * MathF.Sin(DEG2RAD * (j * 360 / slices)),
                    MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * i)),
                    MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * i)) * MathF.Cos(DEG2RAD * (j * 360 / slices))
                );
                Rlgl.Vertex3f(
                    MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Sin(DEG2RAD * ((j + 1) * 360 / slices)),
                    MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))),
                    MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Cos(DEG2RAD * ((j + 1) * 360 / slices))
                );
                Rlgl.Vertex3f(
                    MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Sin(DEG2RAD * (j * 360 / slices)),
                    MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))),
                    MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Cos(DEG2RAD * (j * 360 / slices))
                );

                Rlgl.Vertex3f(
                    MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * i)) * MathF.Sin(DEG2RAD * (j * 360 / slices)),
                    MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * i)),
                    MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * i)) * MathF.Cos(DEG2RAD * (j * 360 / slices))
                );
                Rlgl.Vertex3f(
                    MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i))) * MathF.Sin(DEG2RAD * ((j + 1) * 360 / slices)),
                    MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * (i))),
                    MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i))) * MathF.Cos(DEG2RAD * ((j + 1) * 360 / slices))
                );
                Rlgl.Vertex3f(
                    MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Sin(DEG2RAD * ((j + 1) * 360 / slices)),
                    MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))),
                    MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Cos(DEG2RAD * ((j + 1) * 360 / slices))
                );
            }
        }
        Rlgl.End();
    }
}
