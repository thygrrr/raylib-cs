/*******************************************************************************************
*
*   raylib [shapes] example - circle sector drawing
*
*   Example complexity rating: [★★★☆] 3/4
*
*   Example originally created with raylib 2.5, last time updated with raylib 2.5
*
*   Example contributed by Vlad Adrian (@demizdor) and reviewed by Ramon Santamaria (@raysan5)
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2018-2025 Vlad Adrian (@demizdor) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Examples.Shapes;

public partial class DrawCircleSector
{
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [shapes] example - circle sector drawing");

        Vector2 center = new((GetScreenWidth() - 300) / 2.0f, GetScreenHeight() / 2.0f);

        float outerRadius = 180.0f;
        float startAngle = 0.0f;
        float endAngle = 180.0f;
        float segments = 10.0f;
        float minSegments = 4;

        SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            // NOTE: All variables update happens inside GUI control functions
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawLine(500, 0, 500, GetScreenHeight(), Fade(Color.LightGray, 0.6f));
            DrawRectangle(500, 0, GetScreenWidth() - 500, GetScreenHeight(), Fade(Color.LightGray, 0.3f));

            DrawCircleSector(center, outerRadius, startAngle, endAngle, (int)segments, Fade(Color.Maroon, 0.3f));
            DrawCircleSectorLines(
                center,
                outerRadius,
                startAngle,
                endAngle,
                (int)segments,
                Fade(Color.Maroon, 0.6f)
            );

            // Draw GUI controls
            //------------------------------------------------------------------------------
            /*GuiSliderBar(new Rectangle( 600, 40, 120, 20), "StartAngle", TextFormat("%.2f", startAngle), ref startAngle, 0, 720);
            GuiSliderBar(new Rectangle( 600, 70, 120, 20), "EndAngle", TextFormat("%.2f", endAngle), ref endAngle, 0, 720);

            GuiSliderBar(new Rectangle( 600, 140, 120, 20), "Radius", TextFormat("%.2f", outerRadius), ref outerRadius, 0, 200);
            GuiSliderBar(new Rectangle( 600, 170, 120, 20), "Segments", TextFormat("%.2f", segments), ref segments, 0, 100);*/
            //------------------------------------------------------------------------------

            minSegments = MathF.Truncate(MathF.Ceiling((endAngle - startAngle) / 90));
            Color color = (segments >= minSegments) ? Color.Maroon : Color.DarkGray;
            DrawText($"MODE: {((segments >= minSegments) ? "MANUAL" : "AUTO")}", 600, 200, 10, color);

            DrawFPS(10, 10);

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        CloseWindow();        // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}

