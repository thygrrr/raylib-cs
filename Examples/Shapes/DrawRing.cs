/*******************************************************************************************
*
*   raylib [shapes] example - ring drawing
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

public partial class DrawRing
{
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [shapes] example - ring drawing");

        Vector2 center = new((GetScreenWidth() - 300) / 2.0f, GetScreenHeight() / 2.0f);

        float innerRadius = 80.0f;
        float outerRadius = 190.0f;

        float startAngle = 0.0f;
        float endAngle = 360.0f;
        float segments = 0.0f;

        bool drawRing = true;
        bool drawRingLines = false;
        bool drawCircleLines = false;

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

            if (drawRing)
            {
                DrawRing(
                    center,
                    innerRadius,
                    outerRadius,
                    startAngle,
                    endAngle,
                    (int)segments,
                    Fade(Color.Maroon, 0.3f)
                );
            }
            if (drawRingLines)
            {
                DrawRingLines(
                    center,
                    innerRadius,
                    outerRadius,
                    startAngle,
                    endAngle,
                    (int)segments,
                    Fade(Color.Black, 0.4f)
                );
            }
            if (drawCircleLines)
            {
                DrawCircleSectorLines(
                    center,
                    outerRadius,
                    startAngle,
                    endAngle,
                    (int)segments,
                    Fade(Color.Black, 0.4f)
                );
            }

            // Draw GUI controls
            //------------------------------------------------------------------------------
            /*GuiSliderBar(new Rectangle( 600, 40, 120, 20 ), "StartAngle", TextFormat("%.2f", startAngle), ref startAngle, -450, 450);
            GuiSliderBar(new Rectangle( 600, 70, 120, 20 ), "EndAngle", TextFormat("%.2f", endAngle), ref endAngle, -450, 450);

            GuiSliderBar(new Rectangle( 600, 140, 120, 20 ), "InnerRadius", TextFormat("%.2f", innerRadius), ref innerRadius, 0, 100);
            GuiSliderBar(new Rectangle( 600, 170, 120, 20 ), "OuterRadius", TextFormat("%.2f", outerRadius), ref outerRadius, 0, 200);

            GuiSliderBar(new Rectangle( 600, 240, 120, 20 ), "Segments", TextFormat("%.2f", segments), ref segments, 0, 100);

            GuiCheckBox(new Rectangle( 600, 320, 20, 20 ), "Draw Ring", ref drawRing);
            GuiCheckBox(new Rectangle( 600, 350, 20, 20 ), "Draw RingLines", ref drawRingLines);
            GuiCheckBox(new Rectangle( 600, 380, 20, 20 ), "Draw CircleLines", ref drawCircleLines);*/
            //------------------------------------------------------------------------------

            int minSegments = (int)MathF.Ceiling((endAngle - startAngle) / 90);
            Color color = (segments >= minSegments) ? Color.Maroon : Color.DarkGray;
            DrawText($"MODE: {((segments >= minSegments) ? "MANUAL" : "AUTO")}", 600, 270, 10, color);

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
