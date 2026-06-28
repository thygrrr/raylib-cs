/*******************************************************************************************
*
*   raylib [models] example - yaw pitch roll
*
*   Example complexity rating: [★★☆☆] 2/4
*
*   Example originally created with raylib 1.8, last time updated with raylib 4.0
*
*   Example contributed by Berni (@Berni8k) and reviewed by Ramon Santamaria (@raysan5)
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2017-2025 Berni (@Berni8k) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;

namespace Examples.Models;

public partial class YawPitchRoll
{
    public unsafe static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [models] example - yaw pitch roll");

        Camera3D camera = new();
        camera.Position = new Vector3(0.0f, 50.0f, -120.0f);// Camera position perspective
        camera.Target = new Vector3(0.0f, 0.0f, 0.0f);      // Camera looking at point
        camera.Up = new Vector3(0.0f, 1.0f, 0.0f);          // Camera up vector (rotation towards target)
        camera.FovY = 30.0f;                                // Camera field-of-view Y
        camera.Projection = CameraProjection.Perspective;   // Camera type

        Model model = LoadModel("resources/models/obj/plane.obj");                  // Load model
        Texture2D texture = LoadTexture("resources/models/obj/plane_diffuse.png");  // Load model texture

        SetTextureWrap(texture, TextureWrap.Repeat);        // Force Repeat to avoid issue on Web version

        model.Materials[0].Maps[(int)MaterialMapIndex.Diffuse].Texture = texture;   // Set map diffuse texture

        float pitch = 0.0f;
        float roll = 0.0f;
        float yaw = 0.0f;

        SetTargetFPS(60);
        //--------------------------------------------------------------------------------------

        while (!WindowShouldClose())
        {
            // Update
            //----------------------------------------------------------------------------------

            // Plane pitch (x-axis) controls
            if (IsKeyDown(KeyboardKey.Down))
            {
                pitch += 0.6f;
            }
            else if (IsKeyDown(KeyboardKey.Up))
            {
                pitch -= 0.6f;
            }
            else
            {
                if (pitch > 0.3f)
                {
                    pitch -= 0.3f;
                }
                else if (pitch < -0.3f)
                {
                    pitch += 0.3f;
                }
            }

            // Plane yaw (y-axis) controls
            if (IsKeyDown(KeyboardKey.S))
            {
                yaw += 1.0f;
            }
            else if (IsKeyDown(KeyboardKey.A))
            {
                yaw -= 1.0f;
            }
            else
            {
                if (yaw > 0.0f)
                {
                    yaw -= 0.5f;
                }
                else if (yaw < 0.0f)
                {
                    yaw += 0.5f;
                }
            }

            // Plane roll (z-axis) controls
            if (IsKeyDown(KeyboardKey.Left))
            {
                roll += 1.0f;
            }
            else if (IsKeyDown(KeyboardKey.Right))
            {
                roll -= 1.0f;
            }
            else
            {
                if (roll > 0.0f)
                {
                    roll -= 0.5f;
                }
                else if (roll < 0.0f)
                {
                    roll += 0.5f;
                }
            }

            // Tranformation matrix for rotations
            model.Transform = MatrixRotateXYZ(new Vector3(DEG2RAD * pitch, DEG2RAD * yaw, DEG2RAD * roll));
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // Draw 3D model (recomended to draw 3D always before 2D)
            BeginMode3D(camera);

            // Draw 3d model with texture
            DrawModel(model, new Vector3(0.0f, -8.0f, 0.0f), 1.0f, Color.White);
            DrawGrid(10, 10.0f);

            EndMode3D();

            // Draw controls info
            DrawRectangle(30, 370, 260, 70, Fade(Color.Green, 0.5f));
            DrawRectangleLines(30, 370, 260, 70, Fade(Color.DarkGreen, 0.5f));
            DrawText("Pitch controlled with: KEY_UP / KEY_DOWN", 40, 380, 10, Color.DarkGray);
            DrawText("Roll controlled with: KEY_LEFT / KEY_RIGHT", 40, 400, 10, Color.DarkGray);
            DrawText("Yaw controlled with: KEY_A / KEY_S", 40, 420, 10, Color.DarkGray);

            DrawText(
                "(c) WWI Plane Model created by GiaHanLam",
                screenWidth - 240,
                screenHeight - 20,
                10,
                Color.DarkGray
            );

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadModel(model);

        CloseWindow();
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
