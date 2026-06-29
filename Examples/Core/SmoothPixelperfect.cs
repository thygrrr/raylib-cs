/*******************************************************************************************
*
*   raylib [core] example - smooth pixelperfect
*
*   Example complexity rating: [★★★☆] 3/4
*
*   Example originally created with raylib 3.7, last time updated with raylib 4.0
*
*   Example contributed by Giancamillo Alessandroni (@NotManyIdeasDev) and
*   reviewed by Ramon Santamaria (@raysan5)
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2021-2025 Giancamillo Alessandroni (@NotManyIdeasDev) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Examples.Core;

public partial class SmoothPixelPerfect : IExample
{
    const int screenWidth = 800;
    const int screenHeight = 450;

    const int virtualScreenWidth = 160;
    const int virtualScreenHeight = 90;

    const float virtualRatio = (float)screenWidth / (float)virtualScreenWidth;

    public string Name => "Core / Smooth Pixelperfect";

    Camera2D worldSpaceCamera;  // Game world camera
    Camera2D screenSpaceCamera; // Smoothing camera
    RenderTexture2D target;

    Rectangle rec01;
    Rectangle rec02;
    Rectangle rec03;

    Rectangle sourceRec;
    Rectangle destRec;

    Vector2 origin;
    float rotation;
    float cameraX;
    float cameraY;
    bool smoothOn;
    bool overscan;

    // One-time setup (was the code before the original while loop, minus InitWindow).
    public void Init()
    {
        worldSpaceCamera = new();  // Game world camera
        worldSpaceCamera.Zoom = 1.0f;

        screenSpaceCamera = new(); // Smoothing camera
        screenSpaceCamera.Zoom = 1.0f;

        // Load render texture to draw all our objects
        target = LoadRenderTexture(virtualScreenWidth, virtualScreenHeight);

        rec01 = new(70.0f, 35.0f, 20.0f, 20.0f);
        rec02 = new(90.0f, 55.0f, 30.0f, 10.0f);
        rec03 = new(80.0f, 65.0f, 15.0f, 25.0f);

        // The target's height is flipped (in the source Rectangle), due to OpenGL reasons
        sourceRec = new(
            0.0f,
            0.0f,
            (float)target.Texture.Width,
            -(float)target.Texture.Height
        );
        destRec = new(
            (screenWidth - screenWidth / 1.25f) / 2.0f,
            (screenHeight - screenHeight / 1.25f) / 2.0f,
            screenWidth / 1.25f,
            screenHeight / 1.25f
        );

        origin = new(0.0f, 0.0f);

        rotation = 0.0f;

        cameraX = 0.0f;
        cameraY = 0.0f;

        smoothOn = true;
        overscan = false;
    }

    // A single frame (was the body of the original while loop).
    public void Update()
    {
        // Update
        //----------------------------------------------------------------------------------
        rotation += 60.0f * GetFrameTime();   // Rotate the rectangles, 60 degrees per second

        // Make the camera move to demonstrate the effect
        cameraX = (MathF.Sin((float)GetTime()) * 50.0f) - 10.0f;
        cameraY = MathF.Cos((float)GetTime()) * 30.0f;

        // Set the camera's target to the values computed above
        screenSpaceCamera.Target = new Vector2(cameraX, cameraY);

        // Round worldSpace coordinates, keep decimals into screenSpace coordinates
        worldSpaceCamera.Target.X = MathF.Truncate(screenSpaceCamera.Target.X);
        screenSpaceCamera.Target.X -= worldSpaceCamera.Target.X;
        screenSpaceCamera.Target.X *= virtualRatio;

        worldSpaceCamera.Target.Y = MathF.Truncate(screenSpaceCamera.Target.Y);
        screenSpaceCamera.Target.Y -= worldSpaceCamera.Target.Y;
        screenSpaceCamera.Target.Y *= virtualRatio;

        if (IsKeyPressed(KeyboardKey.S))
        {
            smoothOn = !smoothOn;
        }

        if (IsKeyPressed(KeyboardKey.O))
        {
            overscan = !overscan;
        }

        if (overscan)
        {
            destRec = new Rectangle(
                -virtualRatio,
                -virtualRatio,
                screenWidth + (virtualRatio * 2),
                screenHeight + (virtualRatio * 2)
            );
        }
        else
        {
            destRec = new Rectangle(
                (screenWidth - screenWidth / 1.25f) / 2.0f,
                (screenHeight - screenHeight / 1.25f) / 2.0f,
                screenWidth / 1.25f,
                screenHeight / 1.25f
            );
        }
        //----------------------------------------------------------------------------------

        // Draw
        //----------------------------------------------------------------------------------
        BeginTextureMode(target);
        ClearBackground(Color.RayWhite);

        BeginMode2D(worldSpaceCamera);
        DrawRectanglePro(rec01, origin, rotation, Color.Black);
        DrawRectanglePro(rec02, origin, -rotation, Color.Red);
        DrawRectanglePro(rec03, origin, rotation + 45.0f, Color.Blue);
        EndMode2D();
        EndTextureMode();

        BeginDrawing();
        ClearBackground(Color.LightGray);

        if (smoothOn)
        {
            BeginMode2D(screenSpaceCamera);
            DrawTexturePro(target.Texture, sourceRec, destRec, origin, 0.0f, Color.White);
            EndMode2D();
        }
        else
        {
            DrawTexturePro(target.Texture, sourceRec, destRec, origin, 0.0f, Color.White);
        }

        DrawText($"Screen resolution: {screenWidth}x{screenHeight}", 10, 10, 20, Color.DarkBlue);
        DrawText($"World resolution: {virtualScreenWidth}x{virtualScreenHeight}", 10, 40, 20, Color.DarkGreen);
        DrawText($"Smooth: {(smoothOn ? "ON" : "OFF")}", 10, screenHeight - 60, 20, Color.Red);
        DrawText($"Overscan: {(overscan ? "ON" : "OFF")}", 10, screenHeight - 30, 20, Color.Red);
        DrawFPS(GetScreenWidth() - 95, 10);
        EndDrawing();
        //----------------------------------------------------------------------------------
    }

    // Free resources (was the code after the loop, minus CloseWindow).
    public void Unload()
    {
        UnloadRenderTexture(target);    // Unload render texture
    }

    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        InitWindow(screenWidth, screenHeight, "raylib [core] example - smooth pixelperfect");

        SetTargetFPS(60);
        //--------------------------------------------------------------------------------------

        var game = new SmoothPixelPerfect();
        game.Init();

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            game.Update();
        }

        game.Unload();

        // De-Initialization
        //--------------------------------------------------------------------------------------
        CloseWindow();                  // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
