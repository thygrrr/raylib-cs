/*******************************************************************************************
*
*   raylib [models] example - billboard rendering
*
*   Example complexity rating: [★★★☆] 3/4
*
*   Example originally created with raylib 1.3, last time updated with raylib 3.5
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

public partial class BillboardDemo
{
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [models] example - billboard rendering");

        // Define the camera to look into our 3d world
        Camera3D camera = new();
        camera.Position = new Vector3(5.0f, 4.0f, 5.0f);    // Camera position
        camera.Target = new Vector3(0.0f, 2.0f, 0.0f);      // Camera looking at point
        camera.Up = new Vector3(0.0f, 1.0f, 0.0f);          // Camera up vector (rotation towards target)
        camera.FovY = 45.0f;                                // Camera field-of-view Y
        camera.Projection = CameraProjection.Perspective;   // Camera projection type

        Texture2D bill = LoadTexture("resources/billboard.png");    // Our billboard texture
        Vector3 billPositionStatic = new(0.0f, 2.0f, 0.0f);         // Position of static billboard
        Vector3 billPositionRotating = new(1.0f, 2.0f, 1.0f);       // Position of rotating billboard

        // Entire billboard texture, source is used to take a segment from a larger texture
        Rectangle source = new(0.0f, 0.0f, (float)bill.Width, (float)bill.Height);

        // NOTE: Billboard locked on axis-Y
        Vector3 billUp = new(0.0f, 1.0f, 0.0f);

        // Set the height of the rotating billboard to 1.0 with the aspect ratio fixed
        Vector2 size = new(source.Width / source.Height, 1.0f);

        // Rotate around origin
        // Here we choose to rotate around the image center
        Vector2 origin = size * 0.5f;

        // Distance is needed for the correct billboard draw order
        // Larger distance (further away from the camera) should be drawn prior to smaller distance
        float distanceStatic;
        float distanceRotating;
        float rotation = 0.0f;

        SetTargetFPS(60);                   // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())        // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            UpdateCamera(ref camera, CameraMode.Orbital);

            rotation += 0.4f;
            distanceStatic = Vector3.Distance(camera.Position, billPositionStatic);
            distanceRotating = Vector3.Distance(camera.Position, billPositionRotating);
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(camera);

            DrawGrid(10, 1.0f);        // Draw a grid

            // Draw order matters!
            if (distanceStatic > distanceRotating)
            {
                DrawBillboard(camera, bill, billPositionStatic, 2.0f, Color.White);
                DrawBillboardPro(camera, bill, source, billPositionRotating, billUp, size, origin, rotation, Color.White);
            }
            else
            {
                DrawBillboardPro(camera, bill, source, billPositionRotating, billUp, size, origin, rotation, Color.White);
                DrawBillboard(camera, bill, billPositionStatic, 2.0f, Color.White);
            }

            EndMode3D();

            DrawFPS(10, 10);

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadTexture(bill);        // Unload texture

        CloseWindow();              // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}

