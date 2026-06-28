/*******************************************************************************************
*
*   raylib [models] example - heightmap rendering
*
*   Example complexity rating: [★☆☆☆] 1/4
*
*   Example originally created with raylib 1.8, last time updated with raylib 3.5
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2015-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using static Raylib_cs.Raylib;

namespace Examples.Models;

public partial class HeightmapDemo
{
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [models] example - heightmap rendering");

        // Define our custom camera to look into our 3d world
        Camera3D camera = new();
        camera.Position = new Vector3(18.0f, 21.0f, 18.0f);     // Camera position
        camera.Target = new Vector3(0.0f, 0.0f, 0.0f);          // Camera looking at point
        camera.Up = new Vector3(0.0f, 1.0f, 0.0f);              // Camera up vector (rotation towards target)
        camera.FovY = 45.0f;                                    // Camera field-of-view Y
        camera.Projection = CameraProjection.Perspective;       // Camera projection type

        Image image = LoadImage("resources/heightmap.png");     // Load heightmap image (RAM)
        Texture2D texture = LoadTextureFromImage(image);        // Convert image to texture (VRAM)

        Mesh mesh = GenMeshHeightmap(image, new Vector3(16, 8, 16)); // Generate heightmap mesh (RAM and VRAM)
        Model model = LoadModelFromMesh(mesh);                  // Load model from generated mesh

        // Set map diffuse texture
        Raylib.SetMaterialTexture(ref model, 0, MaterialMapIndex.Albedo, ref texture);

        Vector3 mapPosition = new(-8.0f, 0.0f, -8.0f);          // Define model position

        UnloadImage(image);             // Unload heightmap image from RAM, already uploaded to VRAM

        SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            UpdateCamera(ref camera, CameraMode.Orbital);
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(camera);

            DrawModel(model, mapPosition, 1.0f, Color.Red);

            DrawGrid(20, 1.0f);

            EndMode3D();

            DrawTexture(texture, screenWidth - texture.Width - 20, 20, Color.White);
            DrawRectangleLines(screenWidth - texture.Width - 20, 20, texture.Width, texture.Height, Color.Green);

            DrawFPS(10, 10);

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadTexture(texture);     // Unload texture
        UnloadModel(model);         // Unload model

        CloseWindow();              // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
