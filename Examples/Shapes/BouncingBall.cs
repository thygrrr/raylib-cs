/*******************************************************************************************
*
*   raylib [shapes] example - bouncing ball
*
*   Example complexity rating: [★☆☆☆] 1/4
*
*   Example originally created with raylib 2.5, last time updated with raylib 2.5
*
*   Example contributed by Ramon Santamaria (@raysan5), reviewed by Jopestpe (@jopestpe)
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2013-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using static Raylib_cs.Raylib;

namespace Examples.Shapes;

public partial class BouncingBall
{
    public static int Main()
    {
        // Initialization
        //---------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        SetConfigFlags(ConfigFlags.Msaa4xHint);
        InitWindow(screenWidth, screenHeight, "raylib [shapes] example - bouncing ball");

        Vector2 ballPosition = new(GetScreenWidth() / 2.0f, GetScreenHeight() / 2.0f);
        Vector2 ballSpeed = new(5.0f, 4.0f);
        int ballRadius = 20;
        float gravity = 0.2f;

        bool useGravity = true;
        bool pause = false;
        int framesCounter = 0;

        SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
        //----------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //-----------------------------------------------------
            if (IsKeyPressed(KeyboardKey.G))
            {
                useGravity = !useGravity;
            }
            if (IsKeyPressed(KeyboardKey.Space))
            {
                pause = !pause;
            }

            if (!pause)
            {
                ballPosition.X += ballSpeed.X;
                ballPosition.Y += ballSpeed.Y;

                if (useGravity)
                {
                    ballSpeed.Y += gravity;
                }

                // Check walls collision for bouncing
                if ((ballPosition.X >= (GetScreenWidth() - ballRadius)) || (ballPosition.X <= ballRadius))
                {
                    ballSpeed.X *= -1.0f;
                }
                if ((ballPosition.Y >= (GetScreenHeight() - ballRadius)) || (ballPosition.Y <= ballRadius))
                {
                    ballSpeed.Y *= -0.95f;
                }
            }
            else
            {
                framesCounter++;
            }
            //-----------------------------------------------------

            // Draw
            //-----------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawCircleV(ballPosition, (float)ballRadius, Color.Maroon);
            DrawText("PRESS SPACE to PAUSE BALL MOVEMENT", 10, GetScreenHeight() - 25, 20, Color.LightGray);

            if (useGravity)
            {
                DrawText("GRAVITY: ON (Press G to disable)", 10, GetScreenHeight() - 50, 20, Color.DarkGreen);
            }
            else
            {
                DrawText("GRAVITY: OFF (Press G to enable)", 10, GetScreenHeight() - 50, 20, Color.Red);
            }

            // On pause, we draw a blinking message
            if (pause && ((framesCounter / 30) % 2) != 0)
            {
                DrawText("PAUSED", 350, 200, 30, Color.Gray);
            }

            DrawFPS(10, 10);

            EndDrawing();
            //-----------------------------------------------------
        }

        // De-Initialization
        //---------------------------------------------------------
        CloseWindow();        // Close window and OpenGL context
        //----------------------------------------------------------

        return 0;
    }
}

