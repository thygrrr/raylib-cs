#if BROWSER
using Examples;
using System;

namespace Examples.Models;

public partial class WavingCubes : IExample
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
        public string Name => "Models / Waving Cubes";

        // Specify the amount of blocks in each direction
        private const int NumBlocks = 15;

        private Camera3D _camera;

        public void Init()
        {
            // Initialize the camera
            _camera = new();
            _camera.Position = new Vector3(30.0f, 20.0f, 30.0f);
            _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 70.0f;
            _camera.Projection = CameraProjection.Perspective;
        }

        public void Update()
        {
            double time = GetTime();

            // Calculate time scale for cube position and size
            float scale = (2.0f + (float)Math.Sin(time)) * 0.7f;

            // Move camera around the scene
            double cameraTime = time * 0.3;
            _camera.Position.X = (float)Math.Cos(cameraTime) * 40.0f;
            _camera.Position.Z = (float)Math.Sin(cameraTime) * 40.0f;

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);

            DrawGrid(10, 5.0f);

            for (int x = 0; x < NumBlocks; x++)
            {
                for (int y = 0; y < NumBlocks; y++)
                {
                    for (int z = 0; z < NumBlocks; z++)
                    {
                        // Scale of the blocks depends on x/y/z positions
                        float blockScale = (x + y + z) / 30.0f;

                        // Scatter makes the waving effect by adding blockScale over time
                        float scatter = (float)Math.Sin(blockScale * 20.0f + (float)(time * 4.0f));

                        // Calculate the cube position
                        Vector3 cubePos = new(
                            (float)(x - NumBlocks / 2) * (scale * 3.0f) + scatter,
                            (float)(y - NumBlocks / 2) * (scale * 2.0f) + scatter,
                            (float)(z - NumBlocks / 2) * (scale * 3.0f) + scatter
                        );

                        // Pick a color with a hue depending on cube position for the rainbow color effect
                        Color cubeColor = ColorFromHSV((float)(((x + y + z) * 18) % 360), 0.75f, 0.9f);

                        // Calculate cube size
                        float cubeSize = (2.4f - scale) * blockScale;

                        // And finally, draw the cube!
                        DrawCube(cubePos, cubeSize, cubeSize, cubeSize, cubeColor);
                    }
                }
            }

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
