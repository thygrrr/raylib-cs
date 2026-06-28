/*******************************************************************************************
*
*   raylib [textures] example - sprite animation
*
*   Example complexity rating: [★★☆☆] 2/4
*
*   Example originally created with raylib 1.3, last time updated with raylib 1.3
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2014-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Examples.Textures;

public partial class SpriteAnim
{
    public const int MaxFrameSpeed = 15;
    public const int MinFrameSpeed = 1;

    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [textures] example - sprite animation");

        // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
        Texture2D scarfy = LoadTexture("resources/scarfy.png");        // Texture loading

        Vector2 position = new(350.0f, 280.0f);
        Rectangle frameRec = new(0.0f, 0.0f, (float)scarfy.Width / 6, (float)scarfy.Height);
        int currentFrame = 0;

        int framesCounter = 0;
        int framesSpeed = 8;            // Number of spritesheet frames shown by second

        SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            framesCounter++;

            if (framesCounter >= (60 / framesSpeed))
            {
                framesCounter = 0;
                currentFrame++;

                if (currentFrame > 5)
                {
                    currentFrame = 0;
                }

                frameRec.X = (float)currentFrame * (float)scarfy.Width / 6;
            }

            if (IsKeyPressed(KeyboardKey.Right))
            {
                framesSpeed++;
            }
            else if (IsKeyPressed(KeyboardKey.Left))
            {
                framesSpeed--;
            }

            framesSpeed = Math.Clamp(framesSpeed, MinFrameSpeed, MaxFrameSpeed);
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawTexture(scarfy, 15, 40, Color.White);
            DrawRectangleLines(15, 40, scarfy.Width, scarfy.Height, Color.Lime);
            DrawRectangleLines(
                15 + (int)frameRec.X,
                40 + (int)frameRec.Y,
                (int)frameRec.Width,
                (int)frameRec.Height,
                Color.Red
            );

            DrawText("FRAME SPEED: ", 165, 210, 10, Color.DarkGray);
            DrawText($"{framesSpeed:D2} FPS", 575, 210, 10, Color.DarkGray);
            DrawText("PRESS RIGHT/LEFT KEYS to CHANGE SPEED!", 290, 240, 10, Color.DarkGray);

            for (int i = 0; i < MaxFrameSpeed; i++)
            {
                if (i < framesSpeed)
                {
                    DrawRectangle(250 + 21 * i, 205, 20, 20, Color.Red);
                }
                DrawRectangleLines(250 + 21 * i, 205, 20, 20, Color.Maroon);
            }

            DrawTextureRec(scarfy, frameRec, position, Color.White);  // Draw part of the texture

            DrawText("(c) Scarfy sprite by Eiden Marsal", screenWidth - 200, screenHeight - 20, 10, Color.Gray);

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadTexture(scarfy);       // Texture unloading

        CloseWindow();                // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
