/*******************************************************************************************
*
*   raylib [models] example - first person maze
*
*   Example complexity rating: [★★☆☆] 2/4
*
*   Example originally created with raylib 2.5, last time updated with raylib 3.5
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2019-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using static Raylib_cs.Raylib;

namespace Examples.Models;

public partial class FirstPersonMaze
{
    public unsafe static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [models] example - first person maze");

        // Define the camera to look into our 3d world
        Camera3D camera = new();
        camera.Position = new Vector3(0.2f, 0.4f, 0.2f);    // Camera position
        camera.Target = new Vector3(0.185f, 0.4f, 0.0f);    // Camera looking at point
        camera.Up = new Vector3(0.0f, 1.0f, 0.0f);          // Camera up vector (rotation towards target)
        camera.FovY = 45.0f;                                // Camera field-of-view Y
        camera.Projection = CameraProjection.Perspective;   // Camera projection type

        Image imMap = LoadImage("resources/cubicmap.png");      // Load cubicmap image (RAM)
        Texture2D cubicmap = LoadTextureFromImage(imMap);       // Convert image to texture to display (VRAM)
        Mesh mesh = GenMeshCubicmap(imMap, new Vector3(1.0f, 1.0f, 1.0f));
        Model model = LoadModelFromMesh(mesh);

        // NOTE: By default each cube is mapped to one part of texture atlas
        Texture2D texture = LoadTexture("resources/cubicmap_atlas.png");    // Load map texture

        // Set map diffuse texture
        Raylib.SetMaterialTexture(ref model, 0, MaterialMapIndex.Albedo, ref texture);

        // Get map image data to be used for collision detection
        Color* mapPixels = LoadImageColors(imMap);
        UnloadImage(imMap);             // Unload image from RAM

        Vector3 mapPosition = new(-16.0f, 0.0f, -8.0f);  // Set model position

        DisableCursor();                // Limit cursor to relative movement inside the window

        SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            Vector3 oldCamPos = camera.Position;    // Store old camera position

            UpdateCamera(ref camera, CameraMode.FirstPerson);

            // Check player collision (we simplify to 2D collision detection)
            Vector2 playerPos = new(camera.Position.X, camera.Position.Z);
            float playerRadius = 0.1f;  // Collision radius (player is modelled as a cilinder for collision)

            int playerCellX = (int)(playerPos.X - mapPosition.X + 0.5f);
            int playerCellY = (int)(playerPos.Y - mapPosition.Z + 0.5f);

            // Out-of-limits security check
            if (playerCellX < 0)
            {
                playerCellX = 0;
            }
            else if (playerCellX >= cubicmap.Width)
            {
                playerCellX = cubicmap.Width - 1;
            }

            if (playerCellY < 0)
            {
                playerCellY = 0;
            }
            else if (playerCellY >= cubicmap.Height)
            {
                playerCellY = cubicmap.Height - 1;
            }

            // Check map collisions using image data and player position against surrounding cells only
            for (int y = playerCellY - 1; y <= playerCellY + 1; y++)
            {
                // Avoid map accessing out of bounds
                if ((y >= 0) && (y < cubicmap.Height))
                {
                    for (int x = playerCellX - 1; x <= playerCellX + 1; x++)
                    {
                        // NOTE: Collision: Only checking R channel for white pixel
                        if (((x >= 0) && (x < cubicmap.Width)) &&
                            (mapPixels[y * cubicmap.Width + x].R == 255) &&
                            (CheckCollisionCircleRec(playerPos, playerRadius,
                            new Rectangle(mapPosition.X - 0.5f + x * 1.0f, mapPosition.Z - 0.5f + y * 1.0f, 1.0f, 1.0f))))
                        {
                            // Collision detected, reset camera position
                            camera.Position = oldCamPos;
                        }
                    }
                }
            }
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(camera);
            DrawModel(model, mapPosition, 1.0f, Color.White);                     // Draw maze map
            EndMode3D();

            DrawTextureEx(cubicmap, new Vector2(GetScreenWidth() - cubicmap.Width * 4 - 20, 20), 0.0f, 4.0f, Color.White);
            DrawRectangleLines(GetScreenWidth() - cubicmap.Width * 4 - 20, 20, cubicmap.Width * 4, cubicmap.Height * 4, Color.Green);

            // Draw player position radar
            DrawRectangle(GetScreenWidth() - cubicmap.Width * 4 - 20 + playerCellX * 4, 20 + playerCellY * 4, 4, 4, Color.Red);

            DrawFPS(10, 10);

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadImageColors(mapPixels);   // Unload color array

        UnloadTexture(cubicmap);        // Unload cubicmap texture
        UnloadTexture(texture);         // Unload map texture
        UnloadModel(model);             // Unload map model

        CloseWindow();                  // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
