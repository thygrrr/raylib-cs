/*******************************************************************************************
*
*   raylib [shaders] example - raymarching rendering
*
*   Example complexity rating: [★★★★] 4/4
*
*   NOTE: This example requires raylib OpenGL 3.3 for shaders support and only #version 330
*         is currently supported. OpenGL ES 2.0 platforms are not supported at the moment
*
*   Example originally created with raylib 2.0, last time updated with raylib 4.2
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2018-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using static Raylib_cs.Raylib;
using static Raylib_cs.ConfigFlags;

namespace Examples.Shaders;

public partial class Raymarching
{
    public const int GlslVersion = 330;

    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        int screenWidth = 800;
        int screenHeight = 450;

        SetConfigFlags(ResizableWindow);
        InitWindow(screenWidth, screenHeight, "raylib [shaders] example - raymarching rendering");

        Camera3D camera = new();
        camera.Position = new Vector3(2.5f, 2.5f, 3.0f);    // Camera position
        camera.Target = new Vector3(0.0f, 0.0f, 0.7f);      // Camera looking at point
        camera.Up = new Vector3(0.0f, 1.0f, 0.0f);          // Camera up vector (rotation towards target)
        camera.FovY = 65.0f;                                // Camera field-of-view Y
        camera.Projection = CameraProjection.Perspective;   // Camera projection type

        // Load raymarching shader
        // NOTE: Defining 0 (NULL) for vertex shader forces usage of internal default vertex shader
        Shader shader = LoadShader(null, $"resources/shaders/glsl{GlslVersion}/raymarching.fs");

        // Get shader locations for required uniforms
        int viewEyeLoc = GetShaderLocation(shader, "viewEye");
        int viewCenterLoc = GetShaderLocation(shader, "viewCenter");
        int runTimeLoc = GetShaderLocation(shader, "runTime");
        int resolutionLoc = GetShaderLocation(shader, "resolution");

        float[] resolution = { (float)screenWidth, (float)screenHeight };
        Raylib.SetShaderValue(shader, resolutionLoc, resolution, ShaderUniformDataType.Vec2);

        float runTime = 0.0f;

        DisableCursor();                    // Limit cursor to relative movement inside the window
        SetTargetFPS(60);                   // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())        // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            UpdateCamera(ref camera, CameraMode.FirstPerson);

            float deltaTime = GetFrameTime();
            runTime += deltaTime;

            // Set shader required uniform values
            Raylib.SetShaderValue(shader, viewEyeLoc, camera.Position, ShaderUniformDataType.Vec3);
            Raylib.SetShaderValue(shader, viewCenterLoc, camera.Target, ShaderUniformDataType.Vec3);
            Raylib.SetShaderValue(shader, runTimeLoc, runTime, ShaderUniformDataType.Float);

            // Check if screen is resized
            if (IsWindowResized())
            {
                screenWidth = GetScreenWidth();
                screenHeight = GetScreenHeight();
                resolution = new float[] { (float)screenWidth, (float)screenHeight };
                Raylib.SetShaderValue(shader, resolutionLoc, resolution, ShaderUniformDataType.Vec2);
            }
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // We only draw a white full-screen rectangle,
            // frame is generated in shader using raymarching
            BeginShaderMode(shader);
            DrawRectangle(0, 0, screenWidth, screenHeight, Color.White);
            EndShaderMode();

            DrawText(
                "(c) Raymarching shader by Iñigo Quilez. MIT License.",
                screenWidth - 280,
                screenHeight - 20,
                10,
                Color.Black
            );

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadShader(shader);           // Unload shader

        CloseWindow();                  // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
