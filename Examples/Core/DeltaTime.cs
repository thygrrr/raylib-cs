/*******************************************************************************************
 *
 *   raylib [core] example - delta time
 *
 *   Example complexity rating: [★☆☆☆] 1/4
 *
 *   Example originally created with raylib 5.5, last time updated with raylib 6.0
 *
 *   Example contributed by Robin (@RobinsAviary) and reviewed by Ramon Santamaria (@raysan5)
 *
 *   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
 *   BSD-like license that allows static linking with closed source software
 *
 *   Copyright (c) 2025 Robin (@RobinsAviary)
 *
 ********************************************************************************************/

using System.Numerics;

namespace Examples.Core;

using static Raylib_cs.Raylib;

public partial class DeltaTime : IExample
{
    const int screenWidth = 800;
    const int screenHeight = 450;

    // The speed applied to both circles
    const float speed = 10.0f;
    const float circleRadius = 32.0f;

    int currentFps;

    // Store the position for the both of the circles
    Vector2 deltaCircle;
    Vector2 frameCircle;

    public string Name => "Core / Delta Time";

    // One-time setup (was the code before the original while loop, minus InitWindow).
    public void Init()
    {
        currentFps = 60;

        // Store the position for the both of the circles
        deltaCircle = new Vector2(0, (float)screenHeight / 3.0f);
        frameCircle = new Vector2(0, (float)screenHeight * (2.0f / 3.0f));
    }

    // A single frame (was the body of the original while loop).
    public void Update()
    {
        // Update
        //----------------------------------------------------------------------------------
        // Adjust the FPS target based on the mouse wheel
        float mouseWheel = GetMouseWheelMove();
        if (mouseWheel != 0)
        {
            currentFps += (int)mouseWheel;
            if (currentFps < 0)
            {
                currentFps = 0;
            }
            SetTargetFPS(currentFps);
        }

        // GetFrameTime() returns the time it took to draw the last frame, in seconds (usually called delta time)
        // Uses the delta time to make the circle look like it's moving at a "consistent" speed regardless of FPS

        // Multiply by 6.0 (an arbitrary value) in order to make the speed
        // visually closer to the other circle (at 60 fps), for comparison
        deltaCircle.X += GetFrameTime() * 6.0f * speed;
        // This circle can move faster or slower visually depending on the FPS
        frameCircle.X += 0.1f * speed;

        // If either circle is off the screen, reset it back to the start
        if (deltaCircle.X > screenWidth)
        {
            deltaCircle.X = 0;
        }

        if (frameCircle.X > screenWidth)
        {
            frameCircle.X = 0;
        }

        // Reset both circles positions
        if (IsKeyPressed(KeyboardKey.R))
        {
            deltaCircle.X = 0;
            frameCircle.X = 0;
        }
        //----------------------------------------------------------------------------------

        // Draw
        //----------------------------------------------------------------------------------
        BeginDrawing();
        ClearBackground(Color.RayWhite);

        // Draw both circles to the screen
        DrawCircleV(deltaCircle, circleRadius, Color.Red);
        DrawCircleV(frameCircle, circleRadius, Color.Blue);

        // Draw the help text
        // Determine what help text to show depending on the current FPS target
        var fpsText = "";
        if (currentFps <= 0)
        {
            fpsText = $"FPS: unlimited ({GetFPS()})";
        }
        else
        {
            fpsText = $"FPS: {GetFPS()} (target: {currentFps})";
        }
        DrawText(fpsText, 10, 10, 20, Color.DarkGray);
        DrawText($"Frame time: {GetFrameTime():F2} ms", 10, 30, 20, Color.DarkGray);
        DrawText("Use the scroll wheel to change the fps limit, r to reset", 10, 50, 20, Color.DarkGray);

        // Draw the text above the circles
        DrawText("FUNC: x += GetFrameTime()*speed", 10, 90, 20, Color.Red);
        DrawText("FUNC: x += speed", 10, 240, 20, Color.Blue);

        EndDrawing();
        //----------------------------------------------------------------------------------
    }

    // Free resources (was the code after the loop, minus CloseWindow).
    public void Unload()
    {
    }

    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        InitWindow(screenWidth, screenHeight, "raylib [core] example - delta time");

        SetTargetFPS(60);
        //--------------------------------------------------------------------------------------

        var game = new DeltaTime();
        game.Init();

        // Main game loop
        while (!WindowShouldClose()) // Detect window close button or ESC key
        {
            game.Update();
        }

        game.Unload();

        // De-Initialization
        //--------------------------------------------------------------------------------------
        CloseWindow();        // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
